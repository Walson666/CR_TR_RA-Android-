// dnSpy decompiler from Assembly-CSharp.dll class: EasingG
using System;
using UnityEngine;

public class EasingG
{
	public static float EaseNone(float t, float b, float c, float d)
	{
		return c * t / d + b;
	}

	public static float EaseInQuad(float t, float b, float c, float d)
	{
		return c * (t /= d) * t + b;
	}

	public static float EaseOutQuad(float t, float b, float c, float d)
	{
		return -c * (t /= d) * (t - 2f) + b;
	}

	public static float EaseInOutQuad(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t + b;
		}
		return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
	}

	public static float EaseOutInQuad(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutQuad(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInQuad(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInCubic(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t + b;
	}

	public static float EaseOutCubic(float t, float b, float c, float d)
	{
		t = t / d - 1f; return c * ((t ) * t * t + 1f) + b;
	}

	public static float EaseInOutCubic(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t + 2f) + b;
	}

	public static float EaseOutInCubic(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutCubic(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInCubic(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInQuart(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t + b;
	}

	public static float EaseOutQuart(float t, float b, float c, float d)
	{
		t = t / d - 1f; return -c * ((t ) * t * t * t - 1f) + b;
	}

	public static float EaseInOutQuart(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t + b;
		}
		return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
	}

	public static float EaseOutInQuart(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutQuart(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInQuart(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInQuint(float t, float b, float c, float d)
	{
		return c * (t /= d) * t * t * t * t + b;
	}

	public static float EaseOutQuint(float t, float b, float c, float d)
	{
		t = t / d - 1f; return c * ((t ) * t * t * t * t + 1f) + b;
	}

	public static float EaseInOutQuint(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * t * t * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
	}

	public static float EaseOutInQuint(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutQuint(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInQuint(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInSine(float t, float b, float c, float d)
	{
		return -c * Mathf.Cos(t / d * 1.57079637f) + c + b;
	}

	public static float EaseOutSine(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t / d * 1.57079637f) + b;
	}

	public static float EaseInOutSine(float t, float b, float c, float d)
	{
		return -c / 2f * (Mathf.Cos(3.14159274f * t / d) - 1f) + b;
	}

	public static float EaseOutInSine(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutSine(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInSine(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInExpo(float t, float b, float c, float d)
	{
		return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b - c * 0.001f) : b;
	}

	public static float EaseOutExpo(float t, float b, float c, float d)
	{
		return (t != d) ? (c * 1.001f * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
	}

	public static float EaseInOutExpo(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if (t == d)
		{
			return b + c;
		}
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b - c * 0.0005f;
		}
		return c / 2f * 1.0005f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
	}

	public static float EaseOutInExpo(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutExpo(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInExpo(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInCirc(float t, float b, float c, float d)
	{
		return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
	}

	public static float EaseOutCirc(float t, float b, float c, float d)
	{
		t = t / d - 1f; return c * Mathf.Sqrt(1f - (t ) * t) + b;
	}

	public static float EaseInOutCirc(float t, float b, float c, float d)
	{
		if ((t /= d / 2f) < 1f)
		{
			return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
		}
		return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
	}

	public static float EaseOutInCirc(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutCirc(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInCirc(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInElastic(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = 0f;
		float num3;
		if (num2 == 0f || num2 < Mathf.Abs(c))
		{
			num2 = c;
			num3 = num / 4f;
		}
		else
		{
			num3 = num / 6.28318548f * Mathf.Asin(c / num2);
		}
		return -(num2 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num3) * 6.28318548f / num)) + b;
	}

	public static float EaseOutElastic(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if ((t /= d) == 1f)
		{
			return b + c;
		}
		float num = d * 0.3f;
		float num2 = 0f;
		float num3;
		if (num2 == 0f || num2 < Mathf.Abs(c))
		{
			num2 = c;
			num3 = num / 4f;
		}
		else
		{
			num3 = num / 6.28318548f * Mathf.Asin(c / num2);
		}
		return num2 * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num3) * 6.28318548f / num) + c + b;
	}

	public static float EaseInOutElastic(float t, float b, float c, float d)
	{
		if (t == 0f)
		{
			return b;
		}
		if ((t /= d / 2f) == 2f)
		{
			return b + c;
		}
		float num = d * 0.450000018f;
		float num2 = 0f;
		float num3;
		if (num2 == 0f || num2 < Mathf.Abs(c))
		{
			num2 = c;
			num3 = num / 4f;
		}
		else
		{
			num3 = num / 6.28318548f * Mathf.Asin(c / num2);
		}
		if (t < 1f)
		{
			return -0.5f * (num2 * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num3) * 6.28318548f / num)) + b;
		}
		return num2 * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num3) * 6.28318548f / num) * 0.5f + c + b;
	}

	public static float EaseOutInElastic(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutElastic(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInElastic(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInBack(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		return c * (t /= d) * t * ((num + 1f) * t - num) + b;
	}

	public static float EaseOutBack(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		t = t / d - 1f; return c * ((t ) * t * ((num + 1f) * t + num) + 1f) + b;
	}

	public static float EaseInOutBack(float t, float b, float c, float d)
	{
		float num = 1.70158f;
		if ((t /= d / 2f) < 1f)
		{
			return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
		}
		return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
	}

	public static float EaseOutInBack(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutBack(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInBack(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public static float EaseInBounce(float t, float b, float c, float d)
	{
		return c - EasingG.EaseOutBounce(d - t, 0f, c, d) + b;
	}

	public static float EaseOutBounce(float t, float b, float c, float d)
	{
		if ((t /= d) < 0.363636374f)
		{
			return c * (7.5625f * t * t) + b;
		}
		if (t < 0.727272749f)
		{
			return c * (7.5625f * (t -= 0.545454562f) * t + 0.75f) + b;
		}
		if (t < 0.909090936f)
		{
			return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
		}
		return c * (7.5625f * (t -= 0.954545438f) * t + 0.984375f) + b;
	}

	public static float EaseInOutBounce(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseInBounce(t * 2f, 0f, c, d) * 0.5f + b;
		}
		return EasingG.EaseOutBounce(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
	}

	public static float EaseOutInBounce(float t, float b, float c, float d)
	{
		if (t < d / 2f)
		{
			return EasingG.EaseOutBounce(t * 2f, b, c / 2f, d);
		}
		return EasingG.EaseInBounce(t * 2f - d, b + c / 2f, c / 2f, d);
	}

	public delegate float EaseFunction(float t, float b, float c, float d);
}
