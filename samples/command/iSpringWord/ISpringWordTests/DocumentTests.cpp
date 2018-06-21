#include "stdafx.h"
#include "../LibISpringWord/Document.h"

namespace
{

const auto onSaveImageThrowMock = [](std::string const&, std::string const&) { throw std::runtime_error("mock should not be called"); };
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

}

BOOST_AUTO_TEST_CASE(TestSetTitleUndoRedo)
{
	CDocument document(onSaveImageThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	document.SetTitle("t1");
	document.SetTitle("t2");

	document.Undo();
	BOOST_CHECK(document.GetTitle() == "t1");
	document.Undo();
	BOOST_CHECK(document.GetTitle() == "");

	document.Redo();
	BOOST_CHECK(document.GetTitle() == "t1");
	document.Redo();
	BOOST_CHECK(document.GetTitle() == "t2");
}

BOOST_AUTO_TEST_CASE(TestInsertParagraphUndoRedo)
{
	CDocument document(onSaveImageThrowMock, onCopyImageThrowMock, imageKeeperCreatorThrowMock);
	document.InsertParagraph("p1");
	document.InsertParagraph("p2", 0);

	BOOST_CHECK(document.GetItemsCount() == 2);
	BOOST_CHECK(document.GetItem(0).GetParagraph()->GetText() == "p2");
	BOOST_CHECK(document.GetItem(1).GetParagraph()->GetText() == "p1");

	document.Undo();
	BOOST_CHECK(document.GetItemsCount() == 1);
	BOOST_CHECK(document.GetItem(0).GetParagraph()->GetText() == "p1");

	document.Undo();
	BOOST_CHECK(document.GetItemsCount() == 0);

	document.Redo();
	BOOST_CHECK(document.GetItemsCount() == 1);
	BOOST_CHECK(document.GetItem(0).GetParagraph()->GetText() == "p1");

	document.Redo();
	BOOST_CHECK(document.GetItemsCount() == 2);
	BOOST_CHECK(document.GetItem(0).GetParagraph()->GetText() == "p2");
	BOOST_CHECK(document.GetItem(1).GetParagraph()->GetText() == "p1");
}
