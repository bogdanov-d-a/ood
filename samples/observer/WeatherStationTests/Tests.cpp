#include "stdafx.h"
#include "..\WeatherStation\Observer.h"

namespace
{

class CMockObserver : public IObserver<bool>
{
public:
	bool IsNotified() const
	{
		return m_notified;
	}

	void ResetNotified()
	{
		m_notified = false;
	}

	void SetDoOnNotify(std::function<void()> const& onNotify = std::function<void()>())
	{
		m_onNotify = onNotify;
	}

	void SetUnsubscribeSelf(IObservable<bool> *unsubscribeSelf = nullptr)
	{
		m_unsubscribeSelf = unsubscribeSelf;
	}

private:
	void Update(bool const& data) final
	{
		assert(!data);
		(void)data;

		m_notified = true;
		if (m_onNotify)
		{
			m_onNotify();
		}

		if (m_unsubscribeSelf)
		{
			m_unsubscribeSelf->RemoveObserver(*this);
		}
	}

	bool m_notified = false;
	std::function<void()> m_onNotify;
	IObservable<bool> *m_unsubscribeSelf = nullptr;
};

class CMockObservable : public CObservable<bool>
{
private:
	bool GetChangedData() const final
	{
		return false;
	}
};

}

BOOST_AUTO_TEST_CASE(TestBasic)
{
	CMockObservable observable;

	CMockObserver observer1;
	observable.RegisterObserver(observer1, 0);

	CMockObserver observer2;
	observable.RegisterObserver(observer2, 0);

	CMockObserver observer3;

	observable.NotifyObservers();

	BOOST_CHECK(observer1.IsNotified());
	BOOST_CHECK(observer2.IsNotified());
	BOOST_CHECK(!observer3.IsNotified());
}

BOOST_AUTO_TEST_CASE(TestObserverSelfUnsubscribe)
{
	CMockObservable observable;

	CMockObserver observer1;
	observable.RegisterObserver(observer1, 0);

	CMockObserver observer2;
	observer2.SetUnsubscribeSelf(&observable);
	observable.RegisterObserver(observer2, 0);

	CMockObserver observer3;
	observable.RegisterObserver(observer3, 0);

	observable.NotifyObservers();

	BOOST_CHECK(observer1.IsNotified());
	BOOST_CHECK(observer2.IsNotified());
	BOOST_CHECK(observer3.IsNotified());

	observer1.ResetNotified();
	observer2.ResetNotified();
	observer3.ResetNotified();

	observable.NotifyObservers();

	BOOST_CHECK(observer1.IsNotified());
	BOOST_CHECK(!observer2.IsNotified());
	BOOST_CHECK(observer3.IsNotified());
}

BOOST_AUTO_TEST_CASE(TestObserverPriority)
{
	CMockObservable observable;

	CMockObserver observer1;
	CMockObserver observer2;
	CMockObserver observer3;

	const auto runTest = [&](std::vector<int> const& priorities, std::vector<int> const& result) {
		observable.RegisterObserver(observer1, priorities[0]);
		observable.RegisterObserver(observer2, priorities[1]);
		observable.RegisterObserver(observer3, priorities[2]);

		std::vector<int> updOrder;

		observer1.SetDoOnNotify([&updOrder]() { updOrder.push_back(1); });
		observer2.SetDoOnNotify([&updOrder]() { updOrder.push_back(2); });
		observer3.SetDoOnNotify([&updOrder]() { updOrder.push_back(3); });

		observable.NotifyObservers();
		BOOST_CHECK(updOrder == result);

		observable.RemoveObserver(observer1);
		observable.RemoveObserver(observer2);
		observable.RemoveObserver(observer3);
	};

	runTest({ 0, 42, -1337 }, { 3, 1, 2 });
	runTest({ 0, 1, 2 }, { 1, 2, 3 });
	runTest({ 2, 1, 0 }, { 3, 2, 1 });
}
