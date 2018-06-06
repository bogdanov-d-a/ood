#include "stdafx.h"
#include "DocumentData.h"
#include "Paragraph.h"

std::shared_ptr<IParagraph> DocumentData::InsertParagraph(const std::string & text, const boost::optional<size_t>& position)
{
	auto result = std::make_shared<CParagraph>();
	result->SetText(text);
	m_items.insert(position ? m_items.begin() + *position : m_items.end(), CDocumentItem(std::shared_ptr<IImage>(), result));
	return result;
}

std::shared_ptr<IImage> DocumentData::InsertImage(const std::string & path, int width, int height, const boost::optional<size_t>& position)
{
	throw std::runtime_error("not implemented");
}

size_t DocumentData::GetItemsCount() const
{
	return m_items.size();
}

CConstDocumentItem DocumentData::GetItem(size_t index) const
{
	return m_items.at(index);
}

CDocumentItem DocumentData::GetItem(size_t index)
{
	return m_items.at(index);
}

void DocumentData::DeleteItem(const boost::optional<size_t>& position)
{
	m_items.erase(position ? m_items.begin() + *position : --m_items.end());
}

void DocumentData::SetTitle(const std::string & title)
{
	m_title = title;
}

std::string DocumentData::GetTitle() const
{
	return m_title;
}
