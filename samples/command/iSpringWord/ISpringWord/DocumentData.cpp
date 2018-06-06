#include "stdafx.h"
#include "DocumentData.h"
#include "Paragraph.h"

namespace
{

template<typename T>
decltype(auto) InsertPositionToIterator(T &container, const boost::optional<size_t>& position)
{
	return position ? container.begin() + *position : container.end();
}

size_t PositionToIndex(size_t itemCount, const boost::optional<size_t>& position)
{
	return position ? *position : itemCount - 1;
}

}

std::shared_ptr<IParagraph> DocumentData::InsertParagraph(const std::string & text, const boost::optional<size_t>& position)
{
	auto result = std::make_shared<CParagraph>();
	result->SetText(text);
	m_items.insert(InsertPositionToIterator(m_items, position), CDocumentItem(std::shared_ptr<IImage>(), result));
	return result;
}

std::shared_ptr<IImage> DocumentData::InsertImage(const std::string & path, int width, int height, const boost::optional<size_t>& position)
{
	throw std::runtime_error("not implemented");
}

void DocumentData::InsertItem(CDocumentItem && item, const boost::optional<size_t>& position)
{
	m_items.insert(InsertPositionToIterator(m_items, position), std::move(item));
}

size_t DocumentData::GetItemsCount() const
{
	return m_items.size();
}

CConstDocumentItem DocumentData::GetItem(const boost::optional<size_t>& position) const
{
	return m_items.at(PositionToIndex(GetItemsCount(), position));
}

CDocumentItem DocumentData::GetItem(const boost::optional<size_t>& position)
{
	return m_items.at(PositionToIndex(GetItemsCount(), position));
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
