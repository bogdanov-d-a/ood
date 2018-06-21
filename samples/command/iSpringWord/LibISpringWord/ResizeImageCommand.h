#pragma once

#include "AbstractCommand.h"
#include "Image.h"

class ResizeImageCommand : public CAbstractCommand
{
public:
	explicit ResizeImageCommand(CImage &image, int width, int height);

private:
	struct Size
	{
		Size()
			: Size(0, 0)
		{}

		Size(int width, int height)
			: width(width)
			, height(height)
		{}

		bool operator==(Size const& other) const
		{
			return width == other.width && height == other.height;
		}

		int width;
		int height;
	};

	Size GetImageSize() const;
	void SetImageSize(Size const& size);

	void DoExecute() final;
	void DoUnexecute() final;

	CImage &m_image;
	Size m_oldSize;
	const Size m_newSize;
};
