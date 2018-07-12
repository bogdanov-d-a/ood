#pragma once

#include "IShape.h"

class LeafShape : public IShape
{
public:
	explicit LeafShape();

	boost::optional<RectD> GetFrame() const final;
	bool TrySetFrame(RectD const& frame) final;

	IFillStyle& GetFillStyle() final;
	IFillStyle const& GetFillStyle() const final;

	ILineStyle& GetLineStyle() final;
	ILineStyle const& GetLineStyle() const final;

	void Draw(ICanvas &canvas) final;

private:
	virtual void DrawImpl(ICanvas &canvas, RectD const& frame) = 0;

	std::unique_ptr<IFillStyle> m_fillStyle;
	std::unique_ptr<ILineStyle> m_lineStyle;
	RectD m_frame = { 0, 0, 0, 0 };
};
