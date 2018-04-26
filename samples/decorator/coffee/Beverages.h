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

enum class CapuccinoType
{
	Regular,
	Double,
};

// Капуччино
class CCapuccino : public CCoffee
{
public:
	CCapuccino(CapuccinoType type = CapuccinoType::Regular)
		: CCoffee(std::string("Capuccino") + (type == CapuccinoType::Double ? " (double)" : ""))
		, m_type(type)
	{}

	double GetCost() const override 
	{
		return m_type == CapuccinoType::Double ? 120 : 80;
	}

private:
	CapuccinoType m_type;
};

enum class LatteType
{
	Regular,
	Double,
};

// Латте
class CLatte : public CCoffee
{
public:
	CLatte(LatteType type = LatteType::Regular)
		: CCoffee(std::string("Latte") + (type == LatteType::Double ? " (double)" : ""))
		, m_type(type)
	{}

	double GetCost() const override 
	{
		return m_type == LatteType::Double ? 130 : 90;
	}

private:
	LatteType m_type;
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

enum class MilkshakeType
{
	Small,
	Medium,
	Large,
};

namespace
{

const std::map<MilkshakeType, std::pair<std::string, double>> MILKSHAKE_TYPE_TO_DATA = {
	{ MilkshakeType::Small, { "Small", 50 } },
	{ MilkshakeType::Medium, { "Medium", 60 } },
	{ MilkshakeType::Large, { "Large", 80 } },
};

}

// Молочный коктейль
class CMilkshake : public CBeverage
{
public:
	CMilkshake(MilkshakeType type = MilkshakeType::Large)
		: CBeverage(std::string("Milkshake (") + MILKSHAKE_TYPE_TO_DATA.at(type).first + ")")
		, m_type(type)
	{}

	double GetCost() const override 
	{ 
		return MILKSHAKE_TYPE_TO_DATA.at(m_type).second;
	}

private:
	MilkshakeType m_type;
};
