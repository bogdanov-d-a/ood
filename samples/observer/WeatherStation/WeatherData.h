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
#include <boost/variant.hpp>
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

struct SWeatherInfoWind
{
	SWeatherInfo info;
	SWindData wind;
};

template<typename T>
class CCallbackObserver : public IObserver<T>
{
public:
	using FnType = std::function<void(T const& data, IObservable<T> &sender)>;

	void SetCallback(FnType const& function = FnType())
	{
		m_function = function;
	}

private:
	void Update(T const& data, IObservable<T> &sender) final
	{
		if (m_function)
		{
			m_function(data, sender);
		}
	}

	FnType m_function;
};

class CDualWeatherObserver
{
public:
	using FnType = std::function<void(SWeatherInfo const& basic, boost::optional<SWindData> const& wind,
		boost::variant<IObservable<SWeatherInfo>&, IObservable<SWeatherInfoWind>&> sender)>;

	CDualWeatherObserver()
	{
		m_basicObserver.SetCallback([this](SWeatherInfo const& data, IObservable<SWeatherInfo> &sender) {
			if (m_function)
			{
				m_function(data, boost::none, sender);
			}
		});

		m_windObserver.SetCallback([this](SWeatherInfoWind const& data, IObservable<SWeatherInfoWind> &sender) {
			if (m_function)
			{
				m_function(data.info, data.wind, sender);
			}
		});
	}

	void SetCallback(FnType const& function = FnType())
	{
		m_function = function;
	}

	IObserver<SWeatherInfo>& GetBasicObserver()
	{
		return m_basicObserver;
	}

	IObserver<SWeatherInfoWind>& GetWindObserver()
	{
		return m_windObserver;
	}

private:
	CCallbackObserver<SWeatherInfo> m_basicObserver;
	CCallbackObserver<SWeatherInfoWind> m_windObserver;
	FnType m_function;
};

using SenderNameProvider = std::function<std::string(boost::variant<IObservable<SWeatherInfo>&, IObservable<SWeatherInfoWind>&>)>;

class CDisplay
{
public:
	explicit CDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
		m_observer.SetCallback([this](SWeatherInfo const& basic, boost::optional<SWindData> const& wind,
				boost::variant<IObservable<SWeatherInfo>&, IObservable<SWeatherInfoWind>&> sender) {
			std::cout << "Update from " << m_senderNameProvider(sender) << " sensor:" << std::endl;
			std::cout << "Current Temp " << basic.temperature << std::endl;
			std::cout << "Current Hum " << basic.humidity << std::endl;
			std::cout << "Current Pressure " << basic.pressure << std::endl;
			if (wind)
			{
				std::cout << "Current Wind " << *wind << std::endl;
			}
			std::cout << "----------------" << std::endl;
		});
	}

	IObserver<SWeatherInfo>& GetBasicObserver()
	{
		return m_observer.GetBasicObserver();
	}

	IObserver<SWeatherInfoWind>& GetWindObserver()
	{
		return m_observer.GetWindObserver();
	}

private:
	SenderNameProvider m_senderNameProvider;
	CDualWeatherObserver m_observer;
};

class CMinMaxStatsCalculator
{
public:
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

class CScalarStatsCalculator
{
public:
	void AddValue(double value)
	{
		m_minMaxStat.AddValue(value);
		m_accValue += value;
		++m_countAcc;
	}

	double GetMinValue() const
	{
		return m_minMaxStat.GetMinValue();
	}

	double GetMaxValue() const
	{
		return m_minMaxStat.GetMaxValue();
	}

	double GetAverageValue() const
	{
		return m_accValue / m_countAcc;
	}

private:
	CMinMaxStatsCalculator m_minMaxStat;
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
		m_accValue.x += data.speed * cos(radAngle);
		m_accValue.y += data.speed * sin(radAngle);

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
		double radAngle = atan2(m_accValue.y, m_accValue.x);
		result.direction = radAngle * 180 / M_PI;
		result.speed = sqrt(sqr(m_accValue.x) + sqr(m_accValue.y)) / m_countAcc;
		return result;
	}

private:
	struct CartesianVector
	{
		double x = 0;
		double y = 0;
	};

	CMinMaxStatsCalculator m_minMaxSpeedStat;
	CartesianVector m_accValue;
	unsigned m_countAcc = 0;
};

class CStatsDisplay
{
public:
	explicit CStatsDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
		m_observer.SetCallback([this](SWeatherInfo const& basic, boost::optional<SWindData> const& wind,
				boost::variant<IObservable<SWeatherInfo>&, IObservable<SWeatherInfoWind>&> sender) {
			auto& sensorStats = m_stats[&sender];

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
		});
	}

	IObserver<SWeatherInfo>& GetBasicObserver()
	{
		return m_observer.GetBasicObserver();
	}

	IObserver<SWeatherInfoWind>& GetWindObserver()
	{
		return m_observer.GetWindObserver();
	}

private:
	struct SensorStats
	{
		CScalarStatsCalculator temperatureStats;
		CScalarStatsCalculator humidityStats;
		CScalarStatsCalculator pressureStats;
		CWindStatsCalculator windStats;
	};

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
	CDualWeatherObserver m_observer;
	std::map<const void*, SensorStats> m_stats;
};

class CWeatherData : public CObservable<SWeatherInfo>
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

	double m_temperature = 0.0;
	double m_humidity = 0.0;
	double m_pressure = 760.0;
};

class CWeatherWindData : public CObservable<SWeatherInfoWind>
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
	// Скорость и направление ветра
	SWindData GetWind()const
	{
		return m_wind;
	}

	void MeasurementsChanged()
	{
		NotifyObservers();
	}

	void SetMeasurements(double temp, double humidity, double pressure, double windSpeed, double windDirection)
	{
		m_humidity = humidity;
		m_temperature = temp;
		m_pressure = pressure;
		m_wind.speed = windSpeed;
		m_wind.direction = windDirection;

		MeasurementsChanged();
	}

private:
	SWeatherInfoWind GetChangedData() const final
	{
		SWeatherInfoWind info;
		info.info.temperature = GetTemperature();
		info.info.humidity = GetHumidity();
		info.info.pressure = GetPressure();
		info.wind = GetWind();
		return info;
	}

	double m_temperature = 0.0;
	double m_humidity = 0.0;
	double m_pressure = 760.0;
	SWindData m_wind;
};
