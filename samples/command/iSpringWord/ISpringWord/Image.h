#pragma once

#include "IImage.h"
#include "ImageKeeper.h"

class CImage : public IImage
{
public:
	explicit CImage(std::string const& path, unsigned index, int width, int height);

	std::string GetPath() const final;
	int GetWidth() const final;
	int GetHeight() const final;
	void Resize(int width, int height) final;

private:
	ImageKeeperPtr m_keeper;
	int m_width;
	int m_height;
};
