#pragma once

#include "ICanvas.h"

class CCanvas : public ICanvas
{
public:
	explicit CCanvas(std::ostream &out);

	void SetColor(Color color) final;
	void DrawLine(CPoint const& from, CPoint const& to) final;
	void DrawEllipse(Coordinate left, Coordinate top,
		Coordinate width, Coordinate height) final;

private:
	std::ostream &m_out;
};
