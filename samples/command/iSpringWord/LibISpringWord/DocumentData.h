#pragma once

#include "DocumentItem.h"
#include "IImage.h"
#include "ICommand.h"
#include "IImageKeeper.h"

class CParagraph;
class CImage;

class DocumentData
{
public:
	using ItemData = boost::variant<std::shared_ptr<CParagraph>, std::shared_ptr<CImage>>;
	using OnCreateCommand = std::function<void(ICommandPtr&&)>;
	using OnCopyImage = std::function<std::string(std::string const&)>;
	using ImageKeeperCreator = std::function<IImageKeeperPtr(std::string const&)>;

	explicit DocumentData(OnCreateCommand const& onCreateCommand,
		OnCopyImage const& onCopyImage, ImageKeeperCreator const& imageKeeperCreator);

	void InsertParagraph(const std::string& text,
		const boost::optional<size_t>& position = boost::none);

	void InsertImage(const std::string& path, int width, int height,
		const boost::optional<size_t>& position = boost::none);

	void InsertItem(ItemData && item, const boost::optional<size_t>& position = boost::none);

	size_t GetItemsCount() const;

	CConstDocumentItem GetItem(const boost::optional<size_t>& position) const;
	CDocumentItem GetItem(const boost::optional<size_t>& position);
	ItemData GetItemData(const boost::optional<size_t>& position);

	ItemData DeleteItem(const boost::optional<size_t>& position);

	void SetTitle(const std::string & title);
	std::string GetTitle() const;

	IImageKeeperPtr GetImageKeeper(size_t index) const;

private:
	std::string m_title;
	std::vector<ItemData> m_items;
	const OnCreateCommand m_onCreateCommand;
	const OnCopyImage m_onCopyCommand;
	const ImageKeeperCreator m_imageKeeperCreator;
};
