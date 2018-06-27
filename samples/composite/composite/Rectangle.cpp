#include "stdafx.h"
#include "Rectangle.h"

void Rectangle::DrawImpl(ICanvas & canvas)
{
	const auto frame = GetFrame();
	canvas.DrawPolygon({
		{ frame.left, frame.top },
		{ frame.GetRight(), frame.top },
		{ frame.GetRight(), frame.GetBottom() },
		{ frame.left, frame.GetBottom() },
	});
}
