#pragma once

class ImageKeeper
{
public:
	explicit ImageKeeper(std::string const& path);
	~ImageKeeper();

	std::string GetPath() const;

private:
	std::string m_path;
	bool m_keepAlive = false;
};

using ImageKeeperPtr = std::shared_ptr<ImageKeeper>;
