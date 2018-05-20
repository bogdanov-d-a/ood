#pragma once

#include "IInputDataStream.h"
#include "ReplaceTable.h"

class InputStreamDecryptor : public IInputDataStream
{
public:
	explicit InputStreamDecryptor(IInputDataStreamPtr && stream, uint32_t key);

	bool IsEOF() final;
	uint8_t ReadByte() final;
	std::streamsize ReadBlock(void * dstBuffer, std::streamsize size) final;

private:
	IInputDataStreamPtr m_stream;
	const ReplaceTable m_table;
};
