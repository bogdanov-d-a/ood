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
