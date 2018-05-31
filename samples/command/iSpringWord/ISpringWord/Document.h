#pragma once
#include "IDocument.h"
#include "History.h"

class CDocument:public IDocument
{
public:
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

private:
	std::string m_title;
	std::vector<CDocumentItem> m_items;

	CHistory m_history;
};