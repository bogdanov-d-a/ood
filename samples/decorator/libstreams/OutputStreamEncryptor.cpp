#include "deps.h"
#include "OutputStreamEncryptor.h"

OutputStreamEncryptor::OutputStreamEncryptor(IOutputDataStreamPtr && stream, uint32_t key)
	: m_stream(std::move(stream))
	, m_table(key)
{
}

void OutputStreamEncryptor::WriteByte(uint8_t data)
{
	m_stream->WriteByte(m_table.Map(data));
}

void OutputStreamEncryptor::WriteBlock(const void * srcData, std::streamsize size)
{
	decltype(auto) srcDataCast = reinterpret_cast<uint8_t const*>(srcData);

	for (std::streamsize i = 0; i < size; ++i)
	{
		WriteByte(*srcDataCast);
		++srcDataCast;
	}
}
