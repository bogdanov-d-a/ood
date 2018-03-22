#include <cassert>
#include <iostream>
#include <memory>
#include <vector>

using namespace std;

struct IFlyBehavior
{
	virtual ~IFlyBehavior(){};
	virtual void Fly() = 0;
};
class FlyWithWings : public IFlyBehavior
{
public:
	void Fly() override
	{
		cout << "I'm flying with wings!!" << endl;
	}
};

class FlyNoWay : public IFlyBehavior
{
public:
	void Fly() override {}
};

struct IQuackBehavior
{
	virtual ~IQuackBehavior(){};
	virtual void Quack() = 0;
};

class QuackBehavior : public IQuackBehavior
{
public:
	void Quack() override
	{
		cout << "Quack Quack!!!" << endl;
	}
};
class SqueakBehavior : public IQuackBehavior
{
public:
	void Quack() override
	{
		cout << "Squeek!!!" << endl;
	}
};

class MuteQuackBehavior : public IQuackBehavior
{
public:
	void Quack() override {}
};

struct IDanceBehavior
{
	virtual ~IDanceBehavior() {};
	virtual void Dance() = 0;
};

class WaltzDanceBehavior : public IDanceBehavior
{
public:
	void Dance() override
	{
		cout << "I'm dancing waltz!" << endl;
	}
};

class MinuetDanceBehavior : public IDanceBehavior
{
public:
	void Dance() override
	{
		cout << "I'm dancing minuet!" << endl;
	}
};

class DanceNoWay : public IDanceBehavior
{
public:
	void Dance() override {}
};

class Duck
{
public:
	Duck(unique_ptr<IFlyBehavior>&& flyBehavior,
		unique_ptr<IQuackBehavior>&& quackBehavior,
		unique_ptr<IDanceBehavior>&& danceBehavior)
		: m_quackBehavior(move(quackBehavior))
		, m_danceBehavior(move(danceBehavior))
	{
		assert(m_quackBehavior);
		assert(m_danceBehavior);
		SetFlyBehavior(move(flyBehavior));
	}
	void Quack() const
	{
		m_quackBehavior->Quack();
	}
	void Swim()
	{
		cout << "I'm swimming" << endl;
	}
	void Fly()
	{
		m_flyBehavior->Fly();
	}
	void Dance()
	{
		m_danceBehavior->Dance();
	}
	void SetFlyBehavior(unique_ptr<IFlyBehavior>&& flyBehavior)
	{
		assert(flyBehavior);
		m_flyBehavior = move(flyBehavior);
	}
	virtual void Display() const = 0;
	virtual ~Duck() = default;

private:
	unique_ptr<IFlyBehavior> m_flyBehavior;
	unique_ptr<IQuackBehavior> m_quackBehavior;
	unique_ptr<IDanceBehavior> m_danceBehavior;
};

class MallardDuck : public Duck
{
public:
	MallardDuck()
		: Duck(make_unique<FlyWithWings>(), make_unique<QuackBehavior>(), make_unique<WaltzDanceBehavior>())
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
		: Duck(make_unique<FlyWithWings>(), make_unique<QuackBehavior>(), make_unique<MinuetDanceBehavior>())
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
		: Duck(make_unique<FlyNoWay>(), make_unique<MuteQuackBehavior>(), make_unique<DanceNoWay>())
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
		: Duck(make_unique<FlyNoWay>(), make_unique<SqueakBehavior>(), make_unique<DanceNoWay>())
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
		: Duck(make_unique<FlyNoWay>(), make_unique<QuackBehavior>(), make_unique<DanceNoWay>())
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
	duck.Quack();
	duck.Fly();
	duck.Dance();
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
	modelDuck.SetFlyBehavior(make_unique<FlyWithWings>());
	PlayWithDuck(modelDuck);
}
