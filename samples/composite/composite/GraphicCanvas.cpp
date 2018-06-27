#include "stdafx.h"
#include "GraphicCanvas.h"
#include "Utils.h"

GraphicCanvas::GraphicCanvas(sf::RenderTarget & target)
	: m_target(target)
{
}

void GraphicCanvas::SetFillColor(RGBAColor color)
{
}

void GraphicCanvas::ResetFillColor()
{
}

void GraphicCanvas::SetLineStyle(RGBAColor color, double thickness)
{
}

void GraphicCanvas::ResetLineStyle()
{
}

void GraphicCanvas::DrawPolygon(std::vector<PointD> const & points)
{
	sf::ConvexShape convex;
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
