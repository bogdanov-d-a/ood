#pragma once

#include "IBeverage.h"

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

// Капуччино
class CCapuccino : public CCoffee
{
public:
	CCapuccino() 
		:CCoffee("Capuccino") 
	{}

	double GetCost() const override 
	{
		return 80; 
	}
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

// Чай
class CTea : public CBeverage
{
public:
	CTea() 
		:CBeverage("Tea") 
	{}

	double GetCost() const override 
	{
		return 30; 
	}
};

// Молочный коктейль
class CMilkshake : public CBeverage
{
public:
	CMilkshake() 
		:CBeverage("Milkshake") 
	{}

	double GetCost() const override 
	{ 
		return 80; 
	}
};
