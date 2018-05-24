#include "deps.h"
#include "InputStreamDecompressor.h"

InputStreamDecompressor::InputStreamDecompressor(IInputDataStreamPtr && stream)
	: m_stream(std::move(stream))
{
	TryReadBuffer();
}

bool InputStreamDecompressor::IsEOF()
{
	return m_buffer == boost::none;
}

uint8_t InputStreamDecompressor::ReadByte()
{
	if (IsEOF())
	{
		throw std::ios_base::failure("ReadByte attempt on EOF");
	}

	auto result = m_buffer->byte;
	if (m_buffer->count > 0)
	{
		--m_buffer->count;
	}
	else
	{
		m_buffer.reset();
		TryReadBuffer();
	}
	return result;
}

std::streamsize InputStreamDecompressor::ReadBlock(void * dstBuffer, std::streamsize size)
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

void InputStreamDecompressor::TryReadBuffer()
{
	std::vector<uint8_t> data(2);
	if (m_stream->ReadBlock(data.data(), 2) == 2)
	{
		m_buffer = RlePair(data[0], data[1]);
	}
}
