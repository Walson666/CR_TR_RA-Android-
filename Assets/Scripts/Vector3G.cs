// dnSpy decompiler from Assembly-CSharp.dll class: Vector3G
using System;
using System.Globalization;
using UnityEngine;

public struct Vector3G
{
	public Vector3G(float _x, float _y)
	{
		this.x = _x;
		this.y = _y;
		this.z = 0f;
	}

	public Vector3G(float _x, float _y, float _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	public float magnitude
	{
		get
		{
			return Mathf.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		}
	}

	public float sqrMagnitude
	{
		get
		{
			return this.x * this.x + this.y * this.y + this.z * this.z;
		}
	}

	public Vector3G normalized
	{
		get
		{
			Vector3G result = new Vector3G(this.x, this.y, this.z);
			result.Normalize();
			return result;
		}
	}

	public static implicit operator Vector3G(Vector3 v)
	{
		return new Vector3G(v.x, v.y, v.z);
	}

	public static implicit operator Vector3(Vector3G v)
	{
		return new Vector3(v.x, v.y, v.z);
	}

	public static Vector3G operator +(Vector3G v0, Vector3G v1)
	{
		return new Vector3G(v0.x + v1.x, v0.y + v1.y, v0.z + v1.z);
	}

	public static Vector3G operator -(Vector3G v0, Vector3G v1)
	{
		return new Vector3G(v0.x - v1.x, v0.y - v1.y, v0.z - v1.z);
	}

	public static Vector3G operator -(Vector3G v)
	{
		return new Vector3G(-v.x, -v.y, -v.z);
	}

	public static Vector3G operator *(Vector3G v, float s)
	{
		return new Vector3G(v.x * s, v.y * s, v.z * s);
	}

	public static Vector3G operator *(float s, Vector3G v)
	{
		return new Vector3G(v.x * s, v.y * s, v.z * s);
	}

	public static Vector3G operator /(Vector3G v, float s)
	{
		float num = 1f / s;
		return new Vector3G(v.x * num, v.y * num, v.z * num);
	}

	public static float Dot(Vector3G v0, Vector3G v1)
	{
		return v0.x * v1.x + v0.y * v1.y + v0.z * v1.z;
	}

	public static Vector3G Cross(Vector3G v0, Vector3G v1)
	{
		return new Vector3G(v0.y * v1.z - v1.y * v0.z, v0.z * v1.x - v1.z * v0.x, v0.x * v1.y - v1.x * v0.y);
	}

	public static float Angle(Vector3G v0, Vector3G v1)
	{
		return Mathf.Acos(Vector3G.Dot(v0.normalized, v1.normalized)) * MathC.ToDegrees;
	}

	public static float Distance(Vector3G v0, Vector3G v1)
	{
		float num = v1.x - v0.x;
		float num2 = v1.y - v0.y;
		float num3 = v1.z - v0.z;
		float f = num * num + num2 * num2 + num3 * num3;
		return Mathf.Sqrt(f);
	}

	public static float SqrDistance(Vector3G v0, Vector3G v1)
	{
		float num = v1.x - v0.x;
		float num2 = v1.y - v0.y;
		float num3 = v1.z - v0.z;
		return num * num + num2 * num2 + num3 * num3;
	}

	public static Vector3G Parse(string str)
	{
		char[] separator = new char[]
		{
			','
		};
		string[] array = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
		float num = (array.Length <= 0) ? 0f : float.Parse(array[0], CultureInfo.InvariantCulture);
		float num2 = (array.Length <= 1) ? 0f : float.Parse(array[1], CultureInfo.InvariantCulture);
		float num3 = (array.Length <= 2) ? 0f : float.Parse(array[2], CultureInfo.InvariantCulture);
		return new Vector3G(num, num2, num3);
	}

	public static Vector3G Rotate(Vector3G from, Vector3G to, float t)
	{
		Vector3G axis = Vector3G.Cross(from, to);
		if (axis.sqrMagnitude > 0.001f)
		{
			return QuaternionG.AngleAxis(Vector3G.Angle(from, to) * t, axis) * from;
		}
		return to;
	}

	public static Vector3G Lerp(Vector3G from, Vector3G to, float t)
	{
		t = Mathf.Clamp01(t);
		float num = 1f - t;
		return new Vector3G(from.x * num + to.x * t, from.y * num + to.y * t, from.z * num + to.z * t);
	}

	

	public static bool LessEqualAll(Vector3G v1, Vector3G v2)
	{
		return v1.x <= v2.x && v1.y <= v2.y && v1.z <= v2.z;
	}

	public static bool LessAll(Vector3G v1, Vector3G v2)
	{
		return v1.x < v2.x && v1.y < v2.y && v1.z < v2.z;
	}

	public static bool LessEqualAny(Vector3G v1, Vector3G v2)
	{
		return v1.x <= v2.x || v1.y <= v2.y || v1.z <= v2.z;
	}

	public static bool LessAny(Vector3G v1, Vector3G v2)
	{
		return v1.x < v2.x || v1.y < v2.y || v1.z < v2.z;
	}

	public static bool GreaterEqualAll(Vector3G v1, Vector3G v2)
	{
		return v1.x >= v2.x && v1.y >= v2.y && v1.z >= v2.z;
	}

	public static bool GreaterAll(Vector3G v1, Vector3G v2)
	{
		return v1.x > v2.x && v1.y > v2.y && v1.z > v2.z;
	}

	public static bool GreaterEqualAny(Vector3G v1, Vector3G v2)
	{
		return v1.x >= v2.x || v1.y >= v2.y || v1.z >= v2.z;
	}

	public static bool GreaterAny(Vector3G v1, Vector3G v2)
	{
		return v1.x > v2.x || v1.y > v2.y || v1.z > v2.z;
	}

	public void Set(float _x, float _y, float _z)
	{
		this.x = _x;
		this.y = _y;
		this.z = _z;
	}

	public void Negate()
	{
		this.x = -this.x;
		this.y = -this.y;
		this.z = -this.z;
	}

	public float Normalize()
	{
		float num = this.x * this.x + this.y * this.y + this.z * this.z;
		if (num < MathC.SqrEpsilon)
		{
			return 0f;
		}
		num = Mathf.Sqrt(num);
		float num2 = 1f / num;
		this.x *= num2;
		this.y *= num2;
		this.z *= num2;
		return num;
	}

	public void ScaleBy(float s)
	{
		this.x *= s;
		this.y *= s;
		this.z *= s;
	}

	public void IncrementBy(Vector3G v0)
	{
		this.x += v0.x;
		this.y += v0.y;
		this.z += v0.z;
	}

	public void DecrementBy(Vector3G v0)
	{
		this.x -= v0.x;
		this.y -= v0.y;
		this.z -= v0.z;
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
			")"
		});
	}

	public static Vector3G zero = new Vector3G(0f, 0f, 0f);

	public static Vector3G one = new Vector3G(1f, 1f, 1f);

	public static Vector3G right = new Vector3G(1f, 0f, 0f);

	public static Vector3G up = new Vector3G(0f, 1f, 0f);

	public static Vector3G forward = new Vector3G(0f, 0f, 1f);

	public float x;

	public float y;

	public float z;
}
