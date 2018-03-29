#include "stdafx.h"
#include "ShapeFactory.h"
#include "Shape.h"
#include "Rectangle.h"
#include "Triangle.h"
#include "Ellipse.h"
#include "RegularPolygon.h"
#include "Utils.h"

namespace
{

void ThrowOnWrongArgumentCount(std::string const& name, size_t expected, size_t actual)
{
	if (expected != actual)
	{
		throw std::runtime_error("Invalid argument count for " + name +
			": expected " + std::to_string(expected) + ", got " + std::to_string(actual));
	}
}

}

std::unique_ptr<CShape> CShapeFactory::CreateShape(const std::string & description)
{
	const auto shape = CUtils::ParseShape(description);
	const auto& ints = shape.ints;

	if (shape.type == "rectangle")
	{
		ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
		return std::make_unique<CRectangle>(shape.color, CPoint(ints[0], ints[1]), CPoint(ints[2], ints[3]));
	}
	else if (shape.type == "triangle")
	{
		ThrowOnWrongArgumentCount(shape.type, 6, ints.size());
		return std::make_unique<CTriangle>(shape.color, CPoint(ints[0], ints[1]), CPoint(ints[2], ints[3]), CPoint(ints[4], ints[5]));
	}
	else if (shape.type == "ellipse")
	{
		ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
		return std::make_unique<CEllipse>(shape.color, CPoint(ints[0], ints[1]), ints[2], ints[3]);
	}
	else if (shape.type == "polygon")
	{
		ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
		return std::make_unique<CRegularPolygon>(shape.color, ints[0], CPoint(ints[1], ints[2]), ints[3]);
	}
	else
	{
		throw std::runtime_error("Invalid shape name: " + shape.type);
	}
}
