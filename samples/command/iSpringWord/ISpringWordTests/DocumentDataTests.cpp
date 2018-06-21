#include "stdafx.h"
#include "../LibISpringWord/DocumentData.h"

namespace
{

const auto onCreateCommandThrowMock = [](ICommandPtr&&) { throw std::runtime_error("mock should not be called"); };
const auto onCopyImageThrowMock = [](std::string const&) -> std::string { throw std::runtime_error("not implemented"); };
const auto imageKeeperCreatorThrowMock = [](std::string const&) -> IImageKeeperPtr { throw std::runtime_error("not implemented"); };

class MockImageKeeper : public IImageKeeper
{
public:
	explicit MockImageKeeper(std::string const& path)
		: m_path(path)
	{}

	std::string GetPath() const final
	{
		return m_path;
	};

	void KeepAlive() final
	{
		throw std::runtime_error("not implemented");
	};

public:
	std::string m_path;
};

const auto onCopyImageMock = [](std::string const& s) { return "copy of " + s; };
const auto imageKeeperCreatorMock = [](std::string const& s) { return std::make_shared<MockImageKeeper>(s); };

template<typename T = std::exception>
bool CatchException(std::function<void()> const fn)
{
	bool caught = false;
	try
	{
		fn();
	}
	catch (T&)
	{
		caught = true;
	}
	return caught;
}

}

BOOST_AUTO_TEST_CASE(EmptyWhenCreated)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	BOOST_CHECK(data.GetItemsCount() == 0);
}

BOOST_AUTO_TEST_CASE(CanInsertParagraph)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	data.InsertParagraph("test data");
	BOOST_CHECK(data.GetItemsCount() == 1);
	auto p = data.GetItem(0).GetParagraph();
	BOOST_CHECK(p);
	BOOST_CHECK(p->GetText() == "test data");
}

BOOST_AUTO_TEST_CASE(CanInsertImage)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageMock, imageKeeperCreatorMock);
	data.InsertImage("img_path.jpg", 640, 480);
	BOOST_CHECK(data.GetItemsCount() == 1);
	auto i = data.GetItem(0).GetImage();
	BOOST_CHECK(i);
	BOOST_CHECK(i->GetPath() == "copy of img_path.jpg");
	BOOST_CHECK(i->GetWidth() == 640);
	BOOST_CHECK(i->GetHeight() == 480);
}

BOOST_AUTO_TEST_CASE(CanInsertToPosition)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	data.InsertParagraph("1");
	data.InsertParagraph("2", 0);
	data.InsertParagraph("3", 1);
	data.InsertParagraph("4", 2);

	BOOST_CHECK(data.GetItemsCount() == 4);
	BOOST_CHECK(data.GetItem(0).GetParagraph()->GetText() == "2");
	BOOST_CHECK(data.GetItem(1).GetParagraph()->GetText() == "3");
	BOOST_CHECK(data.GetItem(2).GetParagraph()->GetText() == "4");
	BOOST_CHECK(data.GetItem(3).GetParagraph()->GetText() == "1");
}

BOOST_AUTO_TEST_CASE(CantInsertToWrongPosition)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	BOOST_CHECK(CatchException([&]() { data.InsertParagraph("", 1); }));

	data.InsertParagraph("test data", 0);
	BOOST_CHECK(CatchException([&]() { data.InsertParagraph("", 2); }));

	data.InsertParagraph("test data", 1);
	BOOST_CHECK(CatchException([&]() { data.InsertParagraph("", 3); }));
}

BOOST_AUTO_TEST_CASE(RemoveItemAtPosition)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	data.InsertParagraph("1");
	data.InsertParagraph("2");
	data.InsertParagraph("3");
	data.InsertParagraph("4");

	BOOST_CHECK(CatchException([&]() { data.DeleteItem(4); }));

	data.DeleteItem(2);
	BOOST_CHECK(data.GetItemsCount() == 3);
	BOOST_CHECK(data.GetItem(0).GetParagraph()->GetText() == "1");
	BOOST_CHECK(data.GetItem(1).GetParagraph()->GetText() == "2");
	BOOST_CHECK(data.GetItem(2).GetParagraph()->GetText() == "4");

	BOOST_CHECK(CatchException([&]() { data.DeleteItem(4); }));
	BOOST_CHECK(CatchException([&]() { data.DeleteItem(3); }));

	data.DeleteItem(0);
	BOOST_CHECK(data.GetItemsCount() == 2);
	BOOST_CHECK(data.GetItem(0).GetParagraph()->GetText() == "2");
	BOOST_CHECK(data.GetItem(1).GetParagraph()->GetText() == "4");
}

BOOST_AUTO_TEST_CASE(CanUseTitle)
{
	DocumentData data(onCreateCommandThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	BOOST_CHECK(data.GetTitle() == "");
	data.SetTitle("test title");
	BOOST_CHECK(data.GetTitle() == "test title");
}
