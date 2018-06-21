#include "stdafx.h"
#include "../LibISpringWord/History.h"
#include "../LibISpringWord/ICommand.h"

namespace
{

class MockCommand : public ICommand
{
public:
	using Callback = std::function<void()>;

	explicit MockCommand(Callback const& onExecute, Callback const& onUnexecute)
		: m_onExecute(onExecute)
		, m_onUnexecute(onUnexecute)
	{}

	void Execute() final
	{
		m_onExecute();
	}

	void Unexecute() final
	{
		m_onUnexecute();
	}

private:
	Callback m_onExecute;
	Callback m_onUnexecute;
};

}

BOOST_AUTO_TEST_CASE(CantUndoAndRedoAfterCreation)
{
	CHistory h;
	BOOST_CHECK(!h.CanUndo());
	BOOST_CHECK(!h.CanRedo());
}

BOOST_AUTO_TEST_CASE(ExecutesCommandOnAdd)
{
	bool executed = false;

	CHistory h;
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { executed = true; }, []() { throw std::exception(); }));

	BOOST_CHECK(executed);
}

BOOST_AUTO_TEST_CASE(TestUndoRedo)
{
	std::function<void()> execute;
	std::function<void()> unexecute;

	CHistory h;

	execute = []() {};
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { execute(); }, [&]() { unexecute(); }));

	{
		bool unexecuted = false;
		execute = []() { throw std::exception(); };
		unexecute = [&]() { unexecuted = true; };
		h.Undo();
		BOOST_CHECK(unexecuted);
		BOOST_CHECK(!h.CanUndo());
	}

	{
		bool executed = false;
		execute = [&]() { executed = true; };
		unexecute = []() { throw std::exception(); };
		h.Redo();
		BOOST_CHECK(executed);
		BOOST_CHECK(!h.CanRedo());
	}
}
