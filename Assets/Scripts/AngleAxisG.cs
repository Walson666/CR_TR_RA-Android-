// dnSpy decompiler from Assembly-CSharp.dll class: AngleAxisG
using System;

public struct AngleAxisG
{
	public AngleAxisG(float _angle, float ax, float ay, float az)
	{
		this.angle = _angle;
		this.axis.x = ax;
		this.axis.y = ay;
		this.axis.z = az;
	}

	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"(",
			this.axis.x,
			", ",
			this.axis.y,
			", ",
			this.axis.z,
			", ",
			this.angle,
			")"
		});
	}

	public float angle;

	public Vector3G axis;
}
