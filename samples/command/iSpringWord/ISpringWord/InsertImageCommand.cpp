#include "stdafx.h"
#include "InsertImageCommand.h"

InsertImageCommand::InsertImageCommand(DocumentData & data, std::string const & path, int width, int height, const boost::optional<size_t>& position)
	: m_data(data)
	, m_executeData(CreateData(path, width, height))
	, m_position(position)
{
}

void InsertImageCommand::DoExecute()
{
	if (auto createData = boost::get<CreateData>(&m_executeData))
	{
		m_data.InsertImage(createData->path, createData->width, createData->height, m_position);
	}
	else if (auto image = boost::get<std::shared_ptr<CImage>>(&m_executeData))
	{
		m_data.InsertItem(std::move(*image), m_position);
	}
	else
	{
		assert(false);
	}
	m_executeData = Empty();
}

void InsertImageCommand::DoUnexecute()
{
	assert(m_executeData.type() == typeid(Empty));

	auto item = m_data.DeleteItem(m_position);

	auto imagePtr = boost::get<std::shared_ptr<CImage>>(&item);
	assert(imagePtr);

	m_executeData = std::move(*imagePtr);
}
