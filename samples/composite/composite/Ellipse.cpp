#include "stdafx.h"
#include "Ellipse.h"

void Ellipse::DrawImpl(ICanvas & canvas)
{
	const auto frame = GetFrame();
	canvas.DrawEllipse(frame.left, frame.top, frame.width, frame.height);
}
