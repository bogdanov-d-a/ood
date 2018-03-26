#pragma once

#include "Shape.h"
#include "Point.h"

class CRegularPolygon : public CShape
{
public:
	CRegularPolygon(Color color, size_t vertexCount, CPoint const& center, CUtils::Coordinate radius);
	void Draw(ICanvas &canvas) const final;

private:
	size_t GetVertexCount() const;
	CPoint GetCenter() const;
	CUtils::Coordinate GetRadius() const;

	size_t m_vertexCount;
	CPoint m_center;
	CUtils::Coordinate m_radius;
};
