// Painter.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "../libpainter/Designer.h"
#include "../libpainter/ShapeFactory.h"
#include "../libpainter/PictureDraft.h"
#include "../libpainter/Painter.h"
#include "../libpainter/GraphicCanvas.h"

using namespace std;

int main()
{
	unique_ptr<IShapeFactory> factory = make_unique<CShapeFactory>();
	unique_ptr<IDesigner> designer = make_unique<CDesigner>(*factory);
	auto draft = designer->CreateDraft(cin);
	unique_ptr<IPainter> painter = make_unique<CPainter>();

	sf::RenderWindow window(sf::VideoMode(800, 480), "SFML works!");
	unique_ptr<ICanvas> canvas = make_unique<CGraphicCanvas>(window);

	while (window.isOpen())
	{
		sf::Event event;
		while (window.pollEvent(event))
		{
			if (event.type == sf::Event::Closed)
				window.close();
		}

		window.clear();
		painter->DrawPicture(draft, *canvas);
		window.display();
	}

	return 0;
}
