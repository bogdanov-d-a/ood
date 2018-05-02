#pragma once

#include "IBeverage.h"
#include <map>

// Базовая реализация напитка, предоставляющая его описание
class CBeverage : public IBeverage
{
public:
	CBeverage(const std::string & description)
		:m_description(description)
	{}

	std::string GetDescription()const override final
	{
		return m_description;
	}
private:
	std::string m_description;
};

// Кофе
class CCoffee : public CBeverage
{
public:
	CCoffee(const std::string& description = "Coffee")
		:CBeverage(description) 
	{}

	double GetCost() const override 
	{
		return 60; 
	}
};

enum class CapuccinoSize
{
	Regular,
	Double,
};

// Капуччино
class CCapuccino : public CCoffee
{
public:
	CCapuccino(CapuccinoSize size = CapuccinoSize::Regular)
		: CCoffee(std::string("Capuccino") + (size == CapuccinoSize::Double ? " (double)" : ""))
		, m_size(size)
	{}

	double GetCost() const override 
	{
		return m_size == CapuccinoSize::Double ? 120 : 80;
	}

private:
	CapuccinoSize m_size;
};

enum class LatteSize
{
	Regular,
	Double,
};

// Латте
class CLatte : public CCoffee
{
public:
	CLatte(LatteSize size = LatteSize::Regular)
		: CCoffee(std::string("Latte") + (size == LatteSize::Double ? " (double)" : ""))
		, m_size(size)
	{}

	double GetCost() const override 
	{
		return m_size == LatteSize::Double ? 130 : 90;
	}

private:
	LatteSize m_size;
};

enum class TeaType
{
	Red,
	Yellow,
	Green,
	Blue,
};

namespace
{

const std::map<TeaType, std::string> TEA_TYPE_TO_NAME = {
	{ TeaType::Red, "Red" },
	{ TeaType::Yellow, "Yellow" },
	{ TeaType::Green, "Green" },
	{ TeaType::Blue, "Blue" },
};

}

// Чай
class CTea : public CBeverage
{
public:
	CTea(TeaType type) 
		: CBeverage(std::string("Tea (") + TEA_TYPE_TO_NAME.at(type) + ")")
	{}

	double GetCost() const override 
	{
		return 30; 
	}
};

enum class MilkshakeSize
{
	Small,
	Medium,
	Large,
};

namespace
{

const std::map<MilkshakeSize, std::pair<std::string, double>> MILKSHAKE_SIZE_TO_DATA = {
	{ MilkshakeSize::Small, { "Small", 50 } },
	{ MilkshakeSize::Medium, { "Medium", 60 } },
	{ MilkshakeSize::Large, { "Large", 80 } },
};

}

// Молочный коктейль
class CMilkshake : public CBeverage
{
public:
	CMilkshake(MilkshakeSize size = MilkshakeSize::Large)
		: CBeverage(std::string("Milkshake (") + MILKSHAKE_SIZE_TO_DATA.at(size).first + ")")
		, m_size(size)
	{}

	double GetCost() const override 
	{ 
		return MILKSHAKE_SIZE_TO_DATA.at(m_size).second;
	}

private:
	MilkshakeSize m_size;
};
