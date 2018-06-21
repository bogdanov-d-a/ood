#include "stdafx.h"
#include "../LibISpringWord/DocumentData.h"

namespace
{

const auto onCreateCommandThrowMock = [](ICommandPtr&&) { throw std::runtime_error("mock should not be called"); };
const auto onCopyImageThrowMock = [](std::string const&) -> std::string { throw std::runtime_error("not implemented"); };

const auto onCopyImageMock = [](std::string const& s) { return "Copy of " + s; };

}

BOOST_AUTO_TEST_CASE(EmptyWhenCreated)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock);
	BOOST_CHECK(data.GetItemsCount() == 0);
}

BOOST_AUTO_TEST_CASE(CanInsertParagraph)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock);
	data.InsertParagraph("test data");
	BOOST_CHECK(data.GetItemsCount() == 1);
	auto p = data.GetItem(0).GetParagraph();
	BOOST_CHECK(p);
	BOOST_CHECK(p->GetText() == "test data");
}

BOOST_AUTO_TEST_CASE(CanInsertImage)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageMock);
	data.InsertImage("img_path.jpg", 640, 480);
}
