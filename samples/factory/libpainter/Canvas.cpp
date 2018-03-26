#include "stdafx.h"
#include "Canvas.h"

CCanvas::CCanvas(std::ostream & out)
	: m_out(out)
{
}

void CCanvas::SetColor(Color color)
{
	m_out << "SetColor(" << CUtils::ToString(color) << ")" << std::endl;
}

void CCanvas::DrawLine(CPoint const & from, CPoint const & to)
{
	m_out << "DrawLine(" << from.ToString() << "," << to.ToString() << ")" << std::endl;
}

void CCanvas::DrawEllipse(CUtils::Coordinate left, CUtils::Coordinate top, CUtils::Coordinate width, CUtils::Coordinate height)
{
	m_out << "DrawEllipse(" << CUtils::ToString(left) << "," << CUtils::ToString(top)
		<< "," << CUtils::ToString(width) << "," << CUtils::ToString(height) << ")" << std::endl;
}
