#pragma once

class Utils
{
public:
	Utils() = delete;

	static std::vector<uint8_t> GenerateReplaceTable(uint32_t key);
	static std::vector<uint8_t> InvertReplaceTable(std::vector<uint8_t> const& table);
};
