// ISpringWord.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Menu.h"
#include "Document.h"

using namespace std;
using namespace std::placeholders;

namespace
{

std::string GetPathFromIndex(unsigned index, std::string const& root, std::string const& ext)
{
	return root + "\\img" + std::to_string(index) + ext;
}

std::string GetExtension(std::string const& path)
{
	size_t dotIndex = path.find_last_of('.');
	if (dotIndex == std::string::npos)
	{
		return "";
	}
	return path.substr(dotIndex);
}

std::string GetClonePath(std::string const& path, unsigned index, std::string const& root)
{
	return GetPathFromIndex(index, root, GetExtension(path));
}

std::string GetTempPathWrapper()
{
	constexpr int bufferCapacity = 1024;
	char buffer[bufferCapacity];

	const auto pathLength = GetTempPathA(bufferCapacity, buffer);
	if (pathLength == 0)
	{
		throw std::invalid_argument("GetTempPathA error");
	}

	std::string result(pathLength, '\0');
	for (size_t i = 0; i < pathLength; ++i)
	{
		result[i] = buffer[i];
	}
	return result;
}

class CEditor
{
public:
	CEditor()  //-V730
		: m_document(make_unique<CDocument>(
			[this](ImageKeeperPtr const&) {},
			[this](std::string const& path) {
				const auto clonePath = GetClonePath(path, m_imageIndex++, m_imgPath);
				if (!CopyFileA(path.c_str(), clonePath.c_str(), TRUE))
				{
					throw std::runtime_error("CopyFileA failed");
				}
				return clonePath;
			}
		))
		, m_imgPath(GetTempPathWrapper() + "\\images")
	{
		if (!CreateDirectoryA(m_imgPath.c_str(), NULL))
		{
			throw std::runtime_error("CreateDirectoryA failed");
		}

		m_menu.AddItem("Help", "Help", [this](istream&) { m_menu.ShowInstructions(); });
		m_menu.AddItem("Exit", "Exit", [this](istream&) { m_menu.Exit(); });
		AddMenuItem("SetTitle", "Changes title. Args: <new title>", &CEditor::SetTitle);
		m_menu.AddItem("List", "Show document", bind(&CEditor::List, this, _1));
		AddMenuItem("Undo", "Undo command", &CEditor::Undo);
		AddMenuItem("Redo", "Redo undone command", &CEditor::Redo);
		AddMenuItem("Save", "Save document <path>", &CEditor::Save);
		AddMenuItem("InsertParagraph", "Inserts paragraph. Args: <position>|end <new text>", &CEditor::InsertParagraph);
		AddMenuItem("InsertImage", "Inserts image. Args: <position>|end <width> <height> <path>", &CEditor::InsertImage);
		AddMenuItem("DeleteItem", "Deletes item. Args: <position>", &CEditor::DeleteItem);
		AddMenuItem("ReplaceText", "Replaces text. Args <position> <text>", &CEditor::ReplaceText);
		AddMenuItem("ResizeImage", "Resizes image. Args <position> <width> <height>", &CEditor::ResizeImage);
	}

	~CEditor()
	{
		m_document.reset();  // delete images before working dir
		if (!RemoveDirectoryA(m_imgPath.c_str()))
		{
			std::cerr << "RemoveDirectoryA failed" << std::endl;
		}
	}

	void Start()
	{
		m_menu.Run();
	}

private:
	// Указатель на метод класса CEditor, принимающий istream& и возвращающий void
	typedef void (CEditor::*MenuHandler)(istream & in);

	void AddMenuItem(const string & shortcut, const string & description, MenuHandler handler)
	{
		m_menu.AddItem(shortcut, description, bind(handler, this, _1));
	}

	void SetTitle(istream & in)
	{
		in >> ws;
		string title;
		getline(in, title);
		m_document->SetTitle(title);
	}

	void InsertParagraph(istream & in)
	{
		string posStr;
		in >> posStr;

		boost::optional<size_t> pos;
		if (posStr != "end")
		{
			pos = std::stoi(posStr);
		}

		in >> ws;
		string text;
		getline(in, text);

		m_document->InsertParagraph(text, pos);
	}

	void InsertImage(istream & in)
	{
		string posStr;
		in >> posStr;

		boost::optional<size_t> pos;
		if (posStr != "end")
		{
			pos = std::stoi(posStr);
		}

		int width = 0;
		in >> width;

		int height = 0;
		in >> height;

		in >> ws;
		string path;
		getline(in, path);

		m_document->InsertImage(path, width, height, pos);
	}

	void ReplaceText(istream & in)
	{
		size_t pos;
		in >> pos;

		in >> ws;
		string text;
		getline(in, text);

		m_document->GetItem(pos).GetParagraph()->SetText(text);
	}

	void ResizeImage(istream & in)
	{
		size_t pos;
		in >> pos;

		int width = 0;
		in >> width;

		int height = 0;
		in >> height;

		m_document->GetItem(pos).GetImage()->Resize(width, height);
	}

	void DeleteItem(istream & in)
	{
		size_t pos;
		in >> pos;
		m_document->DeleteItem(pos);
	}

	void List(istream &)
	{
		cout << "-------------" << endl;
		cout << "Title: " << m_document->GetTitle() << endl;
		for (size_t i = 0; i < m_document->GetItemsCount(); ++i)
		{
			cout << std::to_string(i) << ". ";

			auto &item = m_document->GetItem(i);
			if (auto &paragraph = item.GetParagraph())
			{
				cout << "Paragraph: " << m_document->GetItem(i).GetParagraph()->GetText();
			}
			else if (auto &image = item.GetImage())
			{
				cout << "Image: " << image->GetWidth() << " " << image->GetHeight() << " " << image->GetPath();
			}
			else
			{
				assert(false);
			}

			cout << std::endl;
		}
		cout << "-------------" << endl;
	}

	void Undo(istream &)
	{
		if (m_document->CanUndo())
		{
			m_document->Undo();
		}
		else
		{
			cout << "Can't undo" << endl;
		}
	}

	void Redo(istream &)
	{
		if (m_document->CanRedo())
		{
			m_document->Redo();
		}
		else
		{
			cout << "Can't redo" << endl;
		}
	}

	void Save(istream & in)
	{
		in >> ws;
		string path;
		getline(in, path);

		m_document->Save(path);
	}

	CMenu m_menu;
	unique_ptr<IDocument> m_document;
	const std::string m_imgPath;
	unsigned m_imageIndex = 0;
};

}

int main()
{
	CEditor editor;
	editor.Start();
	return 0;
}

