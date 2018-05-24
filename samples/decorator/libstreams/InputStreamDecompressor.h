#pragma once

#include "IInputDataStream.h"

class InputStreamDecompressor : public IInputDataStream
{
public:
	explicit InputStreamDecompressor(IInputDataStreamPtr && stream);

	bool IsEOF() final;
	uint8_t ReadByte() final;
	std::streamsize ReadBlock(void * dstBuffer, std::streamsize size) final;

private:
	struct RlePair
	{
		RlePair(uint8_t count, uint8_t byte)
			: count(count)
			, byte(byte)
		{
		}

		uint8_t count = 0;
		uint8_t byte = 0;
	};

	void TryReadBuffer();

	IInputDataStreamPtr m_stream;
	boost::optional<RlePair> m_buffer;
};
