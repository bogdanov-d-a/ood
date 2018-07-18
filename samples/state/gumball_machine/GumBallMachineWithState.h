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

	virtual void SetSoldOutState() = 0;
	virtual void SetSoldState() = 0;
	virtual void SetBasicState() = 0;

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
		const auto qc = m_gumballMachine.GetQuarterCount();
		if (qc < MAX_QUARTER_COUNT)
		{
			m_gumballMachine.DisplayMessage("You inserted a quarter");
			m_gumballMachine.SetQuarterCount(qc + 1);
		}
		else
		{
			m_gumballMachine.DisplayMessage("You can't insert another quarter");
		}
	}
	void EjectQuarters() override
	{
		const auto qc = m_gumballMachine.GetQuarterCount();
		if (qc > 0)
		{
			m_gumballMachine.DisplayMessage(std::to_string(qc) + " quarter"
				+ (qc > 1 ? "s" : "")
				+ " returned");
		}
		else
		{
			m_gumballMachine.DisplayMessage("You can't eject, you haven't inserted a quarter yet");
		}
		m_gumballMachine.SetQuarterCount(0);
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
		const auto qc = m_gumballMachine.GetQuarterCount();
		if (qc > 0)
		{
			m_gumballMachine.DisplayMessage(std::to_string(qc) + " quarter"
				+ (qc > 1 ? "s" : "")
				+ " returned");
		}
		else
		{
			m_gumballMachine.DisplayMessage("You can't eject, you haven't inserted a quarter yet");
		}
		m_gumballMachine.SetQuarterCount(0);
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

class CBasicState : public IState
{
public:
	CBasicState(IGumballMachine & gumballMachine)
		:m_gumballMachine(gumballMachine)
	{}

	void InsertQuarter() override
	{
		const auto qc = m_gumballMachine.GetQuarterCount();
		if (qc < MAX_QUARTER_COUNT)
		{
			m_gumballMachine.DisplayMessage("You inserted a quarter");
			m_gumballMachine.SetQuarterCount(qc + 1);
		}
		else
		{
			m_gumballMachine.DisplayMessage("You can't insert another quarter");
		}
	}
	void EjectQuarters() override
	{
		const auto qc = m_gumballMachine.GetQuarterCount();
		if (qc > 0)
		{
			m_gumballMachine.DisplayMessage(std::to_string(qc) + " quarter"
				+ (qc > 1 ? "s" : "")
				+ " returned");
		}
		else
		{
			m_gumballMachine.DisplayMessage("You can't eject, you haven't inserted a quarter yet");
		}
		m_gumballMachine.SetQuarterCount(0);
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
	std::string ToString()const
	{
		auto fmt = boost::format(R"(
Mighty Gumball, Inc.
C++-enabled Standing Gumball Model #2016 (with state)
Inventory: %1% gumball%2%
Machine is %3%
)");
		return (fmt % m_gumballCount % (m_gumballCount != 1 ? "s" : "") % m_state->ToString()).str();
	}
private:
	unsigned GetBallCount() const override
	{
		return m_gumballCount;
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
