#pragma once

#include "CommonTypes.h"
#include "IFillStyle.h"
#include "ILineStyle.h"
#include "IDrawable.h"

class IShape : public IDrawable
{
public:
	virtual ~IShape() = default;

	virtual RectD GetFrame() const = 0;
	virtual void SetFrame(RectD const& frame) = 0;

	virtual IFillStyle& GetFillStyle() = 0;
	virtual IFillStyle const& GetFillStyle() const = 0;

	virtual ILineStyle& GetLineStyle() = 0;
	virtual ILineStyle const& GetLineStyle() const = 0;
};

using IShapePtr = std::unique_ptr<IShape>;
