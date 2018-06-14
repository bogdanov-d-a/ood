#pragma once

#include "IImage.h"
#include "ICommand.h"
#include "ImageKeeper.h"

class CImage : public IImage
{
public:
	using OnCreateCommand = std::function<void(ICommandPtr)>;

	explicit CImage(OnCreateCommand const& onCreateCommand, std::string const& path, unsigned index, int width, int height);

	std::string GetPath() const final;
	int GetWidth() const final;
	int GetHeight() const final;
	void Resize(int width, int height) final;
	void ResizeData(int width, int height);
	ImageKeeperPtr GetKeeper() const;

private:
	OnCreateCommand m_onCreateCommand;
	ImageKeeperPtr m_keeper;
	int m_width;
	int m_height;
};
