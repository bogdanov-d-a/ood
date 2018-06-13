#include "stdafx.h"
#include "DocumentData.h"
#include "Paragraph.h"
#include "Image.h"

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

DocumentData::DocumentData(OnCreateCommand const & onCreateCommand)
	: m_onCreateCommand(onCreateCommand)
{
}

std::shared_ptr<IParagraph> DocumentData::InsertParagraph(const std::string & text, const boost::optional<size_t>& position)
{
	auto result = std::make_shared<CParagraph>(m_onCreateCommand, text);
	m_items.insert(InsertPositionToIterator(m_items, position), result);
	return result;
}

std::shared_ptr<IImage> DocumentData::InsertImage(const std::string & path, int width, int height, const boost::optional<size_t>& position)
{
	auto result = std::make_shared<CImage>(m_onCreateCommand, path, m_imageIndex++, width, height);
	m_items.insert(InsertPositionToIterator(m_items, position), result);
	return result;
}

void DocumentData::InsertItem(ItemData && item, const boost::optional<size_t>& position)
{
	m_items.insert(InsertPositionToIterator(m_items, position), std::move(item));
}

size_t DocumentData::GetItemsCount() const
{
	return m_items.size();
}

CConstDocumentItem DocumentData::GetItem(const boost::optional<size_t>& position) const
{
	auto &item = m_items.at(PositionToIndex(GetItemsCount(), position));

	auto textPtr = boost::get<std::shared_ptr<CParagraph>>(&item);
	std::shared_ptr<CParagraph> text;

	if (textPtr)
	{
		text = *textPtr;
	}

	auto imagePtr = boost::get<std::shared_ptr<CImage>>(&item);
	std::shared_ptr<CImage> image;

	if (imagePtr)
	{
		image = *imagePtr;
	}

	return CConstDocumentItem(std::move(image), std::move(text));
}

CDocumentItem DocumentData::GetItem(const boost::optional<size_t>& position)
{
	auto &item = m_items.at(PositionToIndex(GetItemsCount(), position));

	auto textPtr = boost::get<std::shared_ptr<CParagraph>>(&item);
	std::shared_ptr<CParagraph> text;

	if (textPtr)
	{
		text = *textPtr;
	}

	auto imagePtr = boost::get<std::shared_ptr<CImage>>(&item);
	std::shared_ptr<CImage> image;

	if (imagePtr)
	{
		image = *imagePtr;
	}

	return CDocumentItem(std::move(image), std::move(text));
}

DocumentData::ItemData DocumentData::GetItemData(const boost::optional<size_t>& position)
{
	return m_items.at(PositionToIndex(GetItemsCount(), position));
}

DocumentData::ItemData DocumentData::DeleteItem(const boost::optional<size_t>& position)
{
	auto result = std::move(m_items.at(PositionToIndex(GetItemsCount(), position)));
	m_items.erase(position ? m_items.begin() + *position : --m_items.end());
	return result;
}

void DocumentData::SetTitle(const std::string & title)
{
	m_title = title;
}

std::string DocumentData::GetTitle() const
{
	return m_title;
}
