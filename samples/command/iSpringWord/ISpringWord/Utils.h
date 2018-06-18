#pragma once

class Utils
{
public:
	Utils() = delete;

	static std::string GetImagesDirName();

	static bool IsSlash(char c);
	static std::string StripTrailingSlash(std::string const& path);
	static std::string JoinPaths(std::string const& path1, std::string const& path2);
	static std::string StripFilename(std::string path);
	static std::string GetFilename(std::string path);
	static std::string GetExtension(std::string const& path);

	static bool TryCreateDir(std::string const& path);
	static bool KeepCreatingDir(std::string const& path, std::function<bool()> const& promptRetry);
	static bool TryCopyFile(std::string const& source, std::string const& destination);
	static bool TryRemoveDirectory(std::string const& path);

	static bool YesNoPrompt(std::string const& prompt);

	static bool KeepCreatingDirUserPrompt(std::string const& path);
};
