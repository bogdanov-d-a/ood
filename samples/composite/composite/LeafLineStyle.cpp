#include "stdafx.h"
#include "LeafLineStyle.h"

bool LeafLineStyle::IsEnabled() const
{
	return m_enabled;
}

void LeafLineStyle::Enable(bool enable)
{
	m_enabled = enable;
}

RGBAColor LeafLineStyle::GetColor() const
{
	return m_color;
}

void LeafLineStyle::SetColor(RGBAColor color)
{
	m_color = color;
}

double LeafLineStyle::GetThickness() const
{
	return m_thickness;
}

void LeafLineStyle::SetThickness(double thickness)
{
	m_thickness = thickness;
}
