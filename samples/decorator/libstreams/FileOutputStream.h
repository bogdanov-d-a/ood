#pragma once

#include "IOutputDataStream.h"

class FileOutputStream : public IOutputDataStream
{
public:
	explicit FileOutputStream(std::string const& filename);

	void WriteByte(uint8_t data) final;
	void WriteBlock(const void * srcData, std::streamsize size) final;

private:
	std::ofstream m_stream;
};
