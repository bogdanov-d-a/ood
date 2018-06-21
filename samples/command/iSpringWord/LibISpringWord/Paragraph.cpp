#include "stdafx.h"
#include "Paragraph.h"
#include "ReplaceTextCommand.h"

CParagraph::CParagraph(OnCreateCommand const& onCreateCommand, const std::string& text)
	: m_onCreateCommand(onCreateCommand)
	, m_text(text)
{
}

std::string CParagraph::GetText() const
{
	return m_text;
}

void CParagraph::SetText(const std::string & text)
{
	m_onCreateCommand(std::make_unique<ReplaceTextCommand>(*this, text));
}

void CParagraph::SetTextData(const std::string & text)
{
	m_text = text;
}
