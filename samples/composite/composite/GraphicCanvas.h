#pragma once

#include "ICanvas.h"
#include <SFML/Graphics.hpp>

class GraphicCanvas : public ICanvas
{
public:
	explicit GraphicCanvas(sf::RenderTarget &target);

	void SetFillColor(RGBAColor color) final;
	void ResetFillColor() final;

	void SetLineStyle(RGBAColor color, double thickness) final;
	void ResetLineStyle() final;

	void DrawPolygon(std::vector<PointD> const& points) final;
	void DrawEllipse(double left, double top, double width, double height) final;

private:
	sf::RenderTarget &m_target;
};
