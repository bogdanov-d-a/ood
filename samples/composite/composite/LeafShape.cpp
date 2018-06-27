#include "stdafx.h"
#include "LeafShape.h"
#include "LeafFillStyle.h"
#include "LeafLineStyle.h"

LeafShape::LeafShape()
	: m_fillStyle(std::make_unique<LeafFillStyle>())
	, m_lineStyle(std::make_unique<LeafLineStyle>())
{
}

RectD LeafShape::GetFrame() const
{
	return m_frame;
}

void LeafShape::SetFrame(RectD const & frame)
{
	m_frame = frame;
}

IFillStyle & LeafShape::GetFillStyle()
{
	return *m_fillStyle;
}

ILineStyle & LeafShape::GetLineStyle()
{
	return *m_lineStyle;
}

void LeafShape::Draw(ICanvas & canvas)
{
	{
		auto &fillStyle = GetFillStyle();

		if (fillStyle.IsEnabled())
		{
			canvas.SetFillColor(*fillStyle.GetColor());
		}
		else
		{
			canvas.ResetFillColor();
		}
	}

	{
		auto &lineStyle = GetLineStyle();

		if (lineStyle.IsEnabled())
		{
			canvas.SetLineStyle(*lineStyle.GetColor(), *lineStyle.GetThickness());
		}
		else
		{
			canvas.ResetLineStyle();
		}
	}

	DrawImpl(canvas);
}