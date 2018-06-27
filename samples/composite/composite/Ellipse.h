#pragma once

#include "LeafShape.h"

class Ellipse : public LeafShape
{
public:
	void DrawImpl(ICanvas &canvas) final;
};
