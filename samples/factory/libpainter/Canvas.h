#pragma once

#include "ICanvas.h"

class CCanvas : public ICanvas
{
public:
	explicit CCanvas(std::ostream &out);

	void SetColor(Color color) final;
	void DrawLine(CPoint const& from, CPoint const& to) final;
	void DrawEllipse(CUtils::Coordinate left, CUtils::Coordinate top,
		CUtils::Coordinate width, CUtils::Coordinate height) final;

private:
	std::ostream &m_out;
};
