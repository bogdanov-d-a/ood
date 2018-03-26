#pragma once

#include "Shape.h"
#include "Point.h"

class CTriangle : public CShape
{
public:
	CTriangle(Color color, CPoint const& vertex1, CPoint const& vertex2, CPoint const& vertex3);
	void Draw(ICanvas &canvas) const final;

private:
	CPoint GetVertex1() const;
	CPoint GetVertex2() const;
	CPoint GetVertex3() const;

	CPoint m_vertex1;
	CPoint m_vertex2;
	CPoint m_vertex3;
};
