#include "deps.h"
#include "Utils.h"

std::vector<uint8_t> Utils::GenerateReplaceTable(uint32_t key)
{
	std::vector<uint8_t> result(UINT8_MAX + 1);
	std::iota(result.begin(), result.end(), 0);

	std::mt19937 mt(key);
	std::shuffle(result.begin(), result.end(), std::move(mt));

	return result;
}

std::vector<uint8_t> Utils::InvertReplaceTable(std::vector<uint8_t> const & table)
{
	std::vector<uint8_t> result(table.size());

	for (uint8_t i = 0; i < table.size(); ++i)
	{
		result[table[i]] = i;
	}

	return result;
}
