#include "../libstreams/deps.h"
#include "../libstreams/FileInputStream.h"
#include "../libstreams/FileOutputStream.h"
#include "../libstreams/OutputStreamEncryptor.h"
#include "../libstreams/InputStreamDecryptor.h"
#include "../libstreams/OutputStreamCompressor.h"
#include "../libstreams/InputStreamDecompressor.h"

using IntCallback = std::function<void(int)>;
using VoidCallback = std::function<void()>;

namespace
{

enum class ArgParseState
{
	DEFAULT,
	ENCRYPT_KEY,
	DECRYPT_KEY,
};

void ParseArgs(int argc, char *argv[],
	IntCallback const& onEncrypt, IntCallback const& onDecrypt,
	VoidCallback const& onCompress, VoidCallback const& onDecompress)
{
	ArgParseState state = ArgParseState::DEFAULT;

	for (int curArgIndex = 1; curArgIndex < argc - 2; ++curArgIndex)
	{
		std::string curArg(argv[curArgIndex]);

		switch (state)
		{
		case ArgParseState::DEFAULT:
			if (curArg == "--encrypt")
			{
				state = ArgParseState::ENCRYPT_KEY;
			}
			else if (curArg == "--decrypt")
			{
				state = ArgParseState::DECRYPT_KEY;
			}
			else if (curArg == "--compress")
			{
				onCompress();
			}
			else if (curArg == "--decompress")
			{
				onDecompress();
			}
			else
			{
				throw std::invalid_argument(curArg);
			}
			break;

		case ArgParseState::ENCRYPT_KEY:
			onEncrypt(std::stoi(curArg));
			state = ArgParseState::DEFAULT;
			break;

		case ArgParseState::DECRYPT_KEY:
			onDecrypt(std::stoi(curArg));
			state = ArgParseState::DEFAULT;
			break;

		default:
			throw std::runtime_error("bad parser state");
		}
	}
}

}

int main(int argc, char *argv[])
{
	if (argc < 3)
	{
		std::cout << "Not enough arguments" << std::endl;
		return 0;
	}

	const std::string inputName(argv[argc - 2]);
	const std::string outputName(argv[argc - 1]);

	IInputDataStreamPtr input = std::make_unique<FileInputStream>(inputName);
	IOutputDataStreamPtr output = std::make_unique<FileOutputStream>(outputName);

	ParseArgs(argc, argv,
		[&output](int key) {
			output = std::make_unique<OutputStreamEncryptor>(std::move(output), key);
		},
		[&input](int key) {
			input = std::make_unique<InputStreamDecryptor>(std::move(input), key);
		},
		[&output]() {
			output = std::make_unique<OutputStreamCompressor>(std::move(output));
		},
		[&input]() {
			input = std::make_unique<InputStreamDecompressor>(std::move(input));
		}
	);

	constexpr std::streamsize bufferSize = 1024;
	std::vector<uint8_t> buffer(bufferSize);
	for (;;)
	{
		const auto readCount = input->ReadBlock(buffer.data(), bufferSize);
		output->WriteBlock(buffer.data(), readCount);
		if (readCount < bufferSize)
		{
			break;
		}
	}

	return 0;
}
