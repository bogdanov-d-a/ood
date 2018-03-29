#pragma once

#include "Coordinate.h"

class CPoint
{
public:
	CPoint();
	CPoint(Coordinate x, Coordinate y);

	void SetX(Coordinate x);
	void SetY(Coordinate y);

	Coordinate GetX() const;
	Coordinate GetY() const;

	std::string ToString() const;

private:
	Coordinate m_x;
	Coordinate m_y;
};
