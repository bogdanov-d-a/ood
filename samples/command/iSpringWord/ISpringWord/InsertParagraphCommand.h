#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class InsertParagraphCommand : public CAbstractCommand
{
public:
	explicit InsertParagraphCommand(DocumentData &data, std::string const& text, const boost::optional<size_t>& position);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_data;
	const std::string m_text;
	const boost::optional<size_t> m_position;
};
