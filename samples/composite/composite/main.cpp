#include "stdafx.h"
#include "Rectangle.h"
#include "GraphicCanvas.h"

int main()
{
	Rectangle rect;
	rect.SetFrame({ 10, 10, 100, 100 });

	rect.GetFillStyle().Enable(true);
	rect.GetFillStyle().SetColor(0x00FF007F);

	rect.GetLineStyle().Enable(true);
	rect.GetLineStyle().SetColor(0x0000FFFF);
	rect.GetLineStyle().SetThickness(2);

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
		rect.Draw(canvas);
		window.display();
	}

	return 0;
}
