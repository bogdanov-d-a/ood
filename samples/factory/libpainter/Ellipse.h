#pragma once

#include "Shape.h"
#include "Point.h"

class CEllipse : public CShape
{
public:
	CEllipse(Color color, CPoint const& center, Coordinate horizontalRadius, Coordinate verticalRadius);
	void Draw(ICanvas &canvas) const final;

private:
	CPoint GetCenter() const;
	Coordinate GetHorizontalRadius() const;
	Coordinate GetVerticalRadius() const;

	CPoint m_center;
	Coordinate m_horizontalRadius;
	Coordinate m_verticalRadius;
};
