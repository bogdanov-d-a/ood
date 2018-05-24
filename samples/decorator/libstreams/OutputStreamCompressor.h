#pragma once

#include "IOutputDataStream.h"

class OutputStreamCompressor : public IOutputDataStream
{
public:
	explicit OutputStreamCompressor(IOutputDataStreamPtr && stream);
	~OutputStreamCompressor();

	void SyncBuffer();

	void WriteByte(uint8_t data) final;
	void WriteBlock(const void * srcData, std::streamsize size) final;

private:
	IOutputDataStreamPtr m_stream;
	int m_length = 0;
	uint8_t m_byte = '\0';
};
