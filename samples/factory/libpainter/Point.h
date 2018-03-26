#pragma once

#include "Utils.h"

class CPoint
{
public:
	CPoint();
	CPoint(CUtils::Coordinate x, CUtils::Coordinate y);

	void SetX(CUtils::Coordinate x);
	void SetY(CUtils::Coordinate y);

	CUtils::Coordinate GetX() const;
	CUtils::Coordinate GetY() const;

	std::string ToString() const;

private:
	CUtils::Coordinate m_x;
	CUtils::Coordinate m_y;
};
