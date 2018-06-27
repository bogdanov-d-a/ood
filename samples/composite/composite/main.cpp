#include "stdafx.h"
#include "CompositeShape.h"
#include "Rectangle.h"
#include "Ellipse.h"
#include "GraphicCanvas.h"

int main()
{
	CompositeShape slide;

	{
		auto e = std::make_unique<Ellipse>();
		e->SetFrame({ 60, 60, 100, 100 });

		e->GetFillStyle().SetColor(0xFF00003F);

		e->GetLineStyle().SetColor(0x00FFFFFF);
		e->GetLineStyle().SetThickness(2);

		slide.AddShape(std::move(e));
	}

	{
		auto r = std::make_unique<Rectangle>();
		r->SetFrame({ 10, 10, 100, 100 });

		r->GetFillStyle().SetColor(0x00FF007F);

		r->GetLineStyle().SetColor(0x0000FFFF);
		r->GetLineStyle().SetThickness(2);

		slide.AddShape(std::move(r));
	}

	slide.GetFillStyle().Enable(true);
	slide.GetLineStyle().Enable(true);

	slide.SetFrame({ 100, 100, 300, 200 });

	sf::RenderWindow window(sf::VideoMode(800, 480), "SFML works!");
	GraphicCanvas canvas(window);

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		window.clear();
		slide.Draw(canvas);
		window.display();
	}

	return 0;
}
