#include "stdafx.h"
#include "Painter.h"
#include "PictureDraft.h"
#include "ICanvas.h"

void CPainter::DrawPicture(CPictureDraft const & draft, ICanvas & canvas)
{
	for (auto const& shape : draft)
	{
		shape.Draw(canvas);
	}
}
