#pragma once

#include "IShape.h"

class CompositeShape : public IShape
{
public:
	explicit CompositeShape();

	void AddShape(IShapePtr && shape);

	RectD GetFrame() const final;
	void SetFrame(RectD const& frame) final;

	IFillStyle& GetFillStyle() final;
	IFillStyle const& GetFillStyle() const final;

	ILineStyle& GetLineStyle() final;
	ILineStyle const& GetLineStyle() const final;

	void Draw(ICanvas &canvas) final;

private:
	std::forward_list<IShapePtr> m_shapes;
	std::unique_ptr<IFillStyle> m_fillStyle;
	std::unique_ptr<ILineStyle> m_lineStyle;
};
