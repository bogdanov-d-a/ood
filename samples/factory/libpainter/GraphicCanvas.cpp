#include "stdafx.h"
#include "GraphicCanvas.h"
#include "Utils.h"

namespace
{
constexpr size_t ellipseVertexCount = 250;
}

CGraphicCanvas::CGraphicCanvas(sf::RenderWindow & window)
	: m_window(window)
{
}

void CGraphicCanvas::SetColor(Color color)
{
	m_color = color;
}

void CGraphicCanvas::DrawLine(CPoint const & from, CPoint const & to)
{
	sf::VertexArray lines(sf::LinesStrip, 2);
	lines[0].position = CUtils::PointToSfmlVector(from);
	lines[0].color = CUtils::ColorToSfmlColor(m_color);
	lines[1].position = CUtils::PointToSfmlVector(to);
	lines[1].color = CUtils::ColorToSfmlColor(m_color);
	m_window.draw(lines);
}

void CGraphicCanvas::DrawEllipse(Coordinate left, Coordinate top, Coordinate width, Coordinate height)
{
	sf::Vector2f radius(static_cast<float>(width / 2), static_cast<float>(height / 2));
	sf::Vector2f center(static_cast<float>(left + radius.x), static_cast<float>(top + radius.y));

	boost::optional<CPoint> firstPoint;
	boost::optional<CPoint> lastPoint;

	for (size_t curVertex = 0; curVertex < ellipseVertexCount; ++curVertex)
	{
		const auto phi = curVertex * 2 * M_PI / ellipseVertexCount;
		const auto x = center.x + radius.x * cos(phi);
		const auto y = center.y + radius.y * sin(phi);
		const CPoint curPoint(CUtils::RoundFloatToCoordinate(x), CUtils::RoundFloatToCoordinate(y));

		if (firstPoint == boost::none)
		{
			firstPoint = curPoint;
			lastPoint = curPoint;
		}
		else
		{
			DrawLine(*lastPoint, curPoint);
			lastPoint = curPoint;
		}
	}

	DrawLine(*lastPoint, *firstPoint);
}
