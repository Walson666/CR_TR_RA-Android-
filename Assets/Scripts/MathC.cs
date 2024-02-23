// dnSpy decompiler from Assembly-CSharp.dll class: MathC
using System;
using UnityEngine;

public class MathC
{
	public static float RateLimit(float prevValue, float value, float posRateLimit, float negRateLimit)
	{
		if (value - prevValue > posRateLimit)
		{
			return prevValue + posRateLimit;
		}
		if (value - prevValue < -negRateLimit)
		{
			return prevValue - negRateLimit;
		}
		return value;
	}

	public static MathC.ClipStatus GetClipStatus(BoundsG a, BoundsG b)
	{
		if (a.Contains(b))
		{
			return MathC.ClipStatus.Inside;
		}
		if (a.Intersect(b))
		{
			return MathC.ClipStatus.Overlapping;
		}
		return MathC.ClipStatus.Outside;
	}

	public static bool RectContains(Rect r1, Rect r2)
	{
		return r1.xMin <= r2.xMin && r1.yMin <= r2.yMin && r1.xMax >= r2.xMax && r1.yMax >= r2.yMax;
	}

	public static bool RectIntersect(Rect r1, Rect r2)
	{
		return r1.xMax >= r2.xMin && r1.yMax >= r2.yMin && r1.xMin <= r2.xMax && r1.yMin <= r2.yMax;
	}

	public static MathC.ClipStatus GetClipStatus(Rect a, Rect b)
	{
		if (MathC.RectContains(a, b))
		{
			return MathC.ClipStatus.Inside;
		}
		if (MathC.RectIntersect(a, b))
		{
			return MathC.ClipStatus.Overlapping;
		}
		return MathC.ClipStatus.Outside;
	}

	public static float ToDegrees = 57.2957764f;

	public static float ToRadians = 0.0174532924f;

	public static float Epsilon = 1E-05f;

	public static float SqrEpsilon = MathC.Epsilon * MathC.Epsilon;

	public enum ClipStatus
	{
		Inside,
		Overlapping,
		Outside
	}
}
