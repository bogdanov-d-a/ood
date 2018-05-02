#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include <climits>
#include <string>
#define _USE_MATH_DEFINES
#include <math.h>
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
	SWindData wind;
};

class CDisplay: public IObserver<SWeatherInfo>
{
private:
	/* Метод Update сделан приватным, чтобы ограничить возможность его вызова напрямую
		Классу CObservable он будет доступен все равно, т.к. в интерфейсе IObserver он
		остается публичным
	*/
	void Update(SWeatherInfo const& data) override
	{
		std::cout << "Current Temp " << data.temperature << std::endl;
		std::cout << "Current Hum " << data.humidity << std::endl;
		std::cout << "Current Pressure " << data.pressure << std::endl;
		std::cout << "Current Wind " << data.wind << std::endl;
		std::cout << "----------------" << std::endl;
	}
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

class CStatsDisplay : public IObserver<SWeatherInfo>
{
private:
	/* Метод Update сделан приватным, чтобы ограничить возможность его вызова напрямую
	Классу CObservable он будет доступен все равно, т.к. в интерфейсе IObserver он
	остается публичным
	*/
	void Update(SWeatherInfo const& data) override
	{
		m_temperatureStats.AddValue(data.temperature);
		m_humidityStats.AddValue(data.humidity);
		m_pressureStats.AddValue(data.pressure);
		m_windStats.AddValue(data.wind);

		DisplayScalarStats("Temp", m_temperatureStats);
		DisplayScalarStats("Hum", m_humidityStats);
		DisplayScalarStats("Pressure", m_pressureStats);
		DisplayWindStats(m_windStats);
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

	CScalarStatsCalculator m_temperatureStats;
	CScalarStatsCalculator m_humidityStats;
	CScalarStatsCalculator m_pressureStats;
	CWindStatsCalculator m_windStats;
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
protected:
	SWeatherInfo GetChangedData()const override
	{
		SWeatherInfo info;
		info.temperature = GetTemperature();
		info.humidity = GetHumidity();
		info.pressure = GetPressure();
		info.wind = GetWind();
		return info;
	}
private:
	double m_temperature = 0.0;
	double m_humidity = 0.0;	
	double m_pressure = 760.0;	
	SWindData m_wind;
};
