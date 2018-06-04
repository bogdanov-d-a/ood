#include "stdafx.h"
#include "InsertParagraphCommand.h"

InsertParagraphCommand::InsertParagraphCommand(std::string const & text,
		const boost::optional<size_t>& position, OnInsert const & onInsert, OnRemove const & onRemove)
	: m_text(text)
	, m_position(position)
	, m_onInsert(onInsert)
	, m_onRemove(onRemove)
{
}

void InsertParagraphCommand::DoExecute()
{
	m_onInsert(m_text, m_position);
}

void InsertParagraphCommand::DoUnexecute()
{
	m_onRemove(m_position);
}
