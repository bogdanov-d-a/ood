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
		convex.setOutlineThickness(m_lineStyle->second);
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
}
