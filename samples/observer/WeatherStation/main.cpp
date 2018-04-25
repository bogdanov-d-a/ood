#include "WeatherData.h"

int main()
{
	CWeatherData wd;

	CDisplay display;
	wd.RegisterObserver(display, 0);

	CStatsDisplay statsDisplay;
	wd.RegisterObserver(statsDisplay, 0);

	wd.SetMeasurements(3, 0.7, 760, 2, 0);
	wd.SetMeasurements(4, 0.8, 761, 3, 10);

	wd.RemoveObserver(statsDisplay);

	wd.SetMeasurements(10, 0.8, 761, 5, 100);
	wd.SetMeasurements(-10, 0.8, 761, 7, 145);
	return 0;
}