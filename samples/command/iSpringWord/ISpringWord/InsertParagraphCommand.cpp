#include "stdafx.h"
#include "InsertParagraphCommand.h"

InsertParagraphCommand::InsertParagraphCommand(DocumentData &data, std::string const & text, const boost::optional<size_t>& position)
	: m_data(data)
	, m_text(text)
	, m_position(position)
{
}

void InsertParagraphCommand::DoExecute()
{
	m_data.InsertParagraph(m_text, m_position);
}

void InsertParagraphCommand::DoUnexecute()
{
	m_data.DeleteItem(m_position);
}
