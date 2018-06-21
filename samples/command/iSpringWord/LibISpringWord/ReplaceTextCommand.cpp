#include "stdafx.h"
#include "ReplaceTextCommand.h"
#include "Paragraph.h"

ReplaceTextCommand::ReplaceTextCommand(CParagraph &paragraph, std::string const & newText)
	: m_paragraph(paragraph)
	, m_newText(newText)
{
}

void ReplaceTextCommand::DoExecute()
{
	m_oldText = m_paragraph.GetText();
	m_paragraph.SetTextData(m_newText);
}

void ReplaceTextCommand::DoUnexecute()
{
	assert(m_paragraph.GetText() == m_newText);
	m_paragraph.SetTextData(m_oldText);
}
