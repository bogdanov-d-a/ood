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

auto CreateRectangle(CUtils::ParseShapeResult const& shape)
{
	const auto& ints = shape.ints;
	ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
	return std::make_unique<CRectangle>(shape.color, CPoint(ints[0], ints[1]), CPoint(ints[2], ints[3]));
}

auto CreateTriangle(CUtils::ParseShapeResult const& shape)
{
	const auto& ints = shape.ints;
	ThrowOnWrongArgumentCount(shape.type, 6, ints.size());
	return std::make_unique<CTriangle>(shape.color, CPoint(ints[0], ints[1]), CPoint(ints[2], ints[3]), CPoint(ints[4], ints[5]));
}

auto CreateEllipse(CUtils::ParseShapeResult const& shape)
{
	const auto& ints = shape.ints;
	ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
	return std::make_unique<CEllipse>(shape.color, CPoint(ints[0], ints[1]), ints[2], ints[3]);
}

auto CreatePolygon(CUtils::ParseShapeResult const& shape)
{
	const auto& ints = shape.ints;
	ThrowOnWrongArgumentCount(shape.type, 4, ints.size());
	return std::make_unique<CRegularPolygon>(shape.color, ints[0], CPoint(ints[1], ints[2]), ints[3]);
}

const std::map<std::string, std::function<std::unique_ptr<CShape>(CUtils::ParseShapeResult const&)>> NAME_TO_FUNCTION_MAP = {
	{ "rectangle", CreateRectangle },
	{ "triangle", CreateTriangle },
	{ "ellipse", CreateEllipse },
	{ "polygon", CreatePolygon },
};

}

std::unique_ptr<CShape> CShapeFactory::CreateShape(const std::string & description)
{
	const auto shape = CUtils::ParseShape(description);
	const auto it = NAME_TO_FUNCTION_MAP.find(shape.type);

	if (it == NAME_TO_FUNCTION_MAP.end())
	{
		throw std::runtime_error("Invalid shape name: " + shape.type);
	}

	return it->second(shape);
}
