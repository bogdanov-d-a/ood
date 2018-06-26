#include "stdafx.h"
#include "CompositeFillStyle.h"
#include "Utils.h"

CompositeFillStyle::CompositeFillStyle(Enumerator const & enumerator)
	: m_enumerator(enumerator)
{
}

boost::optional<bool> CompositeFillStyle::IsEnabled() const
{
	return Utils::GetCommonProperty<bool>([this](std::function<bool(boost::optional<bool>)> const& function) {
		m_enumerator([&function](IFillStyle &fillStyle) {
			return function(fillStyle.IsEnabled());
		});
	});
}

void CompositeFillStyle::Enable(bool enable)
{
	m_enumerator([enable](IFillStyle &fillStyle) {
		fillStyle.Enable(enable);
		return true;
	});
}

boost::optional<RGBAColor> CompositeFillStyle::GetColor() const
{
	return Utils::GetCommonProperty<RGBAColor>([this](std::function<bool(boost::optional<RGBAColor>)> const& function) {
		m_enumerator([&function](IFillStyle &fillStyle) {
			return function(fillStyle.GetColor());
		});
	});
}

void CompositeFillStyle::SetColor(RGBAColor color)
{
	m_enumerator([color](IFillStyle &fillStyle) {
		fillStyle.SetColor(color);
		return true;
	});
}
