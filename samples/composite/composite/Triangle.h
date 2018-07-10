#pragma once

#include "LeafShape.h"

class Triangle : public LeafShape
{
public:
	void DrawImpl(ICanvas &canvas, RectD const& frame) final;
};
