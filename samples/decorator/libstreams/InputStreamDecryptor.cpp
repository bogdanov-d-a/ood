#include "deps.h"
#include "InputStreamDecryptor.h"
#include "Utils.h"

InputStreamDecryptor::InputStreamDecryptor(IInputDataStreamPtr && stream, uint32_t key)
	: m_stream(std::move(stream))
	, m_table(Utils::InvertReplaceTable(Utils::GenerateReplaceTable(key)))
{
}

bool InputStreamDecryptor::IsEOF()
{
	return m_stream->IsEOF();
}

uint8_t InputStreamDecryptor::ReadByte()
{
	return m_table[m_stream->ReadByte()];
}

std::streamsize InputStreamDecryptor::ReadBlock(void * dstBuffer, std::streamsize size)
{
	decltype(auto) dstBufferCast = reinterpret_cast<uint8_t*>(dstBuffer);

	std::streamsize readCount = 0;
	for (; readCount < size && !IsEOF(); ++readCount)
	{
		*dstBufferCast = ReadByte();
		++dstBufferCast;
	}
	return readCount;
}
