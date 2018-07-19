#pragma once

namespace naive
{
const unsigned MAX_QUARTER_COUNT = 5;

class CGumballMachine;

void InsertQuarterImpl(CGumballMachine &gumballMachine);
void EjectQuartersImpl(CGumballMachine &gumballMachine);

class CGumballMachine
{
public:
	enum class State
	{
		SoldOut,		// Жвачка закончилась
		Basic,			// Готов к работе
		Sold,			// Монетка выдана
	};

	using DisplayCallback = std::function<void(std::string const&)>;

	CGumballMachine(unsigned count, DisplayCallback const& displayCallback)
		: m_gumballCount(count)
		, m_state(count > 0 ? State::Basic : State::SoldOut)
		, m_displayCallback(displayCallback)
	{
	}

	void InsertQuarter()
	{
		using namespace std;
		switch (m_state)
		{
		case State::Basic:
		case State::Sold:
			InsertQuarterImpl(*this);
			break;
		case State::SoldOut:
			DisplayMessage("You can't insert a quarter, the machine is sold out");
			break;
		}
	}

	void EjectQuarters()
	{
		EjectQuartersImpl(*this);
	}

	void TurnCrank()
	{
		using namespace std;
		switch (m_state)
		{
		case State::SoldOut:
			DisplayMessage("You turned but there's no gumballs");
			break;
		case State::Basic:
			if (m_quarterCount > 0)
			{
				DisplayMessage("You turned...");
				m_state = State::Sold;
			}
			else
			{
				DisplayMessage("You turned but there's no quarter");
			}
			break;
		case State::Sold:
			DisplayMessage("Turning twice doesn't get you another gumball");
			break;
		}
		Dispense();
	}

	void Refill(unsigned numBalls)
	{
		switch (m_state)
		{
		case State::Basic:
		case State::SoldOut:
			m_gumballCount = numBalls;
			m_state = numBalls > 0 ? State::Basic : State::SoldOut;
			DisplayMessage("Machine refilled to " + std::to_string(numBalls) + " gumballs");
			break;
		case State::Sold:
			DisplayMessage("Can't refill machine while dispensing the gumball");
			break;
		}
	}

	unsigned GetQuarterCount()const
	{
		return m_quarterCount;
	}

	void SetQuarterCount(unsigned count)
	{
		m_quarterCount = count;
	}

	void DisplayMessage(std::string const& message) const
	{
		m_displayCallback(message);
	}

	std::string ToString()const
	{
		std::string state =
			(m_state == State::SoldOut)    ? "sold out" :
			(m_state == State::Basic)  ? "ready"
			                               : "delivering a gumball";
		auto fmt = boost::format(R"(
Mighty Gumball, Inc.
C++-enabled Standing Gumball Model #2016
Inventory: %1% gumball%2%, %3% quarter%4%
Machine is %5%
)");
		return (fmt
			% m_gumballCount % (m_gumballCount != 1 ? "s" : "")
			% m_quarterCount % (m_quarterCount != 1 ? "s" : "")
			% state).str();
	}

private:
	void Dispense()
	{
		using namespace std;
		switch (m_state)
		{
		case State::Sold:
			DisplayMessage("A gumball comes rolling out the slot...");
			--m_gumballCount;

			assert(m_quarterCount > 0);
			--m_quarterCount;

			if (m_gumballCount == 0)
			{
				DisplayMessage("Oops, out of gumballs");
				m_state = State::SoldOut;
			}
			else
			{
				m_state = State::Basic;
			}
			break;
		case State::SoldOut:
		case State::Basic:
			DisplayMessage("No gumball dispensed");
			break;
		}
	}

	unsigned m_gumballCount = 0;	// Количество шариков
	unsigned m_quarterCount = 0;
	State m_state = State::SoldOut;
	DisplayCallback m_displayCallback;
};

void InsertQuarterImpl(CGumballMachine &gumballMachine)
{
	const auto qc = gumballMachine.GetQuarterCount();
	if (qc < MAX_QUARTER_COUNT)
	{
		gumballMachine.DisplayMessage("You inserted a quarter");
		gumballMachine.SetQuarterCount(qc + 1);
	}
	else
	{
		gumballMachine.DisplayMessage("You can't insert another quarter");
	}
}

void EjectQuartersImpl(CGumballMachine &gumballMachine)
{
	const auto qc = gumballMachine.GetQuarterCount();
	if (qc > 0)
	{
		gumballMachine.DisplayMessage(std::to_string(qc) + " quarter"
			+ (qc > 1 ? "s" : "")
			+ " returned");
	}
	else
	{
		gumballMachine.DisplayMessage("You can't eject, you haven't inserted a quarter yet");
	}
	gumballMachine.SetQuarterCount(0);
}
}