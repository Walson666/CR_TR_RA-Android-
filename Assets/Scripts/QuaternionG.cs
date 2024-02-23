// dnSpy decompiler from Assembly-CSharp.dll class: QuaternionG
using System;
using UnityEngine;

public struct QuaternionG
{
	public QuaternionG(float _x, float _y, float _z, float _w)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
		this.w = _w;
	}

	public QuaternionG inverse
	{
		get
		{
			return new QuaternionG(-this.x, -this.y, -this.z, this.w);
		}
	}

	public QuaternionG normalized
	{
		get
		{
			float num = this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
			if (num < MathC.SqrEpsilon)
			{
				return new QuaternionG(0f, 0f, 0f, 1f);
			}
			num = 1f / Mathf.Sqrt(num);
			return new QuaternionG(this.x * num, this.y * num, this.z * num, this.w * num);
		}
	}

	public AngleAxisG angleAxis
	{
		get
		{
			QuaternionG quaternionG = this;
			if (this.w > 1f)
			{
				quaternionG = this.normalized;
			}
			float num = 2f * Mathf.Acos(quaternionG.w);
			float num2 = Mathf.Sqrt(1f - quaternionG.w * quaternionG.w);
			float ax;
			float ay;
			float az;
			if (num2 < MathC.Epsilon)
			{
				ax = quaternionG.x;
				ay = quaternionG.y;
				az = quaternionG.z;
			}
			else
			{
				float num3 = 1f / num2;
				ax = quaternionG.x * num3;
				ay = quaternionG.y * num3;
				az = quaternionG.z * num3;
			}
			return new AngleAxisG(num * MathC.ToDegrees, ax, ay, az);
		}
		set
		{
			this.SetAngleAxis(value.angle, value.axis);
		}
	}

	public static QuaternionG Integrate(QuaternionG rotation, Vector3G angularVelocity, float dt)
	{
		QuaternionG q = new QuaternionG(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0f) * rotation;
		q.ScaleBy(0.5f * dt);
		QuaternionG result = rotation + q;
		result.Normalize();
		return result;
	}

	public static QuaternionG AngleAxis(float angle, Vector3G axis)
	{
		angle *= MathC.ToRadians * 0.5f;
		Vector3G normalized = axis.normalized;
		float num = Mathf.Sin(angle);
		float num2 = Mathf.Cos(angle);
		return new QuaternionG(normalized.x * num, normalized.y * num, normalized.z * num, num2);
	}

	public static QuaternionG LookRotation(Vector3G forward, Vector3G up)
	{
		forward.Normalize();
		Vector3G normalized = Vector3G.Cross(up.normalized, forward).normalized;
		up = Vector3G.Cross(forward, normalized);
		float num = Mathf.Sqrt(1f + normalized.x + up.y + forward.z) * 0.5f;
		float num2 = 1f / (4f * num);
		float num3 = (forward.y - up.z) * num2;
		float num4 = (normalized.z - forward.x) * num2;
		float num5 = (up.x - normalized.y) * num2;
		return new QuaternionG(num3, num4, num5, num);
	}

	public static QuaternionG LookRotation(Vector3G forward)
	{
		return QuaternionG.LookRotation(forward, Vector3G.up);
	}

	public static QuaternionG Slerp(QuaternionG q0, QuaternionG q1, float t)
	{
		float num = q0.w;
		float num2 = q0.x;
		float num3 = q0.y;
		float num4 = q0.z;
		float num5 = q1.w;
		float num6 = q1.x;
		float num7 = q1.y;
		float num8 = q1.z;
		float num9 = num * num5 + num2 * num6 + num3 * num7 + num4 * num8;
		if (num9 < 0f)
		{
			num9 = -num9;
			num5 = -num5;
			num6 = -num6;
			num7 = -num7;
			num8 = -num8;
		}
		if (num9 < 0.95f)
		{
			float num10 = Mathf.Acos(num9);
			float num11 = 1f / Mathf.Sin(num10);
			float num12 = Mathf.Sin(num10 * (1f - t)) * num11;
			float num13 = Mathf.Sin(num10 * t) * num11;
			num = num * num12 + num5 * num13;
			num2 = num2 * num12 + num6 * num13;
			num3 = num3 * num12 + num7 * num13;
			num4 = num4 * num12 + num8 * num13;
		}
		else
		{
			num += t * (num5 - num);
			num2 += t * (num6 - num2);
			num3 += t * (num7 - num3);
			num4 += t * (num8 - num4);
			float num14 = num2 * num2 + num3 * num3 + num4 * num4 + num * num;
			if (num14 < MathC.SqrEpsilon)
			{
				return new QuaternionG(0f, 0f, 0f, 1f);
			}
			num14 = 1f / Mathf.Sqrt(num14);
			num2 *= num14;
			num3 *= num14;
			num4 *= num14;
			num *= num14;
		}
		return new QuaternionG(num2, num3, num4, num);
	}

	public static void Slerp(QuaternionG q0, QuaternionG q1, float t, out QuaternionG o)
	{
		float num = q0.w;
		float num2 = q0.x;
		float num3 = q0.y;
		float num4 = q0.z;
		float num5 = q1.w;
		float num6 = q1.x;
		float num7 = q1.y;
		float num8 = q1.z;
		float num9 = num * num5 + num2 * num6 + num3 * num7 + num4 * num8;
		if (num9 < 0f)
		{
			num9 = -num9;
			num5 = -num5;
			num6 = -num6;
			num7 = -num7;
			num8 = -num8;
		}
		if (num9 < 0.95f)
		{
			float num10 = Mathf.Acos(num9);
			float num11 = 1f / Mathf.Sin(num10);
			float num12 = Mathf.Sin(num10 * (1f - t)) * num11;
			float num13 = Mathf.Sin(num10 * t) * num11;
			o.w = num * num12 + num5 * num13;
			o.x = num2 * num12 + num6 * num13;
			o.y = num3 * num12 + num7 * num13;
			o.z = num4 * num12 + num8 * num13;
		}
		else
		{
			o.w = num + t * (num5 - num);
			o.x = num2 + t * (num6 - num2);
			o.y = num3 + t * (num7 - num3);
			o.z = num4 + t * (num8 - num4);
			o.Normalize();
		}
	}

	public static implicit operator QuaternionG(Quaternion q)
	{
		return new QuaternionG(q.x, q.y, q.z, q.w);
	}

	public static implicit operator Quaternion(QuaternionG q)
	{
		return new Quaternion(q.x, q.y, q.z, q.w);
	}

	public static QuaternionG operator +(QuaternionG q0, QuaternionG q1)
	{
		return new QuaternionG(q0.x + q1.x, q0.y + q1.y, q0.z + q1.z, q0.w + q1.w);
	}

	public static QuaternionG operator -(QuaternionG q0, QuaternionG q1)
	{
		return new QuaternionG(q0.x - q1.x, q0.y - q1.y, q0.z - q1.z, q0.w - q1.w);
	}

	public static QuaternionG operator -(QuaternionG q)
	{
		return new QuaternionG(-q.x, -q.y, -q.z, -q.w);
	}

	public static QuaternionG operator *(QuaternionG q, float s)
	{
		return new QuaternionG(q.x * s, q.y * s, q.z * s, q.w * s);
	}

	public static QuaternionG operator *(float s, QuaternionG q)
	{
		return new QuaternionG(q.x * s, q.y * s, q.z * s, q.w * s);
	}

	public static QuaternionG operator /(QuaternionG q, float s)
	{
		float num = 1f / s;
		return new QuaternionG(q.x * num, q.y * num, q.z * num, q.w * num);
	}

	public static QuaternionG operator *(QuaternionG q0, QuaternionG q1)
	{
		return new QuaternionG(q0.w * q1.x + q0.x * q1.w + q0.y * q1.z - q0.z * q1.y, q0.w * q1.y - q0.x * q1.z + q0.y * q1.w + q0.z * q1.x, q0.w * q1.z + q0.x * q1.y - q0.y * q1.x + q0.z * q1.w, q0.w * q1.w - q0.x * q1.x - q0.y * q1.y - q0.z * q1.z);
	}

	public static Vector3G operator *(QuaternionG q, Vector3G v)
	{
		float num = -q.x * v.x - q.y * v.y - q.z * v.z;
		float num2 = q.w * v.x + q.y * v.z - q.z * v.y;
		float num3 = q.w * v.y - q.x * v.z + q.z * v.x;
		float num4 = q.w * v.z + q.x * v.y - q.y * v.x;
		return new Vector3G(-num * q.x + num2 * q.w - num3 * q.z + num4 * q.y, -num * q.y + num2 * q.z + num3 * q.w - num4 * q.x, -num * q.z - num2 * q.y + num3 * q.x + num4 * q.w);
	}

	public void Rotate(Vector3G v, ref Vector3G o)
	{
		float num = -this.x * v.x - this.y * v.y - this.z * v.z;
		float num2 = this.w * v.x + this.y * v.z - this.z * v.y;
		float num3 = this.w * v.y - this.x * v.z + this.z * v.x;
		float num4 = this.w * v.z + this.x * v.y - this.y * v.x;
		o.x = -num * this.x + num2 * this.w - num3 * this.z + num4 * this.y;
		o.y = -num * this.y + num2 * this.z + num3 * this.w - num4 * this.x;
		o.z = -num * this.z - num2 * this.y + num3 * this.x + num4 * this.w;
	}

	public void Set(float _x, float _y, float _z, float _w)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
		this.w = _w;
	}

	public void SetAngleAxis(float angle, Vector3G axis)
	{
		angle *= MathC.ToRadians * 0.5f;
		Vector3G normalized = axis.normalized;
		float num = Mathf.Sin(angle);
		float num2 = Mathf.Cos(angle);
		this.x = normalized.x * num;
		this.y = normalized.y * num;
		this.z = normalized.z * num;
		this.w = num2;
	}

	public void Normalize()
	{
		float num = this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
		if (num < MathC.SqrEpsilon)
		{
			this.x = 0f;
			this.y = 0f;
			this.z = 0f;
			this.w = 1f;
			return;
		}
		num = 1f / Mathf.Sqrt(num);
		this.x *= num;
		this.y *= num;
		this.z *= num;
		this.w *= num;
	}

	public void Invert()
	{
		this.x = -this.x;
		this.y = -this.y;
		this.z = -this.z;
	}

	public void Negate()
	{
		this.x = -this.x;
		this.y = -this.y;
		this.z = -this.z;
		this.w = -this.w;
	}

	public void ScaleBy(float s)
	{
		this.x *= s;
		this.y *= s;
		this.z *= s;
		this.w *= s;
	}

	public void IncrementBy(QuaternionG q0)
	{
		this.x += q0.x;
		this.y += q0.y;
		this.z += q0.z;
		this.w += q0.w;
	}

	public void DecrementBy(QuaternionG q0)
	{
		this.x -= q0.x;
		this.y -= q0.y;
		this.z -= q0.z;
		this.w -= q0.w;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"(",
			this.x,
			", ",
			this.y,
			", ",
			this.z,
			", ",
			this.w,
			")"
		});
	}

	public static QuaternionG identity = Quaternion.identity;

	public float x;

	public float y;

	public float z;

	public float w;
}
