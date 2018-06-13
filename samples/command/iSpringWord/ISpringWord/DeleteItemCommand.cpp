#include "stdafx.h"
#include "DeleteItemCommand.h"

DeleteItemCommand::DeleteItemCommand(DocumentData & data, boost::optional<size_t> const & position)
	: m_data(data)
	, m_position(position)
{
}

void DeleteItemCommand::DoExecute()
{
	assert(!m_deletedItem.is_initialized());
	m_deletedItem = m_data.DeleteItem(m_position);
}

void DeleteItemCommand::DoUnexecute()
{
	assert(m_deletedItem.is_initialized());
	m_data.InsertItem(std::move(*m_deletedItem), m_position);
	m_deletedItem = boost::none;
}
