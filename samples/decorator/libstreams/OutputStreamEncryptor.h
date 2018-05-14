#pragma once

#include "IOutputDataStream.h"

class OutputStreamEncryptor : public IOutputDataStream
{
public:
	explicit OutputStreamEncryptor(IOutputDataStreamPtr && stream, uint32_t key);

	void WriteByte(uint8_t data) final;
	void WriteBlock(const void * srcData, std::streamsize size) final;

private:
	IOutputDataStreamPtr m_stream;
	const std::vector<uint8_t> m_table;
};
