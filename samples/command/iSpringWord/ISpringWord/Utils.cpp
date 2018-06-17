#include "stdafx.h"
#include "Utils.h"

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
