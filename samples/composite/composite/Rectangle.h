#pragma once

#include "LeafShape.h"

class Rectangle : public LeafShape
{
public:
	void DrawImpl(ICanvas &canvas, RectD const& frame) final;
};
