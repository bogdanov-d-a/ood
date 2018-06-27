#pragma once

template <typename T>
struct Point
{
	T x;
	T y;
};

template <typename T>
struct Rect
{
	T left;
	T top;
	T width;
	T height;

	T GetRight() const
	{
		return left + width;
	}

	T GetBottom() const
	{
		return top + height;
	}

	void SetRight(T right)
	{
		width += right - GetRight();
	}

	void SetBottom(T bottom)
	{
		height += bottom - GetBottom();
	}
};

typedef Point<double> PointD;
typedef Rect<double> RectD;

typedef uint32_t RGBAColor;
