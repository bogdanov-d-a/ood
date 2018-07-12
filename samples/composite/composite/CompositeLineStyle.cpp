#include "stdafx.h"
#include "CompositeLineStyle.h"
#include "Utils.h"

CompositeLineStyle::CompositeLineStyle(Enumerator const & enumerator)
	: m_enumerator(enumerator)
{
}

boost::optional<bool> CompositeLineStyle::IsEnabled() const
{
	return Utils::GetCommonProperty<bool>([this](auto && function) {
		m_enumerator([&function](ILineStyle &lineStyle) {
			return function(lineStyle.IsEnabled());
		});
	});
}

void CompositeLineStyle::Enable(bool enable)
{
	m_enumerator([enable](ILineStyle &lineStyle) {
		lineStyle.Enable(enable);
		return true;
	});
}

boost::optional<RGBAColor> CompositeLineStyle::GetColor() const
{
	return Utils::GetCommonProperty<RGBAColor>([this](auto && function) {
		m_enumerator([&function](ILineStyle &lineStyle) {
			return function(lineStyle.GetColor());
		});
	});
}

void CompositeLineStyle::SetColor(RGBAColor color)
{
	m_enumerator([color](ILineStyle &lineStyle) {
		lineStyle.SetColor(color);
		return true;
	});
}

boost::optional<double> CompositeLineStyle::GetThickness() const
{
	return Utils::GetCommonProperty<double>([this](auto && function) {
		m_enumerator([&function](ILineStyle &lineStyle) {
			return function(lineStyle.GetThickness());
		});
	});
}

void CompositeLineStyle::SetThickness(double thickness)
{
	m_enumerator([thickness](ILineStyle &lineStyle) {
		lineStyle.SetThickness(thickness);
		return true;
	});
}
