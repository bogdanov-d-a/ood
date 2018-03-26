#pragma once

#include "Shape.h"
#include "Point.h"

class CRectangle : public CShape
{
public:
	CRectangle(Color color, CPoint const& leftTop, CPoint const& rightBottom);
	void Draw(ICanvas &canvas) const final;

private:
	CPoint GetRightTop() const;
	CPoint GetLeftBottom() const;

	CPoint m_leftTop;
	CPoint m_rightBottom;
};
