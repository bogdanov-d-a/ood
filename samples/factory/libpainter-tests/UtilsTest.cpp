#include "stdafx.h"
#include "../libpainter/Utils.h"

using namespace std;

namespace
{

bool TestSplitWordsWrapper(string const& s, vector<string> const& result)
{
	return CUtils::SplitWords(s) == result;
}

bool TestParseShapeWrapper(string const& s, string const& resultType, Color resultColor, vector<int> const& resultInts)
{
	const auto result = CUtils::ParseShape(s);
	return result.type == resultType && result.color == resultColor && result.ints == resultInts;
}

}

BOOST_AUTO_TEST_SUITE(TestSplitWords)

BOOST_AUTO_TEST_CASE(minimal)
{
	BOOST_CHECK(TestSplitWordsWrapper("hello world", { "hello", "world" }));
}

BOOST_AUTO_TEST_CASE(parse_rect)
{
	BOOST_CHECK(TestSplitWordsWrapper("rectangle red 10 20 100 200", { "rectangle", "red", "10", "20", "100", "200" }));
}

BOOST_AUTO_TEST_SUITE_END()


BOOST_AUTO_TEST_SUITE(TestParseShape)

BOOST_AUTO_TEST_CASE(parse_rect)
{
	TestParseShapeWrapper("rectangle green 10 20 100 200", "rectangle", Color::GREEN, { 10, 20, 100, 200 });
}

BOOST_AUTO_TEST_SUITE_END()
