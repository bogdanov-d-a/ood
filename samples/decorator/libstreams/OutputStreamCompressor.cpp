#include "deps.h"
#include "OutputStreamCompressor.h"

OutputStreamCompressor::OutputStreamCompressor(IOutputDataStreamPtr && stream)
	: m_stream(std::move(stream))
{
}

OutputStreamCompressor::~OutputStreamCompressor()
{
	SyncBuffer();
}

void OutputStreamCompressor::WriteByte(uint8_t data)
{
	assert(m_length <= UINT8_MAX + 1);
	if (m_length == UINT8_MAX + 1)
	{
		SyncBuffer();
	}

	if (m_length > 0 && m_byte == data)
	{
		++m_length;
	}
	else
	{
		SyncBuffer();
		m_length = 1;
		m_byte = data;
	}
}

void OutputStreamCompressor::WriteBlock(const void * srcData, std::streamsize size)
{
	decltype(auto) srcDataCast = reinterpret_cast<uint8_t const*>(srcData);

	for (std::streamsize i = 0; i < size; ++i)
	{
		WriteByte(*srcDataCast);
		++srcDataCast;
	}
}

void OutputStreamCompressor::SyncBuffer()
{
	if (m_length > 0)
	{
		m_stream->WriteByte(static_cast<uint8_t>(m_length - 1));
		m_stream->WriteByte(m_byte);
	}
	m_length = 0;
}
