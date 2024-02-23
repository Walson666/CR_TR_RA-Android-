// dnSpy decompiler from Assembly-CSharp.dll class: PlaneG
using System;

public struct PlaneG
{
	public PlaneG(Vector3G _normal, float _distance)
	{
		this.normal = _normal;
		this.distance = _distance;
	}

	public PlaneG(Vector3G _normal, Vector3G point)
	{
		this.normal = _normal;
		this.distance = -Vector3G.Dot(_normal, point);
	}

	public PlaneG(Vector3G p0, Vector3G p1, Vector3G p2)
	{
		this.normal = Vector3G.Cross(p1 - p0, p2 - p0);
		this.distance = -Vector3G.Dot(this.normal, p0);
	}

	public void SetNormalAndPoint(Vector3G _normal, Vector3G point)
	{
		this.normal = _normal;
		this.distance = -Vector3G.Dot(_normal, point);
	}

	public void Set3Points(Vector3G p0, Vector3G p1, Vector3G p2)
	{
		this.normal = Vector3G.Cross(p1 - p0, p2 - p0);
		this.distance = -Vector3G.Dot(this.normal, p0);
	}

	public float GetDistanceToPoint(Vector3G p)
	{
		return Vector3G.Dot(this.normal, p) + this.distance;
	}

	public Vector3G ReflectPoint(Vector3G p)
	{
		return p - 2f * (Vector3G.Dot(this.normal, p) + this.distance) * this.normal;
	}

	public void ReflectPoint(Vector3G p, Vector3G o)
	{
		float num = 2f * (Vector3G.Dot(this.normal, p) + this.distance);
		o.x = p.x - num * this.normal.x;
		o.y = p.y - num * this.normal.y;
		o.z = p.z - num * this.normal.z;
	}

	public Vector3G ReflectVector(Vector3G v)
	{
		return v - 2f * Vector3G.Dot(this.normal, v) * this.normal;
	}

	public void ReflectVector(Vector3G v, Vector3G o)
	{
		float num = 2f * Vector3G.Dot(this.normal, v);
		o.x = v.x - num * this.normal.x;
		o.y = v.y - num * this.normal.y;
		o.z = v.z - num * this.normal.z;
	}

	public Vector3G ProjectPoint(Vector3G p)
	{
		return p - (Vector3G.Dot(this.normal, p) + this.distance) * this.normal;
	}

	public void ProjectPoint(Vector3G p, Vector3G o)
	{
		float num = Vector3G.Dot(this.normal, p) + this.distance;
		o.x = p.x - num * this.normal.x;
		o.y = p.y - num * this.normal.y;
		o.z = p.z - num * this.normal.z;
	}

	public Vector3G ProjectVector(Vector3G v)
	{
		return v - Vector3G.Dot(this.normal, v) * this.normal;
	}

	public void ProjectVector(Vector3G v, Vector3G o)
	{
		float num = Vector3G.Dot(this.normal, v);
		o.x = v.x - num * this.normal.x;
		o.y = v.y - num * this.normal.y;
		o.z = v.z - num * this.normal.z;
	}

	public void Normalize()
	{
		float num = this.normal.Normalize();
		if (num < MathC.Epsilon)
		{
			return;
		}
		this.distance /= num;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"( ",
			this.normal.x,
			", ",
			this.normal.y,
			", ",
			this.normal.z,
			", ",
			this.distance,
			")"
		});
	}

	public Vector3G normal;

	public float distance;
}
