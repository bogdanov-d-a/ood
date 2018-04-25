#include "WeatherData.h"

int main()
{
	CWeatherData wdIndoor;
	CWeatherWindData wdOutdoor;

	const auto senderNameProvider = [&](const void* sender) {
		if (sender == reinterpret_cast<const void*>(&wdIndoor))
		{
			return "indoor";
		}
		else if (sender == reinterpret_cast<const void*>(&wdOutdoor))
		{
			return "outdoor";
		}
		else
		{
			return "unknown";
		}
	};

	CDisplay display(senderNameProvider);
	wdIndoor.RegisterObserver(display, 0);
	wdOutdoor.RegisterObserver(display, 0);

	CStatsDisplay statsDisplay(senderNameProvider);
	wdIndoor.RegisterObserver(statsDisplay, 0);
	wdOutdoor.RegisterObserver(statsDisplay, 0);

	wdOutdoor.SetMeasurements(3, 0.7, 760, 2, 0);
	wdIndoor.SetMeasurements(15, 0.5, 762);
	wdOutdoor.SetMeasurements(4, 0.8, 761, 3, 10);
	wdIndoor.SetMeasurements(16, 0.6, 763);

	wdIndoor.RemoveObserver(statsDisplay);
	wdOutdoor.RemoveObserver(display);

	wdOutdoor.SetMeasurements(10, 0.8, 761, 5, 100);
	wdIndoor.SetMeasurements(20, 0.6, 763);
	wdOutdoor.SetMeasurements(-10, 0.8, 761, 7, 145);
	wdIndoor.SetMeasurements(22, 0.6, 763);

	return 0;
}