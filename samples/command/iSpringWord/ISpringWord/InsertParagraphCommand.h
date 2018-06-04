#pragma once

#include "AbstractCommand.h"
#include "IDocument.h"

class InsertParagraphCommand : public CAbstractCommand
{
public:
	using OnInsert = std::function<void(std::string const&, const boost::optional<size_t>&)>;
	using OnRemove = std::function<void(const boost::optional<size_t>&)>;

	explicit InsertParagraphCommand(std::string const& text, const boost::optional<size_t>& position,
		OnInsert const& onInsert, OnRemove const& onRemove);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	std::string m_text;
	boost::optional<size_t> m_position;
	OnInsert m_onInsert;
	OnRemove m_onRemove;
};
