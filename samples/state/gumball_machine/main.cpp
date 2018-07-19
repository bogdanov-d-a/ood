#include "deps.h"
#include "NaiveGumBallMachine.h"
#include "GumBallMachineWithState.h"
#include "GumBallMachineWithDynamicallyCreatedState.h"
#include "Menu.h"

using namespace std;

namespace
{

template <typename GumballMachineType>
void TestGumballMachine(GumballMachineType & m)
{
	cout << m.ToString() << endl;

	m.InsertQuarter();
	m.TurnCrank();

	cout << m.ToString() << endl;

	m.InsertQuarter();
	m.EjectQuarters();
	m.TurnCrank();

	cout << m.ToString() << endl;

	m.InsertQuarter();
	m.TurnCrank();
	m.InsertQuarter();
	m.TurnCrank();
	m.EjectQuarters();

	cout << m.ToString() << endl;

	m.InsertQuarter();
	m.InsertQuarter();
	m.TurnCrank();
	m.InsertQuarter();
	m.TurnCrank();
	m.InsertQuarter();
	m.TurnCrank();

	cout << m.ToString() << endl;
}

void TestNaiveGumballMachine()
{
	naive::CGumballMachine m(5, [](std::string const& message) {
		std::cout << message << std::endl;
	});
	TestGumballMachine(m);
}

void TestGumballMachineWithState()
{
	with_state::CGumballMachine m(5, [](std::string const& message) {
		std::cout << message << std::endl;
	});
	TestGumballMachine(m);
}

void TestGumballMachineWithDynamicState()
{
	with_dynamic_state::CGumballMachine m(5);
	TestGumballMachine(m);
}

}

int main()
{
	/*TestNaiveGumballMachine();

	cout << "\n-----------------\n";
	TestGumballMachineWithState();

	cout << "\n-----------------\n";
	TestGumballMachineWithDynamicState();*/

	CMenu menu;
	with_state::CGumballMachine machine(2, [](std::string const& message) {
		std::cout << message << std::endl;
	});

	menu.AddItem("Help", "Help", [&](std::istream&) {
		menu.ShowInstructions();
	});
	menu.AddItem("Exit", "Exit", [&](std::istream&) {
		menu.Exit();
	});
	menu.AddItem("State", "Show machine state", [&](std::istream&) {
		cout << machine.ToString() << endl;
	});
	menu.AddItem("Insert", "Insert quarter", [&](std::istream&) {
		machine.InsertQuarter();
	});
	menu.AddItem("Eject", "Eject quarters", [&](std::istream&) {
		machine.EjectQuarters();
	});
	menu.AddItem("Turn", "Turn crank", [&](std::istream&) {
		machine.TurnCrank();
	});
	menu.AddItem("Refill", "Refill <new count>", [&](std::istream &stream) {
		unsigned count = 0;
		stream >> count;
		machine.Refill(count);
	});

	menu.Run();

	return 0;
}
