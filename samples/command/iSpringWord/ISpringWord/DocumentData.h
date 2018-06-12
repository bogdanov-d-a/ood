#pragma once

#include "DocumentItem.h"
#include "IImage.h"
#include "ICommand.h"

class CParagraph;

class DocumentData
{
public:
	using ItemData = boost::variant<std::shared_ptr<CParagraph>, std::shared_ptr<IImage>>;
	using OnCreateCommand = std::function<void(ICommandPtr)>;

	explicit DocumentData(OnCreateCommand const& onCreateCommand);

	std::shared_ptr<IParagraph> InsertParagraph(const std::string& text,
		const boost::optional<size_t>& position = boost::none);

	std::shared_ptr<IImage> InsertImage(const std::string& path, int width, int height,
		const boost::optional<size_t>& position = boost::none);

	void InsertItem(ItemData && item, const boost::optional<size_t>& position = boost::none);

	size_t GetItemsCount() const;

	CConstDocumentItem GetItem(const boost::optional<size_t>& position) const;
	CDocumentItem GetItem(const boost::optional<size_t>& position);
	ItemData GetItemData(const boost::optional<size_t>& position);

	void DeleteItem(const boost::optional<size_t>& position);

	void SetTitle(const std::string & title);
	std::string GetTitle() const;

private:
	std::string m_title;
	std::vector<ItemData> m_items;
	OnCreateCommand m_onCreateCommand;
};
