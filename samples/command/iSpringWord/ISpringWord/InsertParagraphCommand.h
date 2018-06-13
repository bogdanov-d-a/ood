#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class InsertParagraphCommand : public CAbstractCommand
{
public:
	explicit InsertParagraphCommand(DocumentData &data, std::string const& text, const boost::optional<size_t>& position);

private:
	struct Empty
	{};

	using ExecuteData = boost::variant<Empty, std::string, std::shared_ptr<CParagraph>>;

	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_data;
	ExecuteData m_executeData;
	const boost::optional<size_t> m_position;
};
