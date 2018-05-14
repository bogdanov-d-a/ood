#pragma once

#include "IInputDataStream.h"

class MemoryInputStream : public IInputDataStream
{
public:
	explicit MemoryInputStream(std::vector<uint8_t> && data);

	bool IsEOF() final;
	uint8_t ReadByte() final;
	std::streamsize ReadBlock(void * dstBuffer, std::streamsize size) final;

private:
	std::vector<uint8_t> m_data;
	decltype(m_data.begin()) m_pos;
};
