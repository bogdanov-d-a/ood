#include "stdafx.h"
#include "ResizeImageCommand.h"

ResizeImageCommand::ResizeImageCommand(CImage & image, int width, int height)
	: m_image(image)
	, m_newSize(width, height)
{
}

ResizeImageCommand::Size ResizeImageCommand::GetImageSize() const
{
	return Size(m_image.GetWidth(), m_image.GetHeight());
}

void ResizeImageCommand::SetImageSize(Size const & size)
{
	m_image.ResizeData(size.width, size.height);
}

void ResizeImageCommand::DoExecute()
{
	m_oldSize = GetImageSize();
	SetImageSize(m_newSize);
}

void ResizeImageCommand::DoUnexecute()
{
	assert(GetImageSize() == m_newSize);
	SetImageSize(m_oldSize);
}
