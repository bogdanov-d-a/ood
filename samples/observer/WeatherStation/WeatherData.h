#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include <climits>
#include <string>
#include <functional>
#include <map>
#include "Observer.h"

using namespace std;

struct SWeatherInfo
{
	double temperature = 0;
	double humidity = 0;
	double pressure = 0;
};

using SenderNameProvider = std::function<std::string(IObservable<SWeatherInfo>&)>;

class CDisplay: public IObserver<SWeatherInfo>
{
public:
	explicit CDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
	}

private:
	/* Метод Update сделан приватным, чтобы ограничить возможность его вызова напрямую
		Классу CObservable он будет доступен все равно, т.к. в интерфейсе IObserver он
		остается публичным
	*/
	void Update(SWeatherInfo const& data, IObservable<SWeatherInfo> &sender) override
	{
		std::cout << "Update from " << m_senderNameProvider(sender) << " sensor:" << std::endl;
		std::cout << "Current Temp " << data.temperature << std::endl;
		std::cout << "Current Hum " << data.humidity << std::endl;
		std::cout << "Current Pressure " << data.pressure << std::endl;
		std::cout << "----------------" << std::endl;
	}

	SenderNameProvider m_senderNameProvider;
};

class CStatsCalculator
{
public:
	void AddValue(double value)
	{
		if (m_minValue > value)
		{
			m_minValue = value;
		}
		if (m_maxValue < value)
		{
			m_maxValue = value;
		}
		m_accValue += value;
		++m_countAcc;
	}

	double GetMinValue() const
	{
		return m_minValue;
	}

	double GetMaxValue() const
	{
		return m_maxValue;
	}

	double GetAverageValue() const
	{
		return m_accValue / m_countAcc;
	}

private:
	double m_minValue = std::numeric_limits<double>::infinity();
	double m_maxValue = -std::numeric_limits<double>::infinity();
	double m_accValue = 0;
	unsigned m_countAcc = 0;
};

class CStatsDisplay : public IObserver<SWeatherInfo>
{
public:
	explicit CStatsDisplay(SenderNameProvider const& senderNameProvider)
		: m_senderNameProvider(senderNameProvider)
	{
	}

private:
	struct SensorStats
	{
		CStatsCalculator temperatureStats;
		CStatsCalculator humidityStats;
		CStatsCalculator pressureStats;
	};

	/* Метод Update сделан приватным, чтобы ограничить возможность его вызова напрямую
	Классу CObservable он будет доступен все равно, т.к. в интерфейсе IObserver он
	остается публичным
	*/
	void Update(SWeatherInfo const& data, IObservable<SWeatherInfo> &sender) override
	{
		auto& sensorStats = m_stats[&sender];

		sensorStats.temperatureStats.AddValue(data.temperature);
		sensorStats.humidityStats.AddValue(data.humidity);
		sensorStats.pressureStats.AddValue(data.pressure);

		std::cout << "Update from " << m_senderNameProvider(sender) << " sensor:" << std::endl;
		DisplayStats("Temp", sensorStats.temperatureStats);
		DisplayStats("Hum", sensorStats.humidityStats);
		DisplayStats("Pressure", sensorStats.pressureStats);
		std::cout << "----------------" << std::endl;
	}

	void DisplayStats(std::string const& name, CStatsCalculator const& stats)
	{
		std::cout << "Max " << name << " " << stats.GetMaxValue() << std::endl;
		std::cout << "Min " << name << " " << stats.GetMinValue() << std::endl;
		std::cout << "Average " << name << " " << stats.GetAverageValue() << std::endl;
	}

	SenderNameProvider m_senderNameProvider;
	std::map<const IObservable<SWeatherInfo>*, SensorStats> m_stats;
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
protected:
	SWeatherInfo GetChangedData()const override
	{
		SWeatherInfo info;
		info.temperature = GetTemperature();
		info.humidity = GetHumidity();
		info.pressure = GetPressure();
		return info;
	}
private:
	double m_temperature = 0.0;
	double m_humidity = 0.0;	
	double m_pressure = 760.0;	
};
