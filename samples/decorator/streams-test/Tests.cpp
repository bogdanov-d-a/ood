#include "stdafx.h"
#include "../libstreams/Utils.h"

BOOST_AUTO_TEST_CASE(GenerateReplaceTableSameTest)
{
	auto table1 = Utils::GenerateReplaceTable(42);
	auto table2 = Utils::GenerateReplaceTable(42);
	BOOST_CHECK(table1 == table2);
}

BOOST_AUTO_TEST_CASE(GenerateReplaceTableDifferentTest)
{
	auto table1 = Utils::GenerateReplaceTable(42);
	auto table2 = Utils::GenerateReplaceTable(666);
	BOOST_CHECK(table1 != table2);
}

BOOST_AUTO_TEST_CASE(InvertReplaceTableTest)
{
	const auto test = [](std::vector<uint8_t> const& in, std::vector<uint8_t> const& out) {
		return Utils::InvertReplaceTable(in) == out;
	};

	BOOST_CHECK(test({}, {}));
	BOOST_CHECK(test({ 2, 0, 1 }, { 1, 2, 0 }));
}

BOOST_AUTO_TEST_CASE(InvertReplaceTableDoubleTest)
{
	const auto doubleTest = [](std::vector<uint8_t> const& data) {
		return Utils::InvertReplaceTable(Utils::InvertReplaceTable(data)) == data;
	};

	BOOST_CHECK(doubleTest({}));
	BOOST_CHECK(doubleTest({ 0, 1, 2, 3, 4 }));
	BOOST_CHECK(doubleTest({ 0 }));
	BOOST_CHECK(doubleTest({ 2, 4, 3, 1, 0 }));
}
