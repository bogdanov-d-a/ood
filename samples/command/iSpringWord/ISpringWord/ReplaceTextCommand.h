#pragma once

#include "AbstractCommand.h"
#include "Paragraph.h"

class ReplaceTextCommand : public CAbstractCommand
{
public:
	explicit ReplaceTextCommand(CParagraph &paragraph, std::string const& newText);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	CParagraph &m_paragraph;
	const std::string m_newText;
	std::string m_oldText;
};
