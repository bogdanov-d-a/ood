#pragma once

#include "Coordinate.h"
#include "Color.h"

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
};
