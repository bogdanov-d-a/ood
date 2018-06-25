#pragma once
#include "CommonTypes.h"

class ICanvas
{
public:
	virtual void SetFillColor(RGBAColor color) = 0;
	virtual void ResetFillColor() = 0;

	virtual void SetLineColor(RGBAColor color) = 0;
	virtual void ResetLineColor() = 0;

	virtual void SetLineThickness(double thickness) = 0;
	virtual void ResetLineThickness() = 0;

	virtual void DrawPolygon(std::vector<PointD> const& points) = 0;
	virtual void DrawEllipse(double left, double top, double width, double height) = 0;

	virtual ~ICanvas() = 0;
};
