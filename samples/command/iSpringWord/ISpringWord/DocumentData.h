#pragma once

#include "DocumentItem.h"

class DocumentData
{
public:
	std::shared_ptr<IParagraph> InsertParagraph(const std::string& text,
		const boost::optional<size_t>& position = boost::none);

	std::shared_ptr<IImage> InsertImage(const std::string& path, int width, int height,
		const boost::optional<size_t>& position = boost::none);

	void InsertItem(CDocumentItem && item, const boost::optional<size_t>& position = boost::none);

	size_t GetItemsCount() const;

	CConstDocumentItem GetItem(const boost::optional<size_t>& position) const;
	CDocumentItem GetItem(const boost::optional<size_t>& position);

	void DeleteItem(const boost::optional<size_t>& position);

	void SetTitle(const std::string & title);
	std::string GetTitle() const;

private:
	std::string m_title;
	std::vector<CDocumentItem> m_items;
};
