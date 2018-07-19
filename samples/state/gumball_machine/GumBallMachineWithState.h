#pragma once

namespace with_state
{

const unsigned MAX_QUARTER_COUNT = 5;

struct IState
{
	virtual void InsertQuarter() = 0;
	virtual void EjectQuarters() = 0;
	virtual void TurnCrank() = 0;
	virtual void Dispense() = 0;
	virtual void Refill(unsigned numBalls) = 0;
	virtual std::string ToString()const = 0;
	virtual ~IState() = default;
};

struct IGumballMachine
{
	virtual void ReleaseBall() = 0;
	virtual unsigned GetBallCount()const = 0;
	virtual void DisplayMessage(std::string const& message)const = 0;
	virtual unsigned GetQuarterCount()const = 0;
	virtual void SetQuarterCount(unsigned count) = 0;
	virtual void RefillImpl(unsigned numBalls) = 0;

	virtual void SetSoldOutState() = 0;
	virtual void SetSoldState() = 0;
	virtual void SetBasicState() = 0;

	virtual ~IGumballMachine() = default;
};

void InsertQuarterImpl(IGumballMachine &gumballMachine)
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

void EjectQuartersImpl(IGumballMachine &gumballMachine)
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

class CSoldState : public IState
{
public:
	CSoldState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}
	void InsertQuarter() override
	{
		InsertQuarterImpl(m_gumballMachine);
	}
	void EjectQuarters() override
	{
		EjectQuartersImpl(m_gumballMachine);
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
			m_gumballMachine.SetBasicState();
		}
	}
	void Refill(unsigned) override
	{
		m_gumballMachine.DisplayMessage("Can't refill machine while dispensing the gumball");
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
	void EjectQuarters() override
	{
		EjectQuartersImpl(m_gumballMachine);
	}
	void TurnCrank() override
	{
		m_gumballMachine.DisplayMessage("You turned but there's no gumballs");
	}
	void Dispense() override
	{
		m_gumballMachine.DisplayMessage("No gumball dispensed");
	}
	void Refill(unsigned numBalls) override
	{
		m_gumballMachine.RefillImpl(numBalls);
	}
	std::string ToString() const override
	{
		return "sold out";
	}
private:
	IGumballMachine & m_gumballMachine;
};

class CBasicState : public IState
{
public:
	CBasicState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}

	void InsertQuarter() override
	{
		InsertQuarterImpl(m_gumballMachine);
	}
	void EjectQuarters() override
	{
		EjectQuartersImpl(m_gumballMachine);
	}
	void TurnCrank() override
	{
		if (m_gumballMachine.GetQuarterCount() > 0)
		{
			m_gumballMachine.DisplayMessage("You turned...");
			m_gumballMachine.SetSoldState();
		}
		else
		{
			m_gumballMachine.DisplayMessage("You turned but there's no quarter");
		}
	}
	void Dispense() override
	{
		m_gumballMachine.DisplayMessage("No gumball dispensed");
	}
	void Refill(unsigned numBalls) override
	{
		m_gumballMachine.RefillImpl(numBalls);
	}
	std::string ToString() const override
	{
		return "ready";
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
		, m_basicState(*this)
		, m_state(&m_soldOutState)
		, m_gumballCount(numBalls)
		, m_displayCallback(displayCallback)
	{
		if (m_gumballCount > 0)
		{
			m_state = &m_basicState;
		}
	}
	void EjectQuarters()
	{
		m_state->EjectQuarters();
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
	void Refill(unsigned numBalls)
	{
		m_state->Refill(numBalls);
	}
	std::string ToString()const
	{
		auto fmt = boost::format(R"(
Mighty Gumball, Inc.
C++-enabled Standing Gumball Model #2016 (with state)
Inventory: %1% gumball%2%, %3% quarter%4%
Machine is %5%
)");
		return (fmt
			% m_gumballCount % (m_gumballCount != 1 ? "s" : "")
			% m_quarterCount % (m_quarterCount != 1 ? "s" : "")
			% m_state->ToString()).str();
	}
private:
	unsigned GetBallCount() const override
	{
		return m_gumballCount;
	}
	void RefillImpl(unsigned numBalls) override
	{
		m_gumballCount = numBalls;
		numBalls > 0 ? SetBasicState() : SetSoldOutState();
		DisplayMessage("Machine refilled to " + std::to_string(numBalls) + " gumballs");
	}
	virtual void ReleaseBall() override
	{
		if (m_gumballCount != 0)
		{
			DisplayMessage("A gumball comes rolling out the slot...");
			--m_gumballCount;

			assert(m_quarterCount > 0);
			--m_quarterCount;
		}
	}
	void DisplayMessage(std::string const& message) const override
	{
		m_displayCallback(message);
	}
	unsigned GetQuarterCount()const override
	{
		return m_quarterCount;
	}
	void SetQuarterCount(unsigned count)
	{
		m_quarterCount = count;
	}
	void SetSoldOutState() override
	{
		m_state = &m_soldOutState;
	}
	void SetBasicState() override
	{
		m_state = &m_basicState;
	}
	void SetSoldState() override
	{
		m_state = &m_soldState;
	}
private:
	unsigned m_gumballCount = 0;
	unsigned m_quarterCount = 0;
	CSoldState m_soldState;
	CSoldOutState m_soldOutState;
	CBasicState m_basicState;
	IState * m_state;
	DisplayCallback m_displayCallback;
};

}
