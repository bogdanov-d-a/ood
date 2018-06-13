#pragma once

#include "AbstractCommand.h"
#include "DocumentData.h"

class InsertImageCommand : public CAbstractCommand
{
public:
	explicit InsertImageCommand(DocumentData &data, std::string const& path, int width, int height, const boost::optional<size_t>& position);

private:
	struct Empty
	{};

	struct CreateData
	{
		CreateData(std::string const& path, int width, int height)
			: path(path)
			, width(width)
			, height(height)
		{}

		std::string path;
		int width;
		int height;
	};

	using ExecuteData = boost::variant<Empty, CreateData, std::shared_ptr<CImage>>;

	void DoExecute() final;
	void DoUnexecute() final;

	DocumentData &m_data;
	ExecuteData m_executeData;
	const boost::optional<size_t> m_position;
};
