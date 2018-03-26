#pragma once

class CPictureDraft;
struct ICanvas;

class IPainter
{
public:
	virtual ~IPainter() = default;
	virtual void DrawPicture(CPictureDraft const& draft, ICanvas &canvas) = 0;
};
