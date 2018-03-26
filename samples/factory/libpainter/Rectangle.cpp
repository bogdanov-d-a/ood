#include "stdafx.h"
#include "Rectangle.h"
#include "ICanvas.h"

CRectangle::CRectangle(Color color, CPoint const& leftTop, CPoint const& rightBottom)
	: CShape(color)
	, m_leftTop(leftTop)
	, m_rightBottom(rightBottom)
{
}

void CRectangle::Draw(ICanvas & canvas) const
{
	CShape::Draw(canvas);
	canvas.DrawLine(m_leftTop, GetRightTop());
	canvas.DrawLine(GetRightTop(), m_rightBottom);
	canvas.DrawLine(m_rightBottom, GetLeftBottom());
	canvas.DrawLine(GetLeftBottom(), m_leftTop);
}

CPoint CRectangle::GetRightTop() const
{
	return CPoint(m_rightBottom.GetX(), m_leftTop.GetY());
}

CPoint CRectangle::GetLeftBottom() const
{
	return CPoint(m_leftTop.GetX(), m_rightBottom.GetY());
}
