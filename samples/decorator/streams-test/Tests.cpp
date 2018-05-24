#include "stdafx.h"
#include "../libstreams/ReplaceTable.h"
#include "../libstreams/MemoryInputStream.h"
#include "../libstreams/MemoryOutputStream.h"
#include "../libstreams/InputStreamDecryptor.h"
#include "../libstreams/OutputStreamEncryptor.h"
#include "../libstreams/OutputStreamCompressor.h"

BOOST_AUTO_TEST_CASE(ReplaceTableTest)
{
	ReplaceTable rt(42);

	for (uint8_t i = 0;; ++i)
	{
		BOOST_CHECK(i == rt.MapBack(rt.Map(i)));
		BOOST_CHECK(i == rt.Map(rt.MapBack(i)));

		if (i == UINT8_MAX)
		{
			break;
		}
	}
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

BOOST_AUTO_TEST_CASE(OutputStreamCompressorTest)
{
	auto memOutPtr = std::make_unique<MemoryOutputStream>();
	auto& memOutRef = *memOutPtr;

	auto outPtr = std::make_unique<OutputStreamCompressor>(std::move(memOutPtr));

	BOOST_CHECK(memOutRef.GetData().empty());

	{
		std::vector<uint8_t> data({ 0, 1, 2, 2, 2, 0, 3, 5, 3, 5, 5, 5 });
		outPtr->WriteBlock(data.data(), data.size());
		outPtr->SyncBuffer();
	}

	BOOST_CHECK(memOutRef.GetData() == std::vector<uint8_t>({ 0, 0, 0, 1, 2, 2, 0, 0, 0, 3, 0, 5, 0, 3, 2, 5 }));
}
