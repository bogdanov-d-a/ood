#include "stdafx.h"
#include "LeafFillStyle.h"

boost::optional<bool> LeafFillStyle::IsEnabled() const
{
	return m_enabled;
}

void LeafFillStyle::Enable(bool enable)
{
	m_enabled = enable;
}

boost::optional<RGBAColor> LeafFillStyle::GetColor() const
{
	return m_color;
}

void LeafFillStyle::SetColor(RGBAColor color)
{
	m_color = color;
}
