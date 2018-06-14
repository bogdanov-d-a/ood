#include "stdafx.h"
#include "Document.h"
#include "SetTitleCommand.h"
#include "InsertParagraphCommand.h"
#include "InsertImageCommand.h"
#include "DeleteItemCommand.h"

using namespace std;

namespace
{

std::string EscapeStr(std::string const& str)
{
	std::string result;

	constexpr std::array<std::pair<char, char*>, 5> escapeMap = { {
		{ '&', "&amp;" },
		{ '<', "&lt;" },
		{ '>', "&gt;" },
		{ '"', "&quot;" },
		{ '\'', "&apos;" },
	} };

	for (auto c : str)
	{
		const auto found = std::find_if(escapeMap.begin(), escapeMap.end(),
			[c](std::pair<char, char*> const& elem) {
				return elem.first == c;
			}
		);

		if (found != escapeMap.end())
		{
			result += found->second;
		}
		else
		{
			result.push_back(c);
		}
	}

	return result;
}

}

CDocument::CDocument(OnKeepImage const& onKeepImage)
	: m_onKeepImage(onKeepImage)
	, m_data([this](ICommandPtr cmd) {
		m_history.AddAndExecuteCommand(std::move(cmd));
	})
{
}

std::shared_ptr<IParagraph> CDocument::InsertParagraph(const std::string & text, const boost::optional<size_t>& position)
{
	m_history.AddAndExecuteCommand(make_unique<InsertParagraphCommand>(m_data, text, position));
	return GetItem(position ? *position : GetItemsCount() - 1).GetParagraph();
}

std::shared_ptr<IImage> CDocument::InsertImage(const std::string & path, int width, int height, const boost::optional<size_t>& position)
{
	m_history.AddAndExecuteCommand(make_unique<InsertImageCommand>(m_data, path, width, height, position));
	return GetItem(position ? *position : GetItemsCount() - 1).GetImage();
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
	m_history.AddAndExecuteCommand(make_unique<DeleteItemCommand>(m_data, index));
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

void CDocument::Save(const std::string & path) const
{
	std::ofstream out(path);

	out << "<!DOCTYPE html>" << std::endl;
	out << "<html>" << std::endl;

	out << "<head>" << std::endl;
	out << "<title>" << m_data.GetTitle() << "</title>" << std::endl;
	out << "</head>" << std::endl;

	out << "<body>" << std::endl;

	for (size_t i = 0; i < m_data.GetItemsCount(); ++i)
	{
		auto item = m_data.GetItem(i);
		if (auto paragraph = item.GetParagraph())
		{
			out << "<p>" << EscapeStr(paragraph->GetText()) << "</p>" << std::endl;
		}
		else if (auto image = item.GetImage())
		{
			out << "<img src=\"" << image->GetPath() << "\" width=\"" << image->GetWidth() << "\" height=\"" << image->GetHeight() << "\">" << std::endl;
			m_onKeepImage(m_data.GetImageKeeper(i));
		}
		else
		{
			assert(false);
		}
	}

	out << "</body>" << std::endl;
	out << "</html>" << std::endl;
}
