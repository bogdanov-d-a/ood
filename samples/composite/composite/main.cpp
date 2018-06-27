#include "stdafx.h"
#include "CompositeShape.h"
#include "Rectangle.h"
#include "GraphicCanvas.h"

int main()
{
	CompositeShape slide;

	{
		auto r = std::make_unique<Rectangle>();
		r->SetFrame({ 10, 10, 100, 100 });

		r->GetFillStyle().Enable(true);
		r->GetFillStyle().SetColor(0x00FF007F);

		r->GetLineStyle().Enable(true);
		r->GetLineStyle().SetColor(0x0000FFFF);
		r->GetLineStyle().SetThickness(2);

		slide.AddShape(std::move(r));
	}

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
