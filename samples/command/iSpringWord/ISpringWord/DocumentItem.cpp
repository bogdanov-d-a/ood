#include "stdafx.h"
#include "DocumentItem.h"

CDocumentItem::CDocumentItem(std::shared_ptr<IImage> const& imagePtr, std::shared_ptr<IParagraph> const& paragraphPtr)
	: CConstDocumentItem(imagePtr, paragraphPtr)
	, m_imagePtr(imagePtr)
	, m_paragraphPtr(paragraphPtr)
{
}

std::shared_ptr<IImage> CDocumentItem::GetImage()
{
	return m_imagePtr;
}

std::shared_ptr<IParagraph> CDocumentItem::GetParagraph()
{
	return m_paragraphPtr;
}
