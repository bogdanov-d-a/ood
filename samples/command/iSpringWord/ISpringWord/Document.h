#pragma once
#include "IDocument.h"
#include "History.h"
#include "DocumentData.h"
#include "ImageKeeper.h"

class CDocument:public IDocument
{
public:
	using OnKeepImage = std::function<void(ImageKeeperPtr const& keeper)>;

	explicit CDocument(OnKeepImage const& onKeepImage);

	std::shared_ptr<IParagraph> InsertParagraph(const std::string& text,
		const boost::optional<size_t>& position = boost::none) final;

	std::shared_ptr<IImage> InsertImage(const std::string& path, int width, int height,
		const boost::optional<size_t>& position = boost::none) final;

	size_t GetItemsCount()const final;

	CConstDocumentItem GetItem(size_t index)const final;
	CDocumentItem GetItem(size_t index) final;

	void DeleteItem(size_t index) final;

	void SetTitle(const std::string & title) override;
	std::string GetTitle() const override;

	bool CanUndo() const override;	
	void Undo() override;
	bool CanRedo() const override;
	void Redo() override;

	void Save(const std::string& path)const final;

private:
	OnKeepImage m_onKeepImage;
	DocumentData m_data;
	CHistory m_history;
};