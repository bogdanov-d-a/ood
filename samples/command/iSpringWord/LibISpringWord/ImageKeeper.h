#pragma once

#include "IImageKeeper.h"

class ImageKeeper : public IImageKeeper
{
public:
	explicit ImageKeeper(std::string const& path);
	~ImageKeeper();

	std::string GetPath() const final;
	void KeepAlive() final;

private:
	std::string m_path;
	bool m_keepAlive = false;
};
