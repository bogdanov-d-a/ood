#include "stdafx.h"
#include "Image.h"
#include "ResizeImageCommand.h"

namespace
{

std::string GetPathFromIndex(unsigned index, std::string const& ext)
{
	return "images\\img" + std::to_string(index) + ext;
}

std::string GetExtension(std::string const& path)
{
	size_t dotIndex = path.find_last_of('.');
	if (dotIndex == std::string::npos)
	{
		return "";
	}
	return path.substr(dotIndex);
}

std::string GetClonePath(std::string const& path, unsigned index)
{
	return GetPathFromIndex(index, GetExtension(path));
}

}

CImage::CImage(OnCreateCommand const& onCreateCommand, std::string const & path, unsigned index, int width, int height)
	: m_onCreateCommand(onCreateCommand)
	, m_width(width)
	, m_height(height)
{
	const auto clonePath = GetClonePath(path, index);
	if (!CopyFileA(path.c_str(), clonePath.c_str(), TRUE))
	{
		throw std::runtime_error("CopyFileA failed");
	}
	m_keeper = std::make_shared<ImageKeeper>(clonePath);
}

std::string CImage::GetPath() const
{
	return m_keeper->GetPath();
}

int CImage::GetWidth() const
{
	return m_width;
}

int CImage::GetHeight() const
{
	return m_height;
}

void CImage::Resize(int width, int height)
{
	m_onCreateCommand(std::make_unique<ResizeImageCommand>(*this, width, height));
}

void CImage::ResizeData(int width, int height)
{
	m_width = width;
	m_height = height;
}
