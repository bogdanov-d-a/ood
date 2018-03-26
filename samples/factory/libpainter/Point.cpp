#include "stdafx.h"
#include "Point.h"

CPoint::CPoint()
	: CPoint(0, 0)
{
}

CPoint::CPoint(CUtils::Coordinate x, CUtils::Coordinate y)
	: m_x(x)
	, m_y(y)
{
}

void CPoint::SetX(CUtils::Coordinate x)
{
	m_x = x;
}

void CPoint::SetY(CUtils::Coordinate y)
{
	m_y = y;
}

CUtils::Coordinate CPoint::GetX() const
{
	return m_x;
}

CUtils::Coordinate CPoint::GetY() const
{
	return m_y;
}

std::string CPoint::ToString() const
{
	return std::string("(") + CUtils::ToString(m_x) + ";" + CUtils::ToString(m_y) + ")";
}
