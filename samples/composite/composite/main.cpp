#include "stdafx.h"
#include "Ellipse.h"
#include "GraphicCanvas.h"

int main()
{
	Ellipse ellipse;
	ellipse.SetFrame({ 10, 10, 100, 200 });

	ellipse.GetFillStyle().Enable(true);
	ellipse.GetFillStyle().SetColor(0x00FF007F);

	ellipse.GetLineStyle().Enable(true);
	ellipse.GetLineStyle().SetColor(0x0000FFFF);
	ellipse.GetLineStyle().SetThickness(2);

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
		ellipse.Draw(canvas);
		window.display();
	}

	return 0;
}
