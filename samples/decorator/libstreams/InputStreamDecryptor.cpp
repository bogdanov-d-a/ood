#include "deps.h"
#include "InputStreamDecryptor.h"

InputStreamDecryptor::InputStreamDecryptor(IInputDataStreamPtr && stream, uint32_t key)
	: m_stream(std::move(stream))
	, m_table(key)
{
}

bool InputStreamDecryptor::IsEOF()
{
	return m_stream->IsEOF();
}

uint8_t InputStreamDecryptor::ReadByte()
{
	return m_table.MapBack(m_stream->ReadByte());
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
