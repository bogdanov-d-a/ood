#pragma once

#include "Shape.h"
#include "Point.h"

class CEllipse : public CShape
{
public:
	CEllipse(Color color, CPoint const& center, CUtils::Coordinate horizontalRadius, CUtils::Coordinate verticalRadius);
	void Draw(ICanvas &canvas) const final;

private:
	CPoint GetCenter() const;
	CUtils::Coordinate GetHorizontalRadius() const;
	CUtils::Coordinate GetVerticalRadius() const;

	CPoint m_center;
	CUtils::Coordinate m_horizontalRadius;
	CUtils::Coordinate m_verticalRadius;
};
