#include "WeatherData.h"

int main()
{
	CWeatherData wdIndoor;
	CWeatherData wdOutdoor;

	const auto senderNameProvider = [&](const void* sender) {
		decltype(auto) wdSender = reinterpret_cast<const CWeatherData*>(sender);
		if (wdSender == &wdIndoor)
		{
			return "indoor";
		}
		else if (wdSender == &wdOutdoor)
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
	wdIndoor.SetMeasurements(15, 0.5, 762, 0, 0);
	wdOutdoor.SetMeasurements(4, 0.8, 761, 3, 10);
	wdIndoor.SetMeasurements(16, 0.6, 763, 0, 0);

	wdIndoor.RemoveObserver(statsDisplay);
	wdOutdoor.RemoveObserver(display);

	wdOutdoor.SetMeasurements(10, 0.8, 761, 5, 100);
	wdIndoor.SetMeasurements(20, 0.6, 763, 0, 0);
	wdOutdoor.SetMeasurements(-10, 0.8, 761, 7, 145);
	wdIndoor.SetMeasurements(22, 0.6, 763, 0, 0);

	return 0;
}