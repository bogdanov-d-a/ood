#include "deps.h"
#include "FileInputStream.h"

FileInputStream::FileInputStream(std::string const & filename)
	: m_stream(filename, std::ios::binary)
{
	m_stream.exceptions(std::ifstream::failbit | std::ifstream::badbit);
}

bool FileInputStream::IsEOF()
{
	return m_stream.peek() == EOF;
}

uint8_t FileInputStream::ReadByte()
{
	if (IsEOF())
	{
		throw std::ios_base::failure("ReadByte attempt on EOF");
	}
	return m_stream.get();
}

std::streamsize FileInputStream::ReadBlock(void * dstBuffer, std::streamsize size)
{
	if (IsEOF())
	{
		return 0;
	}

	decltype(auto) dstBufferCast = reinterpret_cast<char*>(dstBuffer);

	BOOST_SCOPE_EXIT_ALL(this, oldFlags = m_stream.exceptions())
	{
		m_stream.exceptions(oldFlags);
	};
	m_stream.exceptions(m_stream.exceptions() & ~std::ifstream::failbit);

	m_stream.read(dstBufferCast, size);
	if (m_stream.fail() && !m_stream.eof())
	{
		throw std::ios_base::failure("ReadBlock failbit set");
	}
	return m_stream.gcount();
}
