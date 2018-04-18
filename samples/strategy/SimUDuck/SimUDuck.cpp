#include <cassert>
#include <iostream>
#include <memory>
#include <vector>
#include <functional>

using namespace std;


function<void()> MakeCounterFlyBehavior(function<void(unsigned)> const& flyImpl)
{
	return [flyImpl, count = 0]() mutable {
		++count;
		flyImpl(count);
	};
}

function<void()> MakeFlyWithWings()
{
	return MakeCounterFlyBehavior([](unsigned count) {
		cout << "I'm flying with wings for " << count << "th time!!" << endl;
	});
}

const auto flyNoWay = []() {};


const auto quackBehavior = []() {
	cout << "Quack Quack!!!" << endl;
};
const auto squeakBehavior = []() {
	cout << "Squeek!!!" << endl;
};
const auto muteQuackBehavior = []() {};


const auto waltzDanceBehavior = []() {
	cout << "I'm dancing waltz!" << endl;
};
const auto minuetDanceBehavior = []() {
	cout << "I'm dancing minuet!" << endl;
};
const auto danceNoWay = []() {};


class Duck
{
public:
	Duck(function<void()>&& flyBehavior,
		function<void()>&& quackBehavior,
		function<void()>&& danceBehavior)
		: m_quackBehavior(move(quackBehavior))
		, m_danceBehavior(move(danceBehavior))
	{
		SetFlyBehavior(move(flyBehavior));
	}
	void Quack() const
	{
		m_quackBehavior();
	}
	void Swim()
	{
		cout << "I'm swimming" << endl;
	}
	void Fly()
	{
		m_flyBehavior();
	}
	void Dance()
	{
		m_danceBehavior();
	}
	void SetFlyBehavior(function<void()>&& flyBehavior)
	{
		m_flyBehavior = move(flyBehavior);
	}
	virtual void Display() const = 0;
	virtual ~Duck() = default;

private:
	function<void()> m_flyBehavior;
	function<void()> m_quackBehavior;
	function<void()> m_danceBehavior;
};

class MallardDuck : public Duck
{
public:
	MallardDuck()
		: Duck(MakeFlyWithWings(), quackBehavior, waltzDanceBehavior)
	{
	}

	void Display() const override
	{
		cout << "I'm mallard duck" << endl;
	}
};

class RedheadDuck : public Duck
{
public:
	RedheadDuck()
		: Duck(MakeFlyWithWings(), quackBehavior, minuetDanceBehavior)
	{
	}
	void Display() const override
	{
		cout << "I'm redhead duck" << endl;
	}
};
class DeckoyDuck : public Duck
{
public:
	DeckoyDuck()
		: Duck(flyNoWay, muteQuackBehavior, danceNoWay)
	{
	}
	void Display() const override
	{
		cout << "I'm deckoy duck" << endl;
	}
};
class RubberDuck : public Duck
{
public:
	RubberDuck()
		: Duck(flyNoWay, squeakBehavior, danceNoWay)
	{
	}
	void Display() const override
	{
		cout << "I'm rubber duck" << endl;
	}
};

class ModelDuck : public Duck
{
public:
	ModelDuck()
		: Duck(flyNoWay, quackBehavior, danceNoWay)
	{
	}
	void Display() const override
	{
		cout << "I'm model duck" << endl;
	}
};

void DrawDuck(Duck const& duck)
{
	duck.Display();
}

void PlayWithDuck(Duck& duck)
{
	DrawDuck(duck);
	duck.Fly();
	duck.Quack();
	duck.Dance();
	duck.Fly();
	cout << endl;
}

void main()
{
	MallardDuck mallarDuck;
	PlayWithDuck(mallarDuck);

	RedheadDuck redheadDuck;
	PlayWithDuck(redheadDuck);

	RubberDuck rubberDuck;
	PlayWithDuck(rubberDuck);

	DeckoyDuck deckoyDuck;
	PlayWithDuck(deckoyDuck);

	ModelDuck modelDuck;
	PlayWithDuck(modelDuck);
	modelDuck.SetFlyBehavior(MakeFlyWithWings());
	PlayWithDuck(modelDuck);
}
