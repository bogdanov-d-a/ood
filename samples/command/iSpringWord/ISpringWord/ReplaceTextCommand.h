#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class ReplaceTextCommand : public CAbstractCommand
{
public:
	explicit ReplaceTextCommand(DocumentData &documentData, std::string const& newText, const boost::optional<size_t>& position);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_documentData;
	const std::string m_newText;
	const boost::optional<size_t> m_position;
	std::string m_oldText;
};
