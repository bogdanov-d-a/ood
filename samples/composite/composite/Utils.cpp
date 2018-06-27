#include "stdafx.h"
#include "Utils.h"

sf::Vector2f Utils::PointToSfmlVector(PointD const & point)
{
	return sf::Vector2f(static_cast<float>(point.x), static_cast<float>(point.y));
}

sf::Color Utils::ColorToSfmlColor(RGBAColor color)
{
	return sf::Color(
		(color & (0xff << 24)) >> 24,
		(color & (0xff << 16)) >> 16,
		(color & (0xff << 8)) >> 8,
		color & 0xff
	);
}
