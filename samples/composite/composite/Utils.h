#pragma once

#include <SFML/Graphics.hpp>
#include "CommonTypes.h"

class Utils
{
public:
	Utils() = delete;

	template<typename Value, typename Enumerator>
	static boost::optional<Value> GetCommonProperty(Enumerator const& enumerator)
	{
		boost::optional<Value> result;

		enumerator([&result](boost::optional<Value> const& value) {
			if (!value)
			{
				result = boost::none;
				return false;
			}

			if (!result)
			{
				result = value;
				return true;
			}

			if (result != value)
			{
				result = boost::none;
				return false;
			}

			return true;
		});

		return result;
	}

	static sf::Vector2f PointToSfmlVector(PointD const& point);
	static sf::Color ColorToSfmlColor(RGBAColor color);
};
