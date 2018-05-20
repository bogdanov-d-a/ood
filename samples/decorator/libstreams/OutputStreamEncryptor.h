#pragma once

#include "IOutputDataStream.h"
#include "ReplaceTable.h"

class OutputStreamEncryptor : public IOutputDataStream
{
public:
	explicit OutputStreamEncryptor(IOutputDataStreamPtr && stream, uint32_t key);

	void WriteByte(uint8_t data) final;
	void WriteBlock(const void * srcData, std::streamsize size) final;

private:
	IOutputDataStreamPtr m_stream;
	const ReplaceTable m_table;
};
