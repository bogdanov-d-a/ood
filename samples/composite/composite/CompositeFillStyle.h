#pragma once

#include "IFillStyle.h"

class CompositeFillStyle : public IFillStyle
{
public:
	using Enumerator = std::function<void(std::function<bool(IFillStyle&)>)>;

	explicit CompositeFillStyle(Enumerator const& enumerator);

	boost::optional<bool> IsEnabled() const final;
	void Enable(bool enable) final;

	boost::optional<RGBAColor> GetColor() const final;
	void SetColor(RGBAColor color) final;

private:
	Enumerator m_enumerator;
};
