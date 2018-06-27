#include "stdafx.h"
#include "Triangle.h"
#include "GraphicCanvas.h"

int main()
{
	Triangle triangle;
	triangle.SetFrame({ 10, 10, 100, 100 });

	triangle.GetFillStyle().Enable(true);
	triangle.GetFillStyle().SetColor(0x00FF007F);

	triangle.GetLineStyle().Enable(true);
	triangle.GetLineStyle().SetColor(0x0000FFFF);
	triangle.GetLineStyle().SetThickness(2);

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
		triangle.Draw(canvas);
		window.display();
	}

	return 0;
}
