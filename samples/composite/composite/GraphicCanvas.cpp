#include "stdafx.h"
#include "GraphicCanvas.h"
#include "Utils.h"

GraphicCanvas::GraphicCanvas(sf::RenderTarget & target)
	: m_target(target)
{
}

void GraphicCanvas::SetFillColor(RGBAColor color)
{
	m_fillColor = color;
}

void GraphicCanvas::ResetFillColor()
{
	m_fillColor = boost::none;
}

void GraphicCanvas::SetLineStyle(RGBAColor color, double thickness)
{
	m_lineStyle = std::make_pair(color, thickness);
}

void GraphicCanvas::ResetLineStyle()
{
	m_lineStyle = boost::none;
}

void GraphicCanvas::DrawPolygon(std::vector<PointD> const & points)
{
	sf::ConvexShape convex;

	convex.setFillColor(m_fillColor ? Utils::ColorToSfmlColor(*m_fillColor) : sf::Color::Transparent);

	if (m_lineStyle)
	{
		convex.setOutlineColor(Utils::ColorToSfmlColor(m_lineStyle->first));
		convex.setOutlineThickness(static_cast<float>(m_lineStyle->second));
	}

	convex.setPointCount(points.size());
	for (size_t i = 0; i < points.size(); ++i)
	{
		convex.setPoint(i, Utils::PointToSfmlVector(points[i]));
	}

	m_target.draw(convex);
}

void GraphicCanvas::DrawEllipse(double left, double top, double width, double height)
{
	const sf::Vector2f radius(static_cast<float>(width / 2), static_cast<float>(height / 2));
	const sf::Vector2f center(static_cast<float>(left + radius.x), static_cast<float>(top + radius.y));

	constexpr size_t ellipseVertexCount = 250;
	std::vector<PointD> points(ellipseVertexCount);

	for (size_t curVertexIndex = 0; curVertexIndex < ellipseVertexCount; ++curVertexIndex)
	{
		const auto phi = curVertexIndex * 2 * M_PI / ellipseVertexCount;
		const auto x = center.x + radius.x * cos(phi);
		const auto y = center.y + radius.y * sin(phi);
		const PointD curVertex = { x, y };
		points[curVertexIndex] = curVertex;
	}

	DrawPolygon(points);
}
