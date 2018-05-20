#include "deps.h"
#include "ReplaceTable.h"

namespace
{

std::vector<uint8_t> GenerateReplaceTable(uint32_t key)
{
	std::vector<uint8_t> result(UINT8_MAX + 1);
	std::iota(result.begin(), result.end(), 0);

	std::mt19937 mt(key);
	std::shuffle(result.begin(), result.end(), std::move(mt));

	return result;
}

std::vector<uint8_t> InvertReplaceTable(std::vector<uint8_t> const& table)
{
	std::vector<uint8_t> result(table.size());

	for (uint8_t i = 0; i < table.size(); ++i)
	{
		result[table[i]] = i;
		if (i == UINT8_MAX)
		{
			break;
		}
	}

	return result;
}

}

ReplaceTable::ReplaceTable(uint32_t key)
	: m_data(GenerateReplaceTable(key))
	, m_invertedData(InvertReplaceTable(m_data))
{
}

uint8_t ReplaceTable::Map(uint8_t byte) const
{
	return m_data[byte];
}

uint8_t ReplaceTable::MapBack(uint8_t byte) const
{
	return m_invertedData[byte];
}
