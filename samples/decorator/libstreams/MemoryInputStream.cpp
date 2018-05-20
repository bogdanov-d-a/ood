#include "deps.h"
#include "MemoryInputStream.h"

MemoryInputStream::MemoryInputStream(std::vector<uint8_t> && data)
	: m_data(data)
	, m_pos(m_data.begin())
{
}

bool MemoryInputStream::IsEOF()
{
	return m_pos == m_data.end();
}

uint8_t MemoryInputStream::ReadByte()
{
	if (IsEOF())
	{
		throw std::ios_base::failure("ReadByte attempt on EOF");
	}
	return *(m_pos++);
}

std::streamsize MemoryInputStream::ReadBlock(void * dstBuffer, std::streamsize size)
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
