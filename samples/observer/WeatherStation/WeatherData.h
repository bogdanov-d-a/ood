#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include <climits>
#include <string>
#include <functional>
#include <map>
#define _USE_MATH_DEFINES
#include <math.h>
#include <boost/optional.hpp>
#include "Observer.h"

using namespace std;

struct SWindData
{
	double speed = 0;
	double direction = 0;
};

ostream& operator<<(ostream &stream, SWindData const& wind)
{
	stream << "speed " << wind.speed << " direction " << wind.direction;
	return stream;
}

struct SWeatherInfo
{
	double temperature = 0;
	double humidity = 0;
	double pressure = 0;
};

struct SWeatherInfoWind : public SWeatherInfo
{
	SWindData wind;
};

using SenderNameProvider = std::function<std::string(const void*)>;

class CWeatherObserverHelper : public IObserver<SWeatherInfo>
{
public:
	virtual void UpdateWeather(SWeatherInfo const& data, const void* sender) = 0;

private:
	void Update(SWeatherInfo const& data, const void* sender) final
	{
		UpdateWeather(data, sender);
	}
};

class CWeatherWindObserverHelper : public IObserver<SWeatherInfoWind>
{
public:
	virtual void UpdateWeatherWind(SWeatherInfoWind const& data, const void* sender) = 0;

private:
	void Update(SWeatherInfoWind const& data, const void* sender) final
	{
		UpdateWeatherWind(data, sender);
	}
};

class CDualWeatherObserver : public CWeatherObserverHelper, public CWeatherWindObserverHelper
{
public:
	virtual void Update(SWeatherInfo const& basic, boost::optional<SWindData> const& wind, const void* sender) = 0;

private:
	void UpdateWeather(SWeatherInfo const& data, const void* sender) final
	{
		Update(data, boost::none, sender);
	}

	void UpdateWeatherWind(SWeatherInfoWind const& data, const void* sender) final
	{
		Update(data, data.wind, sender);
	}
};

class CDisplay: public CDualWeatherObserver
{
public:
	explicit CDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
	}

private:
	void Update(SWeatherInfo const& basic, boost::optional<SWindData> const& wind, const void* sender) final
	{
		std::cout << "Update from " << m_senderNameProvider(sender) << " sensor:" << std::endl;
		std::cout << "Current Temp " << basic.temperature << std::endl;
		std::cout << "Current Hum " << basic.humidity << std::endl;
		std::cout << "Current Pressure " << basic.pressure << std::endl;
		if (wind)
		{
			std::cout << "Current Wind " << *wind << std::endl;
		}
		std::cout << "----------------" << std::endl;
	}

	SenderNameProvider m_senderNameProvider;
};

class CMinMaxStatsCalculator
{
public:
	virtual ~CMinMaxStatsCalculator() = default;

	virtual void AddValue(double value)
	{
		if (m_minValue > value)
		{
			m_minValue = value;
		}
		if (m_maxValue < value)
		{
			m_maxValue = value;
		}
	}

	double GetMinValue() const
	{
		return m_minValue;
	}

	double GetMaxValue() const
	{
		return m_maxValue;
	}

private:
	double m_minValue = std::numeric_limits<double>::infinity();
	double m_maxValue = -std::numeric_limits<double>::infinity();
};

class CScalarStatsCalculator : public CMinMaxStatsCalculator
{
public:
	void AddValue(double value) final
	{
		CMinMaxStatsCalculator::AddValue(value);
		m_accValue += value;
		++m_countAcc;
	}

	double GetAverageValue() const
	{
		return m_accValue / m_countAcc;
	}

private:
	double m_accValue = 0;
	unsigned m_countAcc = 0;
};

double sqr(double a)
{
	return a * a;
}

class CWindStatsCalculator
{
public:
	void AddValue(SWindData const& data)
	{
		m_minMaxSpeedStat.AddValue(data.speed);

		const double radAngle = data.direction * M_PI / 180;
		m_accValue.first += data.speed * cos(radAngle);
		m_accValue.second += data.speed * sin(radAngle);

		++m_countAcc;
	}

	double GetMinSpeed() const
	{
		return m_minMaxSpeedStat.GetMinValue();
	}

	double GetMaxSpeed() const
	{
		return m_minMaxSpeedStat.GetMaxValue();
	}

