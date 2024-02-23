// dnSpy decompiler from Assembly-CSharp.dll class: BoundsG
using System;
using UnityEngine;

public struct BoundsG
{
	public BoundsG(Vector3G center, Vector3G size)
	{
		float num = size.x * 0.5f;
		float num2 = size.y * 0.5f;
		float num3 = size.z * 0.5f;
		this.min.x = center.x - num;
		this.min.y = center.y - num2;
		this.min.z = center.z - num3;
		this.max.x = center.x + num;
		this.max.y = center.y + num2;
		this.max.z = center.z + num3;
	}

	public static implicit operator BoundsG(Bounds b)
	{
		return new BoundsG(b.center, b.size);
	}

	public static implicit operator Bounds(BoundsG b)
	{
		return new Bounds(b.center, b.size);
	}

	public Vector3G center
	{
		get
		{
			return (this.min + this.max) * 0.5f;
		}
	}

	public Vector3G extents
	{
		get
		{
			return (this.max - this.min) * 0.5f;
		}
	}

	public Vector3G size
	{
		get
		{
			return this.max - this.min;
		}
	}

	public void Reset()
	{
		this.min.x = float.MaxValue;
		this.min.y = float.MaxValue;
		this.min.z = float.MaxValue;
		this.max.x = float.MinValue;
		this.max.y = float.MinValue;
		this.max.z = float.MinValue;
	}

	public bool Contains(BoundsG other)
	{
		return Vector3G.LessAll(this.min, other.min) && Vector3G.GreaterEqualAll(this.max, other.max);
	}

	public bool Intersect(BoundsG other)
	{
		return !Vector3G.LessAny(this.max, other.min) && !Vector3G.GreaterAny(this.min, other.max);
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"min: ",
			this.min,
			", max: ",
			this.max
		});
	}

	public Vector3G min;

	public Vector3G max;
}
