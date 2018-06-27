#include "stdafx.h"
#include "Triangle.h"

void Triangle::DrawImpl(ICanvas & canvas)
{
	const auto frame = GetFrame();
	canvas.DrawPolygon({
		{ frame.left, frame.GetBottom() },
		{ (frame.left + frame.GetRight()) / 2, frame.top },
		{ frame.GetRight(), frame.GetBottom() },
	});
}
