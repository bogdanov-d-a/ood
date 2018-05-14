#pragma once

#include "IOutputDataStream.h"

class MemoryOutputStream : public IOutputDataStream
{
public:
	std::vector<uint8_t> const& GetData() const;

	void WriteByte(uint8_t data) final;
	void WriteBlock(const void * srcData, std::streamsize size) final;

private:
	std::vector<uint8_t> m_data;
};
