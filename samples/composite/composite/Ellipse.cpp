#include "stdafx.h"
#include "Ellipse.h"

void Ellipse::DrawImpl(ICanvas & canvas, RectD const& frame)
{
	canvas.DrawEllipse(frame.left, frame.top, frame.width, frame.height);
}
