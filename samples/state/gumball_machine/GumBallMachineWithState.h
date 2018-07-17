#pragma once

namespace with_state
{

struct IState
{
	virtual void InsertQuarter() = 0;
	virtual void EjectQuarter() = 0;
	virtual void TurnCrank() = 0;
	virtual void Dispense() = 0;
	virtual std::string ToString()const = 0;
	virtual ~IState() = default;
};

struct IGumballMachine
{
	virtual void ReleaseBall() = 0;
	virtual unsigned GetBallCount()const = 0;
	virtual void DisplayMessage(std::string const& message)const = 0;

	virtual void SetSoldOutState() = 0;
	virtual void SetNoQuarterState() = 0;
	virtual void SetSoldState() = 0;
	virtual void SetHasQuarterState() = 0;

	virtual ~IGumballMachine() = default;
};

class CSoldState : public IState
{
public:
	CSoldState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}
	void InsertQuarter() override
	{
		m_gumballMachine.DisplayMessage("Please wait, we're already giving you a gumball");
	}
	void EjectQuarter() override
	{
		m_gumballMachine.DisplayMessage("Sorry you already turned the crank");
	}
	void TurnCrank() override
	{
		m_gumballMachine.DisplayMessage("Turning twice doesn't get you another gumball");
	}
	void Dispense() override
	{
		m_gumballMachine.ReleaseBall();
		if (m_gumballMachine.GetBallCount() == 0)
		{
			m_gumballMachine.DisplayMessage("Oops, out of gumballs");
			m_gumballMachine.SetSoldOutState();
		}
		else
		{
			m_gumballMachine.SetNoQuarterState();
		}
	}
	std::string ToString() const override
	{
		return "delivering a gumball";
	}
private:
	IGumballMachine & m_gumballMachine;
};

class CSoldOutState : public IState
{
public:
	CSoldOutState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}

	void InsertQuarter() override
	{
		m_gumballMachine.DisplayMessage("You can't insert a quarter, the machine is sold out");
	}
	void EjectQuarter() override
	{
		m_gumballMachine.DisplayMessage("You can't eject, you haven't inserted a quarter yet");
	}
	void TurnCrank() override
	{
		m_gumballMachine.DisplayMessage("You turned but there's no gumballs");
	}
	void Dispense() override
	{
		m_gumballMachine.DisplayMessage("No gumball dispensed");
	}
	std::string ToString() const override
	{
		return "sold out";
	}
private:
	IGumballMachine & m_gumballMachine;
};

class CHasQuarterState : public IState
{
public:
	CHasQuarterState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}

	void InsertQuarter() override
	{
		m_gumballMachine.DisplayMessage("You can't insert another quarter");
	}
	void EjectQuarter() override
	{
		m_gumballMachine.DisplayMessage("Quarter returned");
		m_gumballMachine.SetNoQuarterState();
	}
	void TurnCrank() override
	{
		m_gumballMachine.DisplayMessage("You turned...");
		m_gumballMachine.SetSoldState();
	}
	void Dispense() override
	{
		m_gumballMachine.DisplayMessage("No gumball dispensed");
	}
	std::string ToString() const override
	{
		return "waiting for turn of crank";
	}
private:
	IGumballMachine & m_gumballMachine;
};

class CNoQuarterState : public IState
{
public:
	CNoQuarterState(IGumballMachine & gumballMachine)
		: m_gumballMachine(gumballMachine)
	{}

	void InsertQuarter() override
	{
		m_gumballMachine.DisplayMessage("You inserted a quarter");
		m_gumballMachine.SetHasQuarterState();
	}
	void EjectQuarter() override
	{
		m_gumballMachine.DisplayMessage("You haven't inserted a quarter");
	}
	void TurnCrank() override
	{
		m_gumballMachine.DisplayMessage("You turned but there's no quarter");
	}
	void Dispense() override
	{
		m_gumballMachine.DisplayMessage("You need to pay first");
	}
	std::string ToString() const override
	{
		return "waiting for quarter";
	}
private:
	IGumballMachine & m_gumballMachine;
};

class CGumballMachine : private IGumballMachine
{
public:
	using DisplayCallback = std::function<void(std::string const&)>;

	CGumballMachine(unsigned numBalls, DisplayCallback const& displayCallback)
		: m_soldState(*this)
		, m_soldOutState(*this)
		, m_noQuarterState(*this)
		, m_hasQuarterState(*this)
		, m_state(&m_soldOutState)
		, m_count(numBalls)
		, m_displayCallback(displayCallback)
	{
		if (m_count > 0)
		{
			m_state = &m_noQuarterState;
		}
	}
	void EjectQuarter()
	{
		m_state->EjectQuarter();
	}
	void InsertQuarter()
	{
		m_state->InsertQuarter();
	}
	void TurnCrank()
	{
		m_state->TurnCrank();
		m_state->Dispense();
	}
	std::string ToString()const
	{
		auto fmt = boost::format(R"(
Mighty Gumball, Inc.
C++-enabled Standing Gumball Model #2016 (with state)
Inventory: %1% gumball%2%
Machine is %3%
)");
		return (fmt % m_count % (m_count != 1 ? "s" : "") % m_state->ToString()).str();
	}
private:
	unsigned GetBallCount() const override
	{
		return m_count;
	}
	virtual void ReleaseBall() override
	{
		if (m_count != 0)
		{
			std::cout << "A gumball comes rolling out the slot...\n";
			--m_count;
		}
	}
	void DisplayMessage(std::string const& message) const override
	{
		m_displayCallback(message);
	}
	void SetSoldOutState() override
	{
		m_state = &m_soldOutState;
	}
	void SetNoQuarterState() override
	{
		m_state = &m_noQuarterState;
	}
	void SetSoldState() override
	{
		m_state = &m_soldState;
	}
	void SetHasQuarterState() override
	{
		m_state = &m_hasQuarterState;
	}
private:
	unsigned m_count = 0;
	CSoldState m_soldState;
	CSoldOutState m_soldOutState;
	CNoQuarterState m_noQuarterState;
	CHasQuarterState m_hasQuarterState;
	IState * m_state;
	DisplayCallback m_displayCallback;
};

}
