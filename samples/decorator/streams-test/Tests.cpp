#include "stdafx.h"
#include "../libstreams/ReplaceTable.h"
#include "../libstreams/MemoryInputStream.h"
#include "../libstreams/MemoryOutputStream.h"
#include "../libstreams/InputStreamDecryptor.h"
#include "../libstreams/InputStreamDecompressor.h"
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
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()) == 6);
	BOOST_CHECK(inPtr->IsEOF());
	BOOST_CHECK(outData == std::vector<uint8_t>({ 0, 4, 2, 0, 1, 4 }));
}

BOOST_AUTO_TEST_CASE(OutputStreamCompressorEmptyTest)
{
	auto memOutPtr = std::make_unique<MemoryOutputStream>();
	auto& memOutRef = *memOutPtr;

	auto outPtr = std::make_unique<OutputStreamCompressor>(std::move(memOutPtr));

	BOOST_CHECK(memOutRef.GetData().empty());
	outPtr->SyncBuffer();
	BOOST_CHECK(memOutRef.GetData().empty());
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

BOOST_AUTO_TEST_CASE(OutputStreamCompressor256Test)
{
	auto memOutPtr = std::make_unique<MemoryOutputStream>();
	auto& memOutRef = *memOutPtr;

	auto outPtr = std::make_unique<OutputStreamCompressor>(std::move(memOutPtr));

	BOOST_CHECK(memOutRef.GetData().empty());

	{
		std::vector<uint8_t> data(256, 42);
		outPtr->WriteBlock(data.data(), data.size());
		outPtr->SyncBuffer();
	}

	BOOST_CHECK(memOutRef.GetData() == std::vector<uint8_t>({ 255, 42 }));
}

BOOST_AUTO_TEST_CASE(OutputStreamCompressor257Test)
{
	auto memOutPtr = std::make_unique<MemoryOutputStream>();
	auto& memOutRef = *memOutPtr;

	auto outPtr = std::make_unique<OutputStreamCompressor>(std::move(memOutPtr));

	BOOST_CHECK(memOutRef.GetData().empty());

	{
		std::vector<uint8_t> data(257, 42);
		outPtr->WriteBlock(data.data(), data.size());
		outPtr->SyncBuffer();
	}

	BOOST_CHECK(memOutRef.GetData() == std::vector<uint8_t>({ 255, 42, 0, 42 }));
}

BOOST_AUTO_TEST_CASE(InputStreamDecompressorEmptyTest)
{
	IInputDataStreamPtr inPtr = std::make_unique<MemoryInputStream>(std::vector<uint8_t>());
	inPtr = std::make_unique<InputStreamDecompressor>(std::move(inPtr));

	std::vector<uint8_t> outData(10);
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()) == 0);
	BOOST_CHECK(inPtr->IsEOF());
}

BOOST_AUTO_TEST_CASE(InputStreamDecompressorTest)
{
	IInputDataStreamPtr inPtr = std::make_unique<MemoryInputStream>(std::vector<uint8_t>({ 0, 0, 0, 1, 2, 2, 0, 0, 0, 3, 0, 5, 0, 3, 2, 5 }));
	inPtr = std::make_unique<InputStreamDecompressor>(std::move(inPtr));

	std::vector<uint8_t> outData(12);
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()) == 12);
	BOOST_CHECK(inPtr->IsEOF());
	BOOST_CHECK(outData == std::vector<uint8_t>({ 0, 1, 2, 2, 2, 0, 3, 5, 3, 5, 5, 5 }));
}

BOOST_AUTO_TEST_CASE(InputStreamDecompressor256Test)
{
	IInputDataStreamPtr inPtr = std::make_unique<MemoryInputStream>(std::vector<uint8_t>({ 255, 13 }));
	inPtr = std::make_unique<InputStreamDecompressor>(std::move(inPtr));

	std::vector<uint8_t> outData(256);
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()) == 256);
	BOOST_CHECK(inPtr->IsEOF());
	BOOST_CHECK(outData == std::vector<uint8_t>(256, 13));
}

BOOST_AUTO_TEST_CASE(InputStreamDecompressor257Test)
{
	IInputDataStreamPtr inPtr = std::make_unique<MemoryInputStream>(std::vector<uint8_t>({ 255, 13, 0, 13 }));
	inPtr = std::make_unique<InputStreamDecompressor>(std::move(inPtr));

	std::vector<uint8_t> outData(257);
	BOOST_CHECK(inPtr->ReadBlock(outData.data(), outData.size()) == 257);
	BOOST_CHECK(inPtr->IsEOF());
	BOOST_CHECK(outData == std::vector<uint8_t>(257, 13));
}
