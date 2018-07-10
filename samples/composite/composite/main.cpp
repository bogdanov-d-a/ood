#include "stdafx.h"
#include "CompositeShape.h"
#include "Rectangle.h"
#include "Ellipse.h"
#include "Triangle.h"
#include "GraphicCanvas.h"

namespace
{

void SetFrame(IShape &shape, RectD const& frame)
{
	if (!shape.TrySetFrame(frame))
	{
		throw std::runtime_error("Could not set shape frame");
	}
}

}

int main()
{
	CompositeShape slide;

	{
		auto c = std::make_unique<CompositeShape>();

		{
			auto e = std::make_unique<Ellipse>();
			SetFrame(*e, { 60, 60, 100, 100 });

			e->GetFillStyle().SetColor(0xFF00003F);

			e->GetLineStyle().SetColor(0x00FFFFFF);
			e->GetLineStyle().SetThickness(2);

			c->AddShape(std::move(e));
		}

		{
			auto r = std::make_unique<Rectangle>();
			SetFrame(*r, { 10, 10, 100, 100 });

			r->GetFillStyle().SetColor(0x00FF007F);

			r->GetLineStyle().SetColor(0x0000FFFF);
			r->GetLineStyle().SetThickness(2);

			c->AddShape(std::move(r));
		}

		c->GetFillStyle().Enable(true);
		c->GetLineStyle().Enable(true);

		SetFrame(*c, { 100, 100, 300, 200 });

		slide.AddShape(std::move(c));
	}

	{
		auto t = std::make_unique<Triangle>();
		SetFrame(*t, { 500, 40, 150, 100 });

		t->GetFillStyle().Enable(true);
		t->GetFillStyle().SetColor(0xFFFF00FF);

		t->GetLineStyle().Enable(true);
		t->GetLineStyle().SetColor(0xFF00FF7F);
		t->GetLineStyle().SetThickness(5);

		slide.AddShape(std::move(t));
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
