#include "stdafx.h"
#include "Image.h"
#include "ResizeImageCommand.h"
#include "Utils.h"

CImage::CImage(OnCreateCommand const& onCreateCommand, OnCopyImage const& onCopyImage,
		ImageKeeperCreator const& imageKeeperCreator, std::string const & path, int width, int height)
	: m_onCreateCommand(onCreateCommand)
	, m_width(width)
	, m_height(height)
{
	const auto clonePath = onCopyImage(path);
	m_keeper = imageKeeperCreator(clonePath);
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
	Utils::ValidateImageSize(width, height);
	m_width = width;
	m_height = height;
}

IImageKeeperPtr CImage::GetKeeper() const
{
	return m_keeper;
}
