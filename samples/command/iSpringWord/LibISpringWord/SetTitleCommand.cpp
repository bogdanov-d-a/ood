#include "stdafx.h"
#include "SetTitleCommand.h"

SetTitleCommand::SetTitleCommand(DocumentData &data, std::string const & newTitle)
	: m_data(data)
	, m_newTitle(newTitle)
{
}

void SetTitleCommand::DoExecute()
{
	m_oldTitle = m_data.GetTitle();
	m_data.SetTitle(m_newTitle);
}

void SetTitleCommand::DoUnexecute()
{
	assert(m_data.GetTitle() == m_newTitle);
	m_data.SetTitle(m_oldTitle);
}
