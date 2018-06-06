#include "stdafx.h"
#include "Document.h"
#include "SetTitleCommand.h"
#include "InsertParagraphCommand.h"

using namespace std;

std::shared_ptr<IParagraph> CDocument::InsertParagraph(const std::string & text, const boost::optional<size_t>& position)
{
	m_history.AddAndExecuteCommand(make_unique<InsertParagraphCommand>(m_data, text, position));
	return GetItem(position ? *position : GetItemsCount() - 1).GetParagraph();
}

std::shared_ptr<IImage> CDocument::InsertImage(const std::string & path, int width, int height, const boost::optional<size_t>& position)
{
	throw std::runtime_error("not implemented");
}

size_t CDocument::GetItemsCount() const
{
	return m_data.GetItemsCount();
}

CConstDocumentItem CDocument::GetItem(size_t index) const
{
	return m_data.GetItem(index);
}

CDocumentItem CDocument::GetItem(size_t index)
{
	return m_data.GetItem(index);
}

void CDocument::DeleteItem(size_t index)
{
	m_data.DeleteItem(index);
}

void CDocument::SetTitle(const std::string & title)
{
	m_history.AddAndExecuteCommand(make_unique<SetTitleCommand>(m_data, title));
}

std::string CDocument::GetTitle() const
{
	return m_data.GetTitle();
}

bool CDocument::CanUndo() const
{
	return m_history.CanUndo();
}

void CDocument::Undo()
{
	m_history.Undo();
}

bool CDocument::CanRedo() const
{
	return m_history.CanRedo();
}

void CDocument::Redo()
{
	m_history.Redo();
}