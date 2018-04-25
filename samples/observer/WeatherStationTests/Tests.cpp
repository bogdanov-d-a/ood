#include "stdafx.h"
#include "..\WeatherStation\Observer.h"

namespace
{

class CMockObserver : public IObserver<bool>
{
public:
	const void* GetNotifier() const
	{
		return m_notifier;
	}

	bool IsNotified() const
	{
		return GetNotifier() != nullptr;
	}

	void ResetNotifier()
	{
		m_notifier = nullptr;
	}

	void SetDoOnNotify(std::function<void(const void*)> const& onNotify = std::function<void(const void*)>())
	{
		m_onNotify = onNotify;
	}

	void SetUnsubscribeSelf(IObservable<bool> *unsubscribeSelf = nullptr)
	{
		m_unsubscribeSelf = unsubscribeSelf;
	}

private:
	void Update(bool const& data, const void* sender) final
	{
		assert(!data);
		(void)data;

		m_notifier = sender;
		if (m_onNotify)
		{
			m_onNotify(sender);
		}

		if (m_unsubscribeSelf)
		{
			m_unsubscribeSelf->RemoveObserver(*this);
		}
	}

	const void* m_notifier = nullptr;
	std::function<void(const void*)> m_onNotify;
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

	observer1.ResetNotifier();
	observer2.ResetNotifier();
	observer3.ResetNotifier();

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

		observer1.SetDoOnNotify([&updOrder](const void*) { updOrder.push_back(1); });
		observer2.SetDoOnNotify([&updOrder](const void*) { updOrder.push_back(2); });
		observer3.SetDoOnNotify([&updOrder](const void*) { updOrder.push_back(3); });

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

BOOST_AUTO_TEST_CASE(TestSender)
{
	CMockObservable observable1;
	CMockObservable observable2;

	CMockObserver observer1;
	observable1.RegisterObserver(observer1, 0);

	CMockObserver observer2;
	observable2.RegisterObserver(observer2, 0);

	CMockObserver observer3;
	observable1.RegisterObserver(observer3, 0);
	observable2.RegisterObserver(observer3, 0);

	observable1.NotifyObservers();

	BOOST_CHECK(observer1.GetNotifier() == &observable1);
	BOOST_CHECK(!observer2.IsNotified());
	BOOST_CHECK(observer3.GetNotifier() == &observable1);

	observer1.ResetNotifier();
	observer2.ResetNotifier();
	observer3.ResetNotifier();

	observable2.NotifyObservers();

	BOOST_CHECK(!observer1.IsNotified());
	BOOST_CHECK(observer2.GetNotifier() == &observable2);
	BOOST_CHECK(observer3.GetNotifier() == &observable2);
}
