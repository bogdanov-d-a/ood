#include "stdafx.h"
#include "Ellipse.h"
#include "ICanvas.h"

CEllipse::CEllipse(Color color, CPoint const & center, CUtils::Coordinate horizontalRadius, CUtils::Coordinate verticalRadius)
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
		m_center.GetX() + GetHorizontalRadius(), m_center.GetY() + GetVerticalRadius());
}

CPoint CEllipse::GetCenter() const
{
	return m_center;
}

CUtils::Coordinate CEllipse::GetHorizontalRadius() const
{
	return m_horizontalRadius;
}

CUtils::Coordinate CEllipse::GetVerticalRadius() const
{
	return m_verticalRadius;
}
