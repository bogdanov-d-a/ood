#include "stdafx.h"
#include "..\gumball_machine\NaiveGumBallMachine.h"
#include "..\gumball_machine\GumBallMachineWithState.h"

namespace
{

class TestMachinesWrapper
{
public:
	TestMachinesWrapper(unsigned numBalls)
		: m_naive(numBalls, [this](std::string const& message) { OnNaiveMessage(message); })
		, m_withState(numBalls, [this](std::string const& message) { OnWithStateMessage(message); })
	{
	}

	std::vector<std::string> const& GetLog() const
	{
		return m_log;
	}

	bool TryInsertQuarter()
	{
		return TryPerformOperation([this](){
			m_naive.InsertQuarter();
			m_withState.InsertQuarter();
		});
	}

	bool TryEjectQuarters()
	{
		return TryPerformOperation([this]() {
			m_naive.EjectQuarters();
			m_withState.EjectQuarters();
		});
	}

	bool TryTurnCrank()
	{
		return TryPerformOperation([this]() {
			m_naive.TurnCrank();
			m_withState.TurnCrank();
		});
	}

	bool TryRefill(unsigned numBalls)
	{
		return TryPerformOperation([this, numBalls]() {
			m_naive.Refill(numBalls);
			m_withState.Refill(numBalls);
		});
	}

private:
	void OnNaiveMessage(std::string const& message)
	{
		m_naiveMsgs.push_back(message);
	}

	void OnWithStateMessage(std::string const& message)
	{
		m_withStateMsgs.push_back(message);
	}

	bool TryPerformOperation(std::function<void()> const& callback)
	{
		m_naiveMsgs.clear();
		m_withStateMsgs.clear();

		callback();

		if (m_naiveMsgs != m_withStateMsgs)
		{
			return false;
		}

		m_log.insert(m_log.end(), m_naiveMsgs.begin(), m_naiveMsgs.end());
		return true;
	}

	naive::CGumballMachine m_naive;
	with_state::CGumballMachine m_withState;

	std::vector<std::string> m_log;

	std::vector<std::string> m_naiveMsgs;
	std::vector<std::string> m_withStateMsgs;
};

}

BOOST_AUTO_TEST_CASE(CanConstructSoldOut)
{
	TestMachinesWrapper m(0);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You can't insert a quarter, the machine is sold out",
	}));
}

BOOST_AUTO_TEST_CASE(CanConstructNormal)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"1 quarter returned",
	}));
}

BOOST_AUTO_TEST_CASE(TestSoldOut)
{
	TestMachinesWrapper m(0);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You can't insert a quarter, the machine is sold out",
		"You can't eject, you haven't inserted a quarter yet",
		"You turned but there's no gumballs",
		"No gumball dispensed",
	}));
}

BOOST_AUTO_TEST_CASE(CantEjectQuarterIfNotInserted)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You can't eject, you haven't inserted a quarter yet",
	}));
}

BOOST_AUTO_TEST_CASE(CantGetGumballForFree)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You turned but there's no quarter",
		"No gumball dispensed",
	}));
}

BOOST_AUTO_TEST_CASE(CanBuyWithFourQuarters)
{
	TestMachinesWrapper m(2);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You inserted a quarter",
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"Oops, out of gumballs",
		"2 quarters returned",
	}));
}

BOOST_AUTO_TEST_CASE(CanBuyGumball)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
	}));
}

BOOST_AUTO_TEST_CASE(CanBuyLastGumball)
{
	TestMachinesWrapper m(1);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"Oops, out of gumballs",
		"You can't insert a quarter, the machine is sold out",
	}));
}

BOOST_AUTO_TEST_CASE(CanBuyGumballTwice)
{
	TestMachinesWrapper m(2);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"Oops, out of gumballs",
		"You can't insert a quarter, the machine is sold out",
	}));
}

BOOST_AUTO_TEST_CASE(CantEjectQuarterAfterBuy)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"You can't eject, you haven't inserted a quarter yet",
	}));
}

BOOST_AUTO_TEST_CASE(CantDispenseExtraGumball)
{
	TestMachinesWrapper m(42);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"You turned but there's no quarter",
		"No gumball dispensed",
	}));
}

BOOST_AUTO_TEST_CASE(CanRefillMachine)
{
	TestMachinesWrapper m(1);
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryInsertQuarter());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryRefill(2));
	BOOST_CHECK(m.TryTurnCrank());
	BOOST_CHECK(m.TryEjectQuarters());
	BOOST_CHECK(m.GetLog() == std::vector<std::string>({
		"You inserted a quarter",
		"You inserted a quarter",
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"Oops, out of gumballs",
		"You turned but there's no gumballs",
		"No gumball dispensed",
		"Machine refilled to 2 gumballs",
		"You turned...",
		"A gumball comes rolling out the slot...",
		"1 quarter returned",
	}));
}
