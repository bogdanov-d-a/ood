#include "WeatherData.h"

namespace
{

class SenderNameProviderVisitor : public boost::static_visitor<std::string>
{
public:
	std::string operator()(IObservable<SWeatherInfo>&) const
    {
		return "indoor";
	}
    
	std::string operator()(IObservable<SWeatherInfoWind>&) const
    {
		return "outdoor";
	}
};

}

int main()
{
	CWeatherData wdIndoor;
	CWeatherWindData wdOutdoor;

	const auto senderNameProvider = [](boost::variant<IObservable<SWeatherInfo>&, IObservable<SWeatherInfoWind>&> sender) {
		return sender.apply_visitor(SenderNameProviderVisitor());
	};

	CDisplay display(senderNameProvider);
	wdIndoor.RegisterObserver(display.GetBasicObserver(), 0);
	wdOutdoor.RegisterObserver(display.GetWindObserver(), 0);

	CStatsDisplay statsDisplay(senderNameProvider);
	wdIndoor.RegisterObserver(statsDisplay.GetBasicObserver(), 0);
	wdOutdoor.RegisterObserver(statsDisplay.GetWindObserver(), 0);

	wdOutdoor.SetMeasurements(3, 0.7, 760, 2, 0);
	wdIndoor.SetMeasurements(15, 0.5, 762);
	wdOutdoor.SetMeasurements(4, 0.8, 761, 3, 10);
	wdIndoor.SetMeasurements(16, 0.6, 763);

	wdIndoor.RemoveObserver(statsDisplay.GetBasicObserver());
	wdOutdoor.RemoveObserver(display.GetWindObserver());

	wdOutdoor.SetMeasurements(10, 0.8, 761, 5, 100);
	wdIndoor.SetMeasurements(20, 0.6, 763);
	wdOutdoor.SetMeasurements(-10, 0.8, 761, 7, 145);
	wdIndoor.SetMeasurements(22, 0.6, 763);

	return 0;
}