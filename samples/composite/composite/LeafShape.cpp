#include "stdafx.h"
#include "LeafShape.h"
#include "LeafFillStyle.h"
#include "LeafLineStyle.h"

LeafShape::LeafShape()
	: m_fillStyle(std::make_unique<LeafFillStyle>())
	, m_lineStyle(std::make_unique<LeafLineStyle>())
{
}

boost::optional<RectD> LeafShape::GetFrame() const
{
	return m_frame;
}

bool LeafShape::TrySetFrame(RectD const & frame)
{
	m_frame = frame;
	return true;
}

IFillStyle & LeafShape::GetFillStyle()
{
	return *m_fillStyle;
}

IFillStyle const & LeafShape::GetFillStyle() const
{
	return *m_fillStyle;
}

ILineStyle & LeafShape::GetLineStyle()
{
	return *m_lineStyle;
}

ILineStyle const & LeafShape::GetLineStyle() const
{
	return *m_lineStyle;
}

void LeafShape::Draw(ICanvas & canvas)
{
	const auto frame = GetFrame();
	if (!frame)
	{
		return;
	}

	{
		auto &fillStyle = GetFillStyle();

		auto color = fillStyle.GetColor();
		if (fillStyle.IsEnabled().get_value_or(false) && color)
		{
			canvas.SetFillColor(*color);
		}
		else
		{
			canvas.ResetFillColor();
		}
	}

	{
		auto &lineStyle = GetLineStyle();

		auto color = lineStyle.GetColor();
		auto thickness = lineStyle.GetThickness();
		if (lineStyle.IsEnabled().get_value_or(false) && color && thickness)
		{
			canvas.SetLineStyle(*color, *thickness);
		}
		else
		{
			canvas.ResetLineStyle();
		}
	}

	DrawImpl(canvas, *frame);
}
