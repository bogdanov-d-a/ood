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

BOOST_AUTO_TEST_CASE(AddRemovesRedoCommands)
{
	CHistory h;

	std::function<void()> execute1;
	std::function<void()> unexecute1;

	execute1 = []() {};
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { execute1(); }, [&]() { unexecute1(); }));
	execute1 = []() { throw std::exception(); };

	std::function<void()> execute2;
	std::function<void()> unexecute2;

	execute2 = []() {};
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { execute2(); }, [&]() { unexecute2(); }));
	execute2 = []() { throw std::exception(); };

	std::function<void()> execute3;
	std::function<void()> unexecute3;

	execute3 = []() {};
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { execute3(); }, [&]() { unexecute3(); }));
	execute3 = []() { throw std::exception(); };

	unexecute3 = []() {};
	h.Undo();
	unexecute3 = []() { throw std::exception(); };

	std::function<void()> execute4;
	std::function<void()> unexecute4;

	execute4 = []() {};
	h.AddAndExecuteCommand(std::make_unique<MockCommand>(
		[&]() { execute4(); }, [&]() { unexecute4(); }));
	execute4 = []() { throw std::exception(); };

	unexecute4 = []() {};
	h.Undo();
	unexecute4 = []() { throw std::exception(); };

	unexecute2 = []() {};
	h.Undo();
	unexecute2 = []() { throw std::exception(); };
}
