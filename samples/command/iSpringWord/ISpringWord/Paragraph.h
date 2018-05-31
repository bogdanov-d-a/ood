#pragma once
#include <memory>
#include "IParagraph.h"

//class CParagraphImpl;

class CParagraph:public IParagraph
{
public:
	std::string GetText()const final;
	void SetText(const std::string& text) final;

private:
	std::string m_text;
};

