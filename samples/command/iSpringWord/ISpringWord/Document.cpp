#include "stdafx.h"
#include "Document.h"
#include "ChangeStringCommand.h"
#include "Paragraph.h"

using namespace std;

std::shared_ptr<IParagraph> CDocument::InsertParagraph(const std::string & text, boost::optional<size_t> position)
{
	// TODO: m_history.AddAndExecuteCommand

	auto result = std::make_shared<CParagraph>();
	result->SetText(text);
	m_items.insert(position ? m_items.begin() + *position : m_items.end(), CDocumentItem(std::shared_ptr<IImage>(), result));
	return result;
}

std::shared_ptr<IImage> CDocument::InsertImage(const std::string & path, int width, int height, boost::optional<size_t> position)
{
	throw std::runtime_error("not implemented");
}

size_t CDocument::GetItemsCount() const
{
	return m_items.size();
}

CConstDocumentItem CDocument::GetItem(size_t index) const
{
	return m_items.at(index);
}

CDocumentItem CDocument::GetItem(size_t index)
{
	return m_items.at(index);
}

void CDocument::DeleteItem(size_t index)
{
	m_items.erase(m_items.begin() + index);
}

void CDocument::SetTitle(const std::string & title)
{
	m_history.AddAndExecuteCommand(make_unique<CChangeStringCommand>(m_title, title));
}

std::string CDocument::GetTitle() const
{
	return m_title;
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