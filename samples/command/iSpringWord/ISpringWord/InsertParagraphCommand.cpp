#include "stdafx.h"
#include "InsertParagraphCommand.h"

InsertParagraphCommand::InsertParagraphCommand(DocumentData &data, std::string const & text, const boost::optional<size_t>& position)
	: m_data(data)
	, m_executeData(text)
	, m_position(position)
{
}

void InsertParagraphCommand::DoExecute()
{
	if (auto text = boost::get<std::string>(&m_executeData))
	{
		m_data.InsertParagraph(*text, m_position);
	}
	else if (auto paragraph = boost::get<std::shared_ptr<CParagraph>>(&m_executeData))
	{
		m_data.InsertItem(std::move(*paragraph), m_position);
	}
	else
	{
		assert(false);
	}
	m_executeData = Empty();
}

void InsertParagraphCommand::DoUnexecute()
{
	assert(m_executeData.type() == typeid(Empty));

	auto item = m_data.DeleteItem(m_position);

	auto paragraphPtr = boost::get<std::shared_ptr<CParagraph>>(&item);
	assert(paragraphPtr);

	m_executeData = std::move(*paragraphPtr);
}
