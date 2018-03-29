#include "stdafx.h"
#include "Point.h"
#include "Utils.h"

CPoint::CPoint()
	: CPoint(0, 0)
{
}

CPoint::CPoint(Coordinate x, Coordinate y)
	: m_x(x)
	, m_y(y)
{
}

void CPoint::SetX(Coordinate x)
{
	m_x = x;
}

void CPoint::SetY(Coordinate y)
{
	m_y = y;
}

Coordinate CPoint::GetX() const
{
	return m_x;
}

Coordinate CPoint::GetY() const
{
	return m_y;
}

std::string CPoint::ToString() const
{
	return std::string("(") + CUtils::ToString(m_x) + ";" + CUtils::ToString(m_y) + ")";
}
