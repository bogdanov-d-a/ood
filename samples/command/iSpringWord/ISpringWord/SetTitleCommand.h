#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class SetTitleCommand : public CAbstractCommand
{
public:
	explicit SetTitleCommand(DocumentData &data, std::string const& newTitle);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_data;
	const std::string m_newTitle;
	std::string m_oldTitle;
};
