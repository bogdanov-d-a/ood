#pragma once

#include "IInputDataStream.h"

class InputStreamDecryptor : public IInputDataStream
{
public:
	explicit InputStreamDecryptor(IInputDataStreamPtr && stream, uint32_t key);

	bool IsEOF() final;
	uint8_t ReadByte() final;
	std::streamsize ReadBlock(void * dstBuffer, std::streamsize size) final;

private:
	IInputDataStreamPtr m_stream;
	const std::vector<uint8_t> m_table;
};
