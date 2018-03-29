#pragma once

#include "Color.h"
#include "Point.h"

struct ICanvas
{
	virtual void SetColor(Color color) = 0;
	virtual void DrawLine(CPoint const& from, CPoint const& to) = 0;
	virtual void DrawEllipse(Coordinate left, Coordinate top,
		Coordinate width, Coordinate height) = 0;
};
