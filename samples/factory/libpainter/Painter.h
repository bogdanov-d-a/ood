#pragma once

#include "IPainter.h"

class CPainter : public IPainter
{
public:
	void DrawPicture(CPictureDraft const& draft, ICanvas &canvas) final;
};
