#pragma once

#include "CommonTypes.h"
#include "IFillStyle.h"
#include "ILineStyle.h"

class IShape
{
public:
	virtual ~IShape() = default;

	virtual RectD GetFrame() const = 0;
	virtual void SetFrame(RectD const& frame) = 0;

	virtual IFillStyle& GetFillStyle() = 0;
	virtual ILineStyle& GetLineStyle() = 0;
};
