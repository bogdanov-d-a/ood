#pragma once

#include "IShape.h"

class LeafShape : public IShape
{
public:
	explicit LeafShape();

	RectD GetFrame() const final;
	void SetFrame(RectD const& frame) final;

	IFillStyle& GetFillStyle() final;
	ILineStyle& GetLineStyle() final;

	void Draw(ICanvas &canvas) final;
	virtual void DrawImpl(ICanvas &canvas) = 0;

private:
	std::unique_ptr<IFillStyle> m_fillStyle;
	std::unique_ptr<ILineStyle> m_lineStyle;
	RectD m_frame = { 0, 0, 0, 0 };
};
