#pragma once

class Utils
{
public:
	Utils() = delete;

	static std::string GetImagesDirName();

	static bool TryCreateDir(std::string const& path);
	static bool KeepCreatingDir(std::string const& path, std::function<bool()> const& promptRetry);

	static bool YesNoPrompt(std::string const& prompt);

	static bool KeepCreatingDirUserPrompt(std::string const& path);
};
