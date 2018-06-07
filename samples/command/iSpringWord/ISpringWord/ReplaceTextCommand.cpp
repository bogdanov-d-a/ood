#include "stdafx.h"
#include "ReplaceTextCommand.h"
#include "Paragraph.h"

ReplaceTextCommand::ReplaceTextCommand(DocumentData & documentData, std::string const & newText, const boost::optional<size_t>& position)
	: m_documentData(documentData)
	, m_newText(newText)
	, m_position(position)
{
}

void ReplaceTextCommand::DoExecute()
{
	auto item = m_documentData.GetItemData(m_position);
	auto paragraphPtr = boost::get<std::shared_ptr<CParagraph>>(&item);
	assert(paragraphPtr && *paragraphPtr);

	m_oldText = (*paragraphPtr)->GetText();
	(*paragraphPtr)->SetTextData(m_newText);
}

void ReplaceTextCommand::DoUnexecute()
{
	auto item = m_documentData.GetItemData(m_position);
	auto paragraphPtr = boost::get<std::shared_ptr<CParagraph>>(&item);
	assert(paragraphPtr && *paragraphPtr);

	assert((*paragraphPtr)->GetText() == m_newText);
	(*paragraphPtr)->SetTextData(m_oldText);
}
