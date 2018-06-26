#pragma once

#include "ILineStyle.h"

class CompositeLineStyle : public ILineStyle
{
public:
	using Enumerator = std::function<void(std::function<bool(ILineStyle&)>)>;

	explicit CompositeLineStyle(Enumerator const& enumerator);

	boost::optional<bool> IsEnabled() const final;
	void Enable(bool enable) final;

	boost::optional<RGBAColor> GetColor() const final;
	void SetColor(RGBAColor color) final;

	boost::optional<double> GetThickness() const final;
	void SetThickness(double thickness) final;

private:
	Enumerator m_enumerator;
};
