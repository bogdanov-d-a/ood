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
