#include "stdafx.h"
#include "Ellipse.h"
#include "ICanvas.h"

CEllipse::CEllipse(Color color, CPoint const & center, Coordinate horizontalRadius, Coordinate verticalRadius)
	: CShape(color)
	, m_center(center)
	, m_horizontalRadius(horizontalRadius)
	, m_verticalRadius(verticalRadius)
{
}

void CEllipse::Draw(ICanvas & canvas) const
{
	CShape::Draw(canvas);
	canvas.DrawEllipse(m_center.GetX() - GetHorizontalRadius(), m_center.GetY() - GetVerticalRadius(),
		GetHorizontalRadius() * 2, GetVerticalRadius() * 2);
}

CPoint CEllipse::GetCenter() const
{
	return m_center;
}

Coordinate CEllipse::GetHorizontalRadius() const
{
	return m_horizontalRadius;
}

Coordinate CEllipse::GetVerticalRadius() const
{
	return m_verticalRadius;
}
