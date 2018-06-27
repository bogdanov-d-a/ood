#include "stdafx.h"
#include "Utils.h"

sf::Vector2f Utils::PointToSfmlVector(PointD const & point)
{
	return sf::Vector2f(point.x, point.y);
}
