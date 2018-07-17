#include "stdafx.h"
#include "..\gumball_machine\GumBallMachineWithState.h"

BOOST_AUTO_TEST_CASE(CanConstructSoldOut)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(0, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.InsertQuarter();
	BOOST_CHECK(log == std::vector<std::string>({
		"You can't insert a quarter, the machine is sold out",
	}));
}

BOOST_AUTO_TEST_CASE(CanConstructNormal)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(42, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.InsertQuarter();
	m.EjectQuarter();
	BOOST_CHECK(log == std::vector<std::string>({
		"You inserted a quarter",
		"Quarter returned",
	}));
}

BOOST_AUTO_TEST_CASE(TestSoldOut)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(0, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.InsertQuarter();
	m.EjectQuarter();
	m.TurnCrank();
	BOOST_CHECK(log == std::vector<std::string>({
		"You can't insert a quarter, the machine is sold out",
		"You can't eject, you haven't inserted a quarter yet",
		"You turned but there's no gumballs",
		"No gumball dispensed",
	}));
}

BOOST_AUTO_TEST_CASE(CantEjectQuarterIfNotInserted)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(42, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.EjectQuarter();
	BOOST_CHECK(log == std::vector<std::string>({
		"You haven't inserted a quarter",
	}));
}

BOOST_AUTO_TEST_CASE(CantGetGumballForFree)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(42, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.TurnCrank();
	BOOST_CHECK(log == std::vector<std::string>({
		"You turned but there's no quarter",
		"You need to pay first",
	}));
}

BOOST_AUTO_TEST_CASE(CantInsertTwoQuarters)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(42, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.InsertQuarter();
	m.InsertQuarter();
	m.InsertQuarter();
	BOOST_CHECK(log == std::vector<std::string>({
		"You inserted a quarter",
		"You can't insert another quarter",
		"You can't insert another quarter",
	}));
}

BOOST_AUTO_TEST_CASE(CanBuyGumball)
{
	std::vector<std::string> log;
	with_state::CGumballMachine m(42, [&log](std::string const& message) {
		log.push_back(message);
	});
	m.InsertQuarter();
	m.TurnCrank();
	BOOST_CHECK(log == std::vector<std::string>({
		"You inserted a quarter",
		"You turned...",
		"A gumball comes rolling out the slot...",
	}));
}
