#pragma once

class ReplaceTable
{
public:
	explicit ReplaceTable(uint32_t key);

	uint8_t Map(uint8_t byte) const;
	uint8_t MapBack(uint8_t byte) const;

private:
	const std::vector<uint8_t> m_data;
	const std::vector<uint8_t> m_invertedData;
};
