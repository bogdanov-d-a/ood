#include "stdafx.h"
#include "ConstDocumentItem.h"


CConstDocumentItem::CConstDocumentItem(std::shared_ptr<const IImage>&& imagePtr, std::shared_ptr<const IParagraph>&& paragraphPtr)
	: m_imagePtr(std::move(imagePtr))
	, m_paragraphPtr(std::move(paragraphPtr))
{
}

std::shared_ptr<const IImage> CConstDocumentItem::GetImage()const
{
	return m_imagePtr;
}

std::shared_ptr<const IParagraph> CConstDocumentItem::GetParagraph()const
{
	return m_paragraphPtr;
}
