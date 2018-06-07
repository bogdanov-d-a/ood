#pragma once
#include <memory>
#include "IParagraph.h"
#include "DocumentData.h"

class CParagraph:public IParagraph
{
public:
	explicit CParagraph(DocumentData &documentData, const std::string& text);
	std::string GetText()const final;
	void SetText(const std::string& text) final;
	void SetTextData(const std::string& text);

private:
	DocumentData &m_documentData;
	std::string m_text;
};

