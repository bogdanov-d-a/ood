#pragma once
#include "ConstDocumentItem.h"

class CDocumentItem : public CConstDocumentItem
{
public:
	CDocumentItem(std::shared_ptr<IImage> const& imagePtr, std::shared_ptr<IParagraph> const& paragraphPtr);
	// Возвращает указатель на изображение, либо nullptr, если элемент не является изображением
	std::shared_ptr<IImage> GetImage();
	// Возвращает указатель на параграф, либо nullptr, если элемент не является параграфом
	std::shared_ptr<IParagraph> GetParagraph();

private:
	std::shared_ptr<IImage> m_imagePtr;
	std::shared_ptr<IParagraph> m_paragraphPtr;
};
