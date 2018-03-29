#include "stdafx.h"
#include "RegularPolygon.h"
#include "ICanvas.h"
#include "Utils.h"

CRegularPolygon::CRegularPolygon(Color color, size_t vertexCount, CPoint const & center, Coordinate radius)
	: CShape(color)
	, m_vertexCount(vertexCount)
	, m_center(center)
	, m_radius(radius)
{
}

void CRegularPolygon::Draw(ICanvas & canvas) const
{
	CShape::Draw(canvas);

	boost::optional<CPoint> firstPoint;
	boost::optional<CPoint> lastPoint;

	for (size_t curVertex = 0; curVertex < GetVertexCount(); ++curVertex)
	{
		const auto phi = curVertex * 2 * M_PI / GetVertexCount();
		const auto x = GetRadius() * cos(phi);
		const auto y = GetRadius() * sin(phi);
		const CPoint curPoint(CUtils::RoundFloatToCoordinate(x), CUtils::RoundFloatToCoordinate(y));

		if (firstPoint == boost::none)
		{
			firstPoint = curPoint;
			lastPoint = curPoint;
		}
		else
		{
			canvas.DrawLine(*lastPoint, curPoint);
			lastPoint = curPoint;
		}
	}

	canvas.DrawLine(*lastPoint, *firstPoint);
}

size_t CRegularPolygon::GetVertexCount() const
{
	return m_vertexCount;
}

CPoint CRegularPolygon::GetCenter() const
{
	return m_center;
}

Coordinate CRegularPolygon::GetRadius() const
{
	return m_radius;
}
