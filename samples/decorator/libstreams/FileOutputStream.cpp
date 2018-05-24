#include "deps.h"
#include "FileOutputStream.h"

FileOutputStream::FileOutputStream(std::string const & filename)
	: m_stream(filename, std::ios::binary)
{
	m_stream.exceptions(std::ifstream::failbit | std::ifstream::badbit);
}

void FileOutputStream::WriteByte(uint8_t data)
{
	m_stream.put(data);
}

void FileOutputStream::WriteBlock(const void * srcData, std::streamsize size)
{
	decltype(auto) srcDataCast = reinterpret_cast<char const*>(srcData);
	m_stream.write(srcDataCast, size);
}
