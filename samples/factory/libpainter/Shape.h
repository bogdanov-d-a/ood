#pragma once

#include "Color.h"

struct ICanvas;

class CShape
{
public:
	explicit CShape(Color color);
	virtual ~CShape();

	virtual void Draw(ICanvas &canvas) const;
	Color GetColor() const;

private:
	Color m_color;
};
