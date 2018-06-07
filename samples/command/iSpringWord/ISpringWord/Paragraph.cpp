#include "stdafx.h"
#include "Paragraph.h"

CParagraph::CParagraph(DocumentData &documentData, const std::string& text)
	: m_documentData(documentData)
	, m_text(text)
{
}

std::string CParagraph::GetText() const
{
	return m_text;
}

void CParagraph::SetText(const std::string & text)
{
	m_documentData.CallOnSetParagraphText(this, text);
}

void CParagraph::SetTextData(const std::string & text)
{
	m_text = text;
}
