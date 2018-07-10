#include "stdafx.h"
#include "Triangle.h"

void Triangle::DrawImpl(ICanvas & canvas, RectD const& frame)
{
	canvas.DrawPolygon({
		{ frame.left, frame.GetBottom() },
		{ (frame.left + frame.GetRight()) / 2, frame.top },
		{ frame.GetRight(), frame.GetBottom() },
	});
}
