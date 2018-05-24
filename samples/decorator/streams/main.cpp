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

struct EncryptAction
{
	explicit EncryptAction(int key)
		: key(key)
	{}

	int key = 0;
};

class CompressAction
{
};

using OutputAction = boost::variant<EncryptAction, CompressAction>;

class ApplyOutputActionVisitor : boost::static_visitor<IOutputDataStreamPtr>
{
public:
	explicit ApplyOutputActionVisitor(IOutputDataStreamPtr &&stream)
		: m_stream(std::move(stream))
	{
	}

	IOutputDataStreamPtr operator()(EncryptAction const& action)
	{
		return std::make_unique<OutputStreamEncryptor>(std::move(m_stream), action.key);
	}

	IOutputDataStreamPtr operator()(CompressAction const&)
	{
		return std::make_unique<OutputStreamCompressor>(std::move(m_stream));
	}

private:
	IOutputDataStreamPtr m_stream;
};

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

	{
		std::vector<OutputAction> outputActions;

		ParseArgs(argc, argv,
			[&outputActions](int key) {
				outputActions.emplace_back(EncryptAction(key));
			},
			[&input](int key) {
				input = std::make_unique<InputStreamDecryptor>(std::move(input), key);
			},
			[&outputActions]() {
				outputActions.emplace_back(CompressAction());
			},
			[&input]() {
				input = std::make_unique<InputStreamDecompressor>(std::move(input));
			}
		);

		for (auto it = outputActions.rbegin(); it != outputActions.rend(); ++it)
		{
			ApplyOutputActionVisitor visitor(std::move(output));
			output = boost::apply_visitor(visitor, *it);
		}
	}

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
