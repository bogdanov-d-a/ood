#pragma once

class IImageKeeper
{
public:
	virtual ~IImageKeeper() = default;

	virtual std::string GetPath() const = 0;
	virtual void KeepAlive() = 0;
};

using IImageKeeperPtr = std::shared_ptr<IImageKeeper>;
