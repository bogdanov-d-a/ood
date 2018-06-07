#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class DeleteItemCommand : public CAbstractCommand
{
public:
	explicit DeleteItemCommand(DocumentData &data, boost::optional<size_t> const& position);

private:
	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_data;
	const boost::optional<size_t> m_position;
	boost::optional<DocumentData::ItemData> m_deletedItem;
};
