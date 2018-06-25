#pragma once

#include "IFillStyle.h"

class LeafFillStyle : public IFillStyle
{
public:
	bool IsEnabled() const final;
	void Enable(bool enable) final;

	RGBAColor GetColor() const final;
	void SetColor(RGBAColor color) final;

private:
	bool m_enabled = false;
	RGBAColor m_color = 0x0;
};
