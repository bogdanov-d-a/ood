#include "deps.h"
#include "MemoryOutputStream.h"

std::vector<uint8_t> const & MemoryOutputStream::GetData() const
{
	return m_data;
}

void MemoryOutputStream::WriteByte(uint8_t data)
{
	m_data.push_back(data);
}

void MemoryOutputStream::WriteBlock(const void * srcData, std::streamsize size)
{
	decltype(auto) srcDataCast = reinterpret_cast<uint8_t const*>(srcData);

	for (std::streamsize i = 0; i < size; ++i)
	{
		WriteByte(*srcDataCast);
		++srcDataCast;
	}
}
