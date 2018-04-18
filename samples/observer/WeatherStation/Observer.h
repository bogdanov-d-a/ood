#pragma once

#include <set>
#include <functional>
#include <cassert>
#include <boost\scope_exit.hpp>

/*
Шаблонный интерфейс IObserver. Его должен реализовывать класс, 
желающий получать уведомления от соответствующего IObservable
Параметром шаблона является тип аргумента,
передаваемого Наблюдателю в метод Update
*/
template <typename T>
class IObserver
{
public:
	virtual void Update(T const& data) = 0;
	virtual ~IObserver() = default;
};

/*
Шаблонный интерфейс IObservable. Позволяет подписаться и отписаться на оповещения, а также
инициировать рассылку уведомлений зарегистрированным наблюдателям.
*/
template <typename T>
class IObservable
{
public:
	virtual ~IObservable() = default;
	virtual void RegisterObserver(IObserver<T> & observer, int priority) = 0;
	virtual void NotifyObservers() = 0;
	virtual void RemoveObserver(IObserver<T> & observer) = 0;
};

// Реализация интерфейса IObservable
template <class T>
class CObservable : public IObservable<T>
{
public:
	typedef IObserver<T> ObserverType;

	void RegisterObserver(ObserverType & observer, int priority) override
	{
		if (FindObserverByPointer(&observer) == m_observers.end())
		{
			m_observers.insert(ObserverData(priority, &observer));
		}
		else
		{
			assert(false);
		}
	}

	void NotifyObservers() override
	{
		if (m_observerIt != m_observers.end())
		{
			// NotifyObservers is already in progress
			assert(false);
			return;
		}

		T data = GetChangedData();

		BOOST_SCOPE_EXIT_ALL(this)
		{
			m_observerIt = m_observers.end();
		};

		for (m_observerIt = m_observers.begin(); m_observerIt != m_observers.end();)
		{
			BOOST_SCOPE_EXIT_ALL(this)
			{
				m_skipObserverIncrement = false;
			};
			assert(!m_skipObserverIncrement);

			m_observerIt->second->Update(data);

			if (!m_skipObserverIncrement)
			{
				++m_observerIt;
			}
		}
	}

	void RemoveObserver(ObserverType & observer) override
	{
		const auto eraseIt = FindObserverByPointer(&observer);
		if (eraseIt == m_observers.end())
		{
			// Observer not found
			assert(false);
			return;
		}

		const bool updatingElementErase = (m_observerIt == eraseIt);
		const auto afterEraseIt = m_observers.erase(eraseIt);

		if (updatingElementErase)
		{
			m_observerIt = afterEraseIt;
			m_skipObserverIncrement = true;
		}
	}

protected:
	// Классы-наследники должны перегрузить данный метод, 
	// в котором возвращать информацию об изменениях в объекте
	virtual T GetChangedData()const = 0;

private:
	using ObserverData = std::pair<int, ObserverType *>;

	auto FindObserverByPointer(ObserverType *observerPtr)
	{
		for (auto it = m_observers.begin(); it != m_observers.end(); ++it)
		{
			if (it->second == observerPtr)
			{
				return it;
			}
		}
		return m_observers.end();
	}

	std::set<ObserverData> m_observers;
	typename std::set<ObserverData>::iterator m_observerIt = m_observers.end();
	bool m_skipObserverIncrement = false;
};
