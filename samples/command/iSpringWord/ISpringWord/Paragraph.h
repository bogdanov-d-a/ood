#pragma once
#include <memory>
#include "IParagraph.h"
#include "ICommand.h"

class CParagraph:public IParagraph
{
public:
	using OnCreateCommand = std::function<void(ICommandPtr)>;

	explicit CParagraph(OnCreateCommand const& onCreateCommand, const std::string& text);
	std::string GetText()const final;
	void SetText(const std::string& text) final;
	void SetTextData(const std::string& text);

private:
	OnCreateCommand m_onCreateCommand;
	std::string m_text;
};

