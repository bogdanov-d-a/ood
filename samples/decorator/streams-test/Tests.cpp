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
