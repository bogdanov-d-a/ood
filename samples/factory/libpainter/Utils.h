#pragma once

#include "Coordinate.h"
#include "Color.h"
#include <SFML/Graphics.hpp>
#include "Point.h"

class CUtils
{
public:
	CUtils() = delete;

	struct ParseShapeResult
	{
		std::string type;
		Color color;
		std::vector<int> ints;
	};

	static Coordinate ParseCoordinate(std::string const& s);
	static std::string ToString(Coordinate const& c);

	static Color ParseColor(std::string const& s);
	static std::string ToString(Color color);

	static std::vector<std::string> SplitWords(std::string const& s);
	static ParseShapeResult ParseShape(std::string const& s);
	static Coordinate RoundFloatToCoordinate(double d);

	static sf::Color ColorToSfmlColor(Color color);
	static sf::Vector2f PointToSfmlVector(CPoint const& point);
};
