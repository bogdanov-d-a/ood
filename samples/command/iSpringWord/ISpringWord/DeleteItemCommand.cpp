#include "stdafx.h"
#include "DeleteItemCommand.h"

DeleteItemCommand::DeleteItemCommand(DocumentData & data, boost::optional<size_t> const & position)
	: m_data(data)
	, m_position(position)
{
}

void DeleteItemCommand::DoExecute()
{
	m_deletedItem = m_data.GetItemData(m_position);
	m_data.DeleteItem(m_position);
}

void DeleteItemCommand::DoUnexecute()
{
	m_data.InsertItem(std::move(*m_deletedItem), m_position);
}
