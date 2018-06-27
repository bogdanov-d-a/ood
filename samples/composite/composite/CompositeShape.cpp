#include "stdafx.h"
#include "CompositeShape.h"
#include "CompositeFillStyle.h"
#include "CompositeLineStyle.h"

CompositeShape::CompositeShape()
	: m_fillStyle(std::make_unique<CompositeFillStyle>([this](std::function<bool(IFillStyle&)> function) {
		for (auto &shape : m_shapes)
		{
			if (!function(shape->GetFillStyle()))
			{
				break;
			}
		}
	}))
	, m_lineStyle(std::make_unique<CompositeLineStyle>([this](std::function<bool(ILineStyle&)> function) {
		for (auto &shape : m_shapes)
		{
			if (!function(shape->GetLineStyle()))
			{
				break;
			}
		}
	}))
{
}

RectD CompositeShape::GetFrame() const
{
	boost::optional<RectD> result;

	for (auto &shape : m_shapes)
	{
		auto frame = shape->GetFrame();

		if (!result)
		{
			result = frame;
			continue;
		}

		result->left = std::min(result->left, frame.left);
		result->top = std::min(result->top, frame.top);
		result->SetRight(std::max(result->GetRight(), frame.GetRight()));
		result->SetBottom(std::max(result->GetBottom(), frame.GetBottom()));
	}

	if (!result)
	{
		throw std::runtime_error("can't get frame for empty composite shape");
	}
	return *result;
}

void CompositeShape::SetFrame(RectD const & frame)
{
	const auto oldFrame = GetFrame();

	const auto moveX = frame.left - oldFrame.left;
	const auto moveY = frame.top - oldFrame.top;

	const auto scaleX = frame.width / oldFrame.width;
	const auto scaleY = frame.height / oldFrame.height;

	for (auto &shape : m_shapes)
	{
		auto curFrame = shape->GetFrame();

		const auto scaleLeftDiff = (curFrame.left - oldFrame.left) * (scaleX - 1);
		const auto scaleTopDiff = (curFrame.top - oldFrame.top) * (scaleY - 1);

		curFrame.left += moveX + scaleLeftDiff;
		curFrame.top += moveY + scaleTopDiff;
		curFrame.width *= scaleX;
		curFrame.height *= scaleY;

		shape->SetFrame(curFrame);
	}
}

IFillStyle & CompositeShape::GetFillStyle()
{
	return *m_fillStyle;
}

ILineStyle & CompositeShape::GetLineStyle()
{
	return *m_lineStyle;
}

void CompositeShape::Draw(ICanvas & canvas)
{
	for (auto &shape : m_shapes)
	{
		shape->Draw(canvas);
	}
}
