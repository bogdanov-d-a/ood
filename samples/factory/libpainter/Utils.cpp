#include "stdafx.h"
#include "Utils.h"

namespace
{

const std::map<std::string, Color> stringToColor = {
	{ "green", Color::GREEN },
	{ "red", Color::RED },
	{ "blue", Color::BLUE },
	{ "yellow", Color::YELLOW },
	{ "pink", Color::PINK },
	{ "black", Color::BLACK },
};

}

Coordinate CUtils::ParseCoordinate(std::string const & s)
{
	return std::stoi(s);
}

std::string CUtils::ToString(Coordinate const & c)
{
	return std::to_string(c);
}

Color CUtils::ParseColor(std::string const & s)
{
	const auto it = stringToColor.find(s);
	if (it == stringToColor.end())
	{
		throw std::runtime_error("Invalid color " + s);
	}
	return it->second;
}

std::string CUtils::ToString(Color color)
{
	switch (color)
	{
	case Color::GREEN:
		return "green";
	case Color::RED:
		return "red";
	case Color::BLUE:
		return "blue";
	case Color::YELLOW:
		return "yellow";
	case Color::PINK:
		return "pink";
	case Color::BLACK:
		return "black";
	default:
		throw std::runtime_error("Unknown color value");
	}
}

std::vector<std::string> CUtils::SplitWords(std::string const & s)
{
	std::istringstream iss(s);
	std::vector<std::string> tokens;
	std::copy(std::istream_iterator<std::string>(iss),
		std::istream_iterator<std::string>(),
		std::back_inserter(tokens));
	return tokens;
}

CUtils::ParseShapeResult CUtils::ParseShape(std::string const & s)
{
	ParseShapeResult result;
	const auto words = SplitWords(s);

	if (words.size() < 2)
	{
		throw std::runtime_error("Not enough tokens");
	}

	result.type = words[0];
	result.color = ParseColor(words[1]);
	result.ints.reserve(words.size() - 2);

	for (auto it = ++++words.begin(); it != words.end(); ++it)
	{
		result.ints.push_back(std::stoi(*it));
	}

	return result;
}

Coordinate CUtils::RoundFloatToCoordinate(double d)
{
	return static_cast<Coordinate>(round(d));
}

sf::Color CUtils::ColorToSfmlColor(Color color)
{
	switch (color)
	{
	case Color::GREEN:
		return sf::Color(0, 255, 0);
	case Color::RED:
		return sf::Color(255, 0, 0);
	case Color::BLUE:
		return sf::Color(0, 0, 255);
	case Color::YELLOW:
		return sf::Color(255, 255, 0);
	case Color::PINK:
		return sf::Color(255, 0, 255);
	case Color::BLACK:
		return sf::Color(0, 0, 0);
	default:
		throw std::runtime_error("Unknown color value");
	}
}

sf::Vector2f CUtils::PointToSfmlVector(CPoint const & point)
{
	return sf::Vector2f(static_cast<float>(point.GetX()), static_cast<float>(point.GetY()));
}
