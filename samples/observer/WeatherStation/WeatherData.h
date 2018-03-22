#pragma once
#include <iostream>
#include <vector>
#include <algorithm>
#include <climits>
#include <string>
#include "Observer.h"

using namespace std;

struct SWeatherInfo
{
	double temperature = 0;
	double humidity = 0;
	double pressure = 0;
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
		std::cout << "----------------" << std::endl;
	}
};

class CAbstractStatsDisplay : public IObserver<SWeatherInfo>
{
protected:
	virtual string GetAlias() const = 0;
	virtual double GetValue(SWeatherInfo const& data) const = 0;

private:
	/* Метод Update сделан приватным, чтобы ограничить возможность его вызова напрямую
	Классу CObservable он будет доступен все равно, т.к. в интерфейсе IObserver он
	остается публичным
	*/
	void Update(SWeatherInfo const& data) override
	{
		if (m_minValue > GetValue(data))
		{
			m_minValue = GetValue(data);
		}
		if (m_maxValue < GetValue(data))
		{
			m_maxValue = GetValue(data);
		}
		m_accValue += GetValue(data);
		++m_countAcc;

		std::cout << "Max " << GetAlias() << " " << m_maxValue << std::endl;
		std::cout << "Min " << GetAlias() << " " << m_minValue << std::endl;
		std::cout << "Average " << GetAlias() << " " << (m_accValue / m_countAcc) << std::endl;
		std::cout << "----------------" << std::endl;
	}

	double m_minValue = std::numeric_limits<double>::infinity();
	double m_maxValue = -std::numeric_limits<double>::infinity();
	double m_accValue = 0;
	unsigned m_countAcc = 0;

};

class CTemperatureStatsDisplay : public CAbstractStatsDisplay
{
private:
	string GetAlias() const final
	{
		return "Temp";
	}
	double GetValue(SWeatherInfo const& data) const final
	{
		return data.temperature;
	}
};

class CHumidityStatsDisplay : public CAbstractStatsDisplay
{
private:
	string GetAlias() const final
	{
		return "Hum";
	}
	double GetValue(SWeatherInfo const& data) const final
	{
		return data.humidity;
	}
};

class CPressureStatsDisplay : public CAbstractStatsDisplay
{
private:
	string GetAlias() const final
	{
		return "Pressure";
	}
	double GetValue(SWeatherInfo const& data) const final
	{
		return data.pressure;
	}
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
