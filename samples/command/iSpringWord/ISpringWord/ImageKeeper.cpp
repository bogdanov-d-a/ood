#include "stdafx.h"
#include "ImageKeeper.h"

ImageKeeper::ImageKeeper(std::string const & path)
	: m_path(path)
{
}

ImageKeeper::~ImageKeeper()
{
	if (!m_keepAlive)
	{
		if (!DeleteFileA(m_path.c_str()))
		{
			std::cerr << "~ImageKeeper() DeleteFileA failed" << std::endl;
		}
	}
}

std::string ImageKeeper::GetPath() const
{
	return m_path;
}
