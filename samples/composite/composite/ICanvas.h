#pragma once
#include "CommonTypes.h"

class ICanvas
{
public:
	virtual void SetFillColor(RGBAColor color) = 0;
	virtual void ResetFillColor() = 0;

	virtual void SetLineStyle(RGBAColor color, double thickness) = 0;
	virtual void ResetLineStyle() = 0;

	virtual void DrawPolygon(std::vector<PointD> const& points) = 0;
	virtual void DrawEllipse(double left, double top, double width, double height) = 0;

	virtual ~ICanvas() = default;
};
