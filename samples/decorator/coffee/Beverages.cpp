#include "Beverages.h"

const std::map<TeaType, std::string> TEA_TYPE_TO_NAME = {
	{ TeaType::Red, "Red" },
	{ TeaType::Yellow, "Yellow" },
	{ TeaType::Green, "Green" },
	{ TeaType::Blue, "Blue" },
};

const std::map<MilkshakeSize, std::pair<std::string, double>> MILKSHAKE_SIZE_TO_DATA = {
	{ MilkshakeSize::Small, { "Small", 50 } },
	{ MilkshakeSize::Medium, { "Medium", 60 } },
	{ MilkshakeSize::Large, { "Large", 80 } },
};
