#include "stdafx.h"
#include "Utils.h"

namespace
{

constexpr char PATH_SLASH = '\\';

}

std::string Utils::GetImagesDirName()
{
	return "images";
}

void Utils::ValidateImageSize(int width, int height)
{
	if (width < 1 || height < 1 || width > 10000 || height > 10000)
	{
		throw std::runtime_error("image size is out of range");
	}
}

bool Utils::IsSlash(char c)
{
	return c == PATH_SLASH || c == '/';
}

std::string Utils::StripTrailingSlash(std::string const & path)
{
	return (path.empty() || !IsSlash(path.back()) ||
			path.size() == 1 || *(path.end() - 2) == ':')
		? path
		: path.substr(0, path.size() - 1);
}

std::string Utils::JoinPaths(std::string const & path1, std::string const & path2)
{
	if (path1.empty())
	{
		return StripTrailingSlash(path2);
	}
	if (path2.empty())
	{
		return StripTrailingSlash(path1);
	}
	return StripTrailingSlash(StripTrailingSlash(path1) + PATH_SLASH + path2);
}

std::string Utils::StripFilename(std::string path)
{
	path = StripTrailingSlash(path);
	const auto pos = path.find_last_of(PATH_SLASH);
	return pos == std::string::npos ? "" : path.substr(0, pos);
}

std::string Utils::GetFilename(std::string path)
{
	path = StripTrailingSlash(path);
	const auto pos = path.find_last_of(PATH_SLASH);

	if (pos == std::string::npos)
	{
		return path;
	}

	if (pos + 1 >= path.size())
	{
		return "";
	}

	return path.substr(pos + 1);
}

std::string Utils::GetExtension(std::string const& path)
{
	const auto pos = path.find_last_of('.');
	return pos == std::string::npos ? "" : path.substr(pos);
}

bool Utils::TryCreateDir(std::string const & path)
{
	return CreateDirectoryA(path.c_str(), NULL);
}

bool Utils::KeepCreatingDir(std::string const & path, std::function<bool()> const & promptRetry)
{
	for (;;)
	{
		if (TryCreateDir(path))
		{
			return true;
		}
		if (!promptRetry())
		{
			return false;
		}
	}
}

bool Utils::TryCopyFile(std::string const & source, std::string const & destination)
{
	return CopyFileA(source.c_str(), destination.c_str(), TRUE);
}

bool Utils::TryRemoveDirectory(std::string const & path)
{
	return RemoveDirectoryA(path.c_str());
}

bool Utils::YesNoPrompt(std::string const & prompt)
{
	for (;;)
	{
		std::cout << prompt << std::endl;

		std::string s;
		getline(std::cin, s);

		if (s == "y")
		{
			return true;
		}
		if (s == "n")
		{
			return false;
		}
	}
}

bool Utils::KeepCreatingDirUserPrompt(std::string const & path)
{
	return KeepCreatingDir(path, [&path]() {
		return YesNoPrompt("Could not create directory " + path + ". Try again?");
	});
}
