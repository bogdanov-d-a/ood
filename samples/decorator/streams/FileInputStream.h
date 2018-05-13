#pragma once

#include "IInputDataStream.h"

class FileInputStream : public IInputDataStream
{
public:
	explicit FileInputStream(std::string const& filename);

	bool IsEOF() final;
	uint8_t ReadByte() final;
	std::streamsize ReadBlock(void * dstBuffer, std::streamsize size) final;

private:
	std::ifstream m_stream;
};
