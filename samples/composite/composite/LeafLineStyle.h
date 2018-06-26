#pragma once

#include "ILineStyle.h"

class LeafLineStyle : public ILineStyle
{
public:
	boost::optional<bool> IsEnabled() const final;
	void Enable(bool enable) final;

	boost::optional<RGBAColor> GetColor() const final;
	void SetColor(RGBAColor color) final;

	boost::optional<double> GetThickness() const final;
	void SetThickness(double thickness) final;

private:
	bool m_enabled = false;
	RGBAColor m_color = 0x0;
	double m_thickness = 0;
};
