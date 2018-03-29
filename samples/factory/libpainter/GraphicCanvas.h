#pragma once

#include "ICanvas.h"
#include <SFML/Graphics.hpp>
#include "Color.h"

class CGraphicCanvas : public ICanvas
{
public:
	explicit CGraphicCanvas(sf::RenderWindow &window);

	void SetColor(Color color) final;
	void DrawLine(CPoint const& from, CPoint const& to) final;
	void DrawEllipse(Coordinate left, Coordinate top,
		Coordinate width, Coordinate height) final;

private:
	sf::RenderWindow &m_window;
	Color m_color = Color::BLACK;
};
