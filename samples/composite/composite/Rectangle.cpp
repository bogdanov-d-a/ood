#include "stdafx.h"
#include "Rectangle.h"

void Rectangle::DrawImpl(ICanvas & canvas, RectD const& frame)
{
	canvas.DrawPolygon({
		{ frame.left, frame.top },
		{ frame.GetRight(), frame.top },
		{ frame.GetRight(), frame.GetBottom() },
		{ frame.left, frame.GetBottom() },
	});
}
