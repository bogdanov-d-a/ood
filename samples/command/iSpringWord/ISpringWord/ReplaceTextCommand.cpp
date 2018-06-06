#include "stdafx.h"
#include "ReplaceTextCommand.h"

ReplaceTextCommand::ReplaceTextCommand(DocumentData & documentData, std::string const & newText, const boost::optional<size_t>& position)
	: m_documentData(documentData)
	, m_newText(newText)
	, m_position(position)
{
}

void ReplaceTextCommand::DoExecute()
{
	auto& paragraph = m_documentData.GetItem(m_position).GetParagraph();
	m_oldText = paragraph->GetText();
	paragraph->SetText(m_newText);
}

void ReplaceTextCommand::DoUnexecute()
{
	auto& paragraph = m_documentData.GetItem(m_position).GetParagraph();
	assert(paragraph->GetText() == m_newText);
	paragraph->SetText(m_oldText);
}
