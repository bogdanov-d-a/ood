#include "../libstreams/deps.h"
#include "../libstreams/FileInputStream.h"
#include "../libstreams/FileOutputStream.h"

int main(int argc, char *argv[])
{
	if (argc < 2)
	{
		std::cout << "Not enough arguments" << std::endl;
		return 0;
	}

	const std::string inputName(argv[argc - 2]);
	const std::string outputName(argv[argc - 1]);

	FileInputStream input(inputName);
	FileOutputStream output(outputName);

	while (!input.IsEOF())
	{
		output.WriteByte(input.ReadByte());
	}

	return 0;
}