	SWindData GetAverageValue() const
	{
		SWindData result;
		double radAngle = atan2(m_accValue.second, m_accValue.first);
		result.direction = radAngle * 180 / M_PI;
		result.speed = sqrt(sqr(m_accValue.first) + sqr(m_accValue.second)) / m_countAcc;
		return result;
	}

private:
	CMinMaxStatsCalculator m_minMaxSpeedStat;
	std::pair<double, double> m_accValue = { 0, 0 };
	unsigned m_countAcc = 0;
};

class CStatsDisplay : public CDualWeatherObserver
{
public:
	explicit CStatsDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
	}

private:
	struct SensorStats
	{
		CScalarStatsCalculator temperatureStats;
		CScalarStatsCalculator humidityStats;
		CScalarStatsCalculator pressureStats;
		CWindStatsCalculator windStats;
	};

	void Update(SWeatherInfo const& basic, boost::optional<SWindData> const& wind, const void* sender) final
	{
		auto& sensorStats = m_stats[sender];

		sensorStats.temperatureStats.AddValue(basic.temperature);
		sensorStats.humidityStats.AddValue(basic.humidity);
		sensorStats.pressureStats.AddValue(basic.pressure);
		if (wind)
		{
			sensorStats.windStats.AddValue(*wind);
		}

		std::cout << "Update from " << m_senderNameProvider(sender) << " sensor:" << std::endl;
		DisplayScalarStats("Temp", sensorStats.temperatureStats);
		DisplayScalarStats("Hum", sensorStats.humidityStats);
		DisplayScalarStats("Pressure", sensorStats.pressureStats);
		if (wind)
		{
			DisplayWindStats(sensorStats.windStats);
		}
		std::cout << "----------------" << std::endl;
	}

	static void DisplayScalarStats(std::string const& name, CScalarStatsCalculator const& stats)
	{
		std::cout << "Max " << name << " " << stats.GetMaxValue() << std::endl;
		std::cout << "Min " << name << " " << stats.GetMinValue() << std::endl;
		std::cout << "Average " << name << " " << stats.GetAverageValue() << std::endl;
	}

	static void DisplayWindStats(CWindStatsCalculator const& stats)
	{
		std::cout << "Max Wind speed " << stats.GetMaxSpeed() << std::endl;
		std::cout << "Min Wind speed " << stats.GetMinSpeed() << std::endl;
		std::cout << "Average Wind " << stats.GetAverageValue() << std::endl;
	}

	SenderNameProvider m_senderNameProvider;
	std::map<const void*, SensorStats> m_stats;
};

template<typename InfoType>
class CAbstractWeatherData : public CObservable<InfoType>
{
public:
	// Температура в градусах Цельсия
	double GetTemperature()const
	{
		return m_temperature;
	}
	// Относительная влажность (0...100)
	double GetHumidity()const
	{
		return m_humidity;
	}
	// Атмосферное давление (в мм.рт.ст)
	double GetPressure()const
	{
		return m_pressure;
	}

	void MeasurementsChanged()
	{
		NotifyObservers();
	}

protected:
	double m_temperature = 0.0;
	double m_humidity = 0.0;	
	double m_pressure = 760.0;	
};

class CWeatherData : public CAbstractWeatherData<SWeatherInfo>
{
public:
	void SetMeasurements(double temp, double humidity, double pressure)
	{
		m_humidity = humidity;
		m_temperature = temp;
		m_pressure = pressure;

		MeasurementsChanged();
	}

private:
	SWeatherInfo GetChangedData() const final
	{
		SWeatherInfo info;
		info.temperature = GetTemperature();
		info.humidity = GetHumidity();
		info.pressure = GetPressure();
		return info;
	}
};

class CWeatherWindData : public CAbstractWeatherData<SWeatherInfoWind>
{
public:
	void SetMeasurements(double temp, double humidity, double pressure, double windSpeed, double windDirection)
	{
		m_humidity = humidity;
		m_temperature = temp;
		m_pressure = pressure;
		m_wind.speed = windSpeed;
		m_wind.direction = windDirection;

		MeasurementsChanged();
	}

	// Скорость и направление ветра
	SWindData GetWind()const
	{
		return m_wind;
	}

private:
	SWeatherInfoWind GetChangedData() const final
	{
		SWeatherInfoWind info;
		info.temperature = GetTemperature();
		info.humidity = GetHumidity();
		info.pressure = GetPressure();
		info.wind = GetWind();
		return info;
	}

	SWindData m_wind;
};
