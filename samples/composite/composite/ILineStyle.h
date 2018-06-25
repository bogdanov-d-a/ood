#pragma once

#include "CommonTypes.h"

class ILineStyle
{
public:
	virtual ~ILineStyle() = default;

	virtual bool IsEnabled() const = 0;
	virtual void Enable(bool enable) = 0;

	virtual RGBAColor GetColor() const = 0;
	virtual void SetColor(RGBAColor color) = 0;

	virtual double GetThickness() const = 0;
	virtual void SetThickness(double thickness) = 0;
};
