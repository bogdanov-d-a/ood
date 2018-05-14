#include "stdafx.h"
#include "../libstreams/Utils.h"
#include "../libstreams/MemoryInputStream.h"
#include "../libstreams/MemoryOutputStream.h"
#include "../libstreams/InputStreamDecryptor.h"
#include "../libstreams/OutputStreamEncryptor.h"

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

BOOST_AUTO_TEST_CASE(OutputStreamEncryptorTest)
{
	auto memOutPtr = std::make_unique<MemoryOutputStream>();
	auto& memOutRef = *memOutPtr;

	IOutputDataStreamPtr outPtr = std::move(memOutPtr);
	outPtr = std::make_unique<OutputStreamEncryptor>(std::move(outPtr), 42);

	BOOST_CHECK(memOutRef.GetData().empty());

	{
		std::vector<uint8_t> data({ 0, 4, 2, 0, 1, 4 });
		outPtr->WriteBlock(data.data(), data.size());
	}

	BOOST_CHECK(memOutRef.GetData() == std::vector<uint8_t>({ 9, 72, 211, 9, 187, 72 }));
}

BOOST_AUTO_TEST_CASE(InputStreamDecryptorTest)
{
	IInputDataStreamPtr inPtr = std::make_unique<MemoryInputStream>(std::vector<uint8_t>({ 9, 72, 211, 9, 187, 72 }));
	inPtr = std::make_unique<InputStreamDecryptor>(std::move(inPtr), 42);

	std::vector<uint8_t> outData(6);
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()));
	BOOST_CHECK(inPtr->IsEOF());
	BOOST_CHECK(outData == std::vector<uint8_t>({ 0, 4, 2, 0, 1, 4 }));
}
