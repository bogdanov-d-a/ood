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

		if (m_unsubscribeSelf)
		{
			m_unsubscribeSelf->RemoveObserver(*this);
		}
	}

	bool m_notified = false;
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
	observable.RegisterObserver(observer1);

	CMockObserver observer2;
	observable.RegisterObserver(observer2);

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
	observable.RegisterObserver(observer1);

	CMockObserver observer2;
	observer2.SetUnsubscribeSelf(&observable);
	observable.RegisterObserver(observer2);

	CMockObserver observer3;
	observable.RegisterObserver(observer3);

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
