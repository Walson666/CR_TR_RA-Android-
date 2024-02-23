// dnSpy decompiler from Assembly-CSharp.dll class: Matrix4x4G
using System;
using UnityEngine;

public struct Matrix4x4G
{
	public Matrix4x4G(float _00, float _01, float _02, float _03, float _10, float _11, float _12, float _13, float _20, float _21, float _22, float _23, float _30, float _31, float _32, float _33)
	{
		this.m00 = _00;
		this.m01 = _01;
		this.m02 = _02;
		this.m03 = _03;
		this.m10 = _10;
		this.m11 = _11;
		this.m12 = _12;
		this.m13 = _13;
		this.m20 = _20;
		this.m21 = _21;
		this.m22 = _22;
		this.m23 = _23;
		this.m30 = _30;
		this.m31 = _31;
		this.m32 = _32;
		this.m33 = _33;
	}

	public Matrix4x4G transposed
	{
		get
		{
			return new Matrix4x4G(this.m00, this.m10, this.m20, this.m30, this.m01, this.m11, this.m21, this.m31, this.m02, this.m12, this.m22, this.m32, this.m03, this.m13, this.m23, this.m33);
		}
	}

	public Matrix4x4G inverse
	{
		get
		{
			Matrix4x4G result = default(Matrix4x4G);
			float num = this.determinant;
			if (Mathf.Abs(num) < MathC.Epsilon)
			{
				return result;
			}
			num = 1f / num;
			result.m00 = num * (this.m11 * (this.m22 * this.m33 - this.m32 * this.m23) + this.m21 * (this.m32 * this.m13 - this.m12 * this.m33) + this.m31 * (this.m12 * this.m23 - this.m22 * this.m13));
			result.m10 = num * (this.m12 * (this.m20 * this.m33 - this.m30 * this.m23) + this.m22 * (this.m30 * this.m13 - this.m10 * this.m33) + this.m32 * (this.m10 * this.m23 - this.m20 * this.m13));
			result.m20 = num * (this.m13 * (this.m20 * this.m31 - this.m30 * this.m21) + this.m23 * (this.m30 * this.m11 - this.m10 * this.m31) + this.m33 * (this.m10 * this.m21 - this.m20 * this.m11));
			result.m30 = num * (this.m10 * (this.m31 * this.m22 - this.m21 * this.m32) + this.m20 * (this.m11 * this.m32 - this.m31 * this.m12) + this.m30 * (this.m21 * this.m12 - this.m11 * this.m22));
			result.m01 = num * (this.m21 * (this.m02 * this.m33 - this.m32 * this.m03) + this.m31 * (this.m22 * this.m03 - this.m02 * this.m23) + this.m01 * (this.m32 * this.m23 - this.m22 * this.m33));
			result.m11 = num * (this.m22 * (this.m00 * this.m33 - this.m30 * this.m03) + this.m32 * (this.m20 * this.m03 - this.m00 * this.m23) + this.m02 * (this.m30 * this.m23 - this.m20 * this.m33));
			result.m21 = num * (this.m23 * (this.m00 * this.m31 - this.m30 * this.m01) + this.m33 * (this.m20 * this.m01 - this.m00 * this.m21) + this.m03 * (this.m30 * this.m21 - this.m20 * this.m31));
			result.m31 = num * (this.m20 * (this.m31 * this.m02 - this.m01 * this.m32) + this.m30 * (this.m01 * this.m22 - this.m21 * this.m02) + this.m00 * (this.m21 * this.m32 - this.m31 * this.m22));
			result.m02 = num * (this.m31 * (this.m02 * this.m13 - this.m12 * this.m03) + this.m01 * (this.m12 * this.m33 - this.m32 * this.m13) + this.m11 * (this.m32 * this.m03 - this.m02 * this.m33));
			result.m12 = num * (this.m32 * (this.m00 * this.m13 - this.m10 * this.m03) + this.m02 * (this.m10 * this.m33 - this.m30 * this.m13) + this.m12 * (this.m30 * this.m03 - this.m00 * this.m33));
			result.m22 = num * (this.m33 * (this.m00 * this.m11 - this.m10 * this.m01) + this.m03 * (this.m10 * this.m31 - this.m30 * this.m11) + this.m13 * (this.m30 * this.m01 - this.m00 * this.m31));
			result.m32 = num * (this.m30 * (this.m11 * this.m02 - this.m01 * this.m12) + this.m00 * (this.m31 * this.m12 - this.m11 * this.m32) + this.m10 * (this.m01 * this.m32 - this.m31 * this.m02));
			result.m03 = num * (this.m01 * (this.m22 * this.m13 - this.m12 * this.m23) + this.m11 * (this.m02 * this.m23 - this.m22 * this.m03) + this.m21 * (this.m12 * this.m03 - this.m02 * this.m13));
			result.m13 = num * (this.m02 * (this.m20 * this.m13 - this.m10 * this.m23) + this.m12 * (this.m00 * this.m23 - this.m20 * this.m03) + this.m22 * (this.m10 * this.m03 - this.m00 * this.m13));
			result.m23 = num * (this.m03 * (this.m20 * this.m11 - this.m10 * this.m21) + this.m13 * (this.m00 * this.m21 - this.m20 * this.m01) + this.m23 * (this.m10 * this.m01 - this.m00 * this.m11));
			result.m33 = num * (this.m00 * (this.m11 * this.m22 - this.m21 * this.m12) + this.m10 * (this.m21 * this.m02 - this.m01 * this.m22) + this.m20 * (this.m01 * this.m12 - this.m11 * this.m02));
			return result;
		}
	}

	public Matrix4x4G inverseFast
	{
		get
		{
			Matrix4x4G result = default(Matrix4x4G);
			float num = this.determinant;
			if (Mathf.Abs(num) < MathC.Epsilon)
			{
				return result;
			}
			num = 1f / num;
			result.m00 = num * (this.m11 * this.m22 - this.m12 * this.m21);
			result.m01 = num * (this.m21 * this.m02 - this.m22 * this.m01);
			result.m02 = num * (this.m01 * this.m12 - this.m02 * this.m11);
			result.m03 = 0f;
			result.m10 = num * (this.m12 * this.m20 - this.m10 * this.m22);
			result.m11 = num * (this.m22 * this.m00 - this.m20 * this.m02);
			result.m12 = num * (this.m02 * this.m10 - this.m00 * this.m12);
			result.m13 = 0f;
			result.m20 = num * (this.m10 * this.m21 - this.m11 * this.m20);
			result.m21 = num * (this.m20 * this.m01 - this.m21 * this.m00);
			result.m22 = num * (this.m00 * this.m11 - this.m01 * this.m10);
			result.m23 = 0f;
			result.m30 = num * (this.m10 * (this.m22 * this.m31 - this.m21 * this.m32) + this.m11 * (this.m20 * this.m32 - this.m22 * this.m30) + this.m12 * (this.m21 * this.m30 - this.m20 * this.m31));
			result.m31 = num * (this.m20 * (this.m02 * this.m31 - this.m01 * this.m32) + this.m21 * (this.m00 * this.m32 - this.m02 * this.m30) + this.m22 * (this.m01 * this.m30 - this.m00 * this.m31));
			result.m32 = num * (this.m30 * (this.m02 * this.m11 - this.m01 * this.m12) + this.m31 * (this.m00 * this.m12 - this.m02 * this.m10) + this.m32 * (this.m01 * this.m10 - this.m00 * this.m11));
			result.m33 = 1f;
			return result;
		}
	}

	public float determinant
	{
		get
		{
			return (this.m00 * this.m11 - this.m10 * this.m01) * (this.m22 * this.m33 - this.m32 * this.m23) - (this.m00 * this.m21 - this.m20 * this.m01) * (this.m12 * this.m33 - this.m32 * this.m13) + (this.m00 * this.m31 - this.m30 * this.m01) * (this.m12 * this.m23 - this.m22 * this.m13) + (this.m10 * this.m21 - this.m20 * this.m11) * (this.m02 * this.m33 - this.m32 * this.m03) - (this.m10 * this.m31 - this.m30 * this.m11) * (this.m02 * this.m23 - this.m22 * this.m03) + (this.m20 * this.m31 - this.m30 * this.m21) * (this.m02 * this.m13 - this.m12 * this.m03);
		}
	}

	public Vector3G xAxis
	{
		get
		{
			return new Vector3G(this.m00, this.m01, this.m02);
		}
		set
		{
			this.m00 = value.x;
			this.m01 = value.y;
			this.m02 = value.z;
			this.m03 = 0f;
		}
	}

	public Vector3G yAxis
	{
		get
		{
			return new Vector3G(this.m10, this.m11, this.m12);
		}
		set
		{
			this.m10 = value.x;
			this.m11 = value.y;
			this.m12 = value.z;
			this.m13 = 0f;
		}
	}

	public Vector3G zAxis
	{
		get
		{
			return new Vector3G(this.m20, this.m21, this.m22);
		}
		set
		{
			this.m20 = value.x;
			this.m21 = value.y;
			this.m22 = value.z;
			this.m23 = 0f;
		}
	}

	public Vector3G position
	{
		get
		{
			return new Vector3G(this.m30, this.m31, this.m32);
		}
		set
		{
			this.m30 = value.x;
			this.m31 = value.y;
			this.m32 = value.z;
			this.m33 = 1f;
		}
	}

	public Vector3G scale
	{
		get
		{
			return new Vector3G(this.xAxis.magnitude, this.yAxis.magnitude, this.zAxis.magnitude);
		}
		set
		{
			Vector3G xAxis = this.xAxis;
			Vector3G yAxis = this.yAxis;
			Vector3G zAxis = this.zAxis;
			float num = Vector3G.Dot(xAxis, xAxis);
			float num2 = Vector3G.Dot(yAxis, yAxis);
			float num3 = Vector3G.Dot(zAxis, zAxis);
			if (num > MathC.SqrEpsilon)
			{
				xAxis.ScaleBy(value.x / Mathf.Sqrt(num));
			}
			if (num2 > MathC.SqrEpsilon)
			{
				yAxis.ScaleBy(value.y / Mathf.Sqrt(num2));
			}
			if (num3 > MathC.SqrEpsilon)
			{
				zAxis.ScaleBy(value.z / Mathf.Sqrt(num3));
			}
			this.xAxis = xAxis;
			this.yAxis = yAxis;
			this.zAxis = zAxis;
		}
	}

	public QuaternionG rotation
	{
		get
		{
			float num = this.m00 + this.m11 + this.m22 + this.m33;
			float x;
			float y;
			float z;
			float w;
			if (num > MathC.Epsilon)
			{
				float num2 = Mathf.Sqrt(num) * 2f;
				float num3 = 1f / num2;
				x = (this.m12 - this.m21) * num3;
				y = (this.m20 - this.m02) * num3;
				z = (this.m01 - this.m10) * num3;
				w = 0.25f * num2;
			}
			else if (this.m00 > this.m11 && this.m00 > this.m22)
			{
				float num2 = Mathf.Sqrt(1f + this.m00 - this.m11 - this.m22) * 2f;
				float num3 = 1f / num2;
				x = -0.25f * num2;
				y = -(this.m10 + this.m01) * num3;
				z = -(this.m02 + this.m20) * num3;
				w = (this.m12 - this.m21) * num3;
			}
			else if (this.m11 > this.m22)
			{
				float num2 = Mathf.Sqrt(1f + this.m11 - this.m00 - this.m22) * 2f;
				float num3 = 1f / num2;
				x = -(this.m10 + this.m01) * num3;
				y = -0.25f * num2;
				z = -(this.m21 + this.m12) * num3;
				w = (this.m20 - this.m02) * num3;
			}
			else
			{
				float num2 = Mathf.Sqrt(1f + this.m22 - this.m00 - this.m11) * 2f;
				float num3 = 1f / num2;
				x = -(this.m02 + this.m20) * num3;
				y = -(this.m21 + this.m12) * num3;
				z = -0.25f * num2;
				w = (this.m01 - this.m10) * num3;
			}
			QuaternionG result = new QuaternionG(x, y, z, w);
			result.Normalize();
			return result;
		}
		set
		{
			Vector3G position = this.position;
			Vector3G scale = this.scale;
			this.SetTRS(position, value, scale);
		}
	}

	public AngleAxisG angleAxis
	{
		get
		{
			return this.rotation.angleAxis;
		}
		set
		{
			Vector3G position = this.position;
			Vector3G scale = this.scale;
			QuaternionG q = QuaternionG.AngleAxis(value.angle, value.axis);
			this.SetTRS(position, q, scale);
		}
	}

	public static implicit operator Matrix4x4G(Matrix4x4 _m)
	{
		return new Matrix4x4G
		{
			m00 = _m.m00,
			m01 = _m.m10,
			m02 = _m.m20,
			m03 = _m.m30,
			m10 = _m.m01,
			m11 = _m.m11,
			m12 = _m.m21,
			m13 = _m.m31,
			m20 = _m.m02,
			m21 = _m.m12,
			m22 = _m.m22,
			m23 = _m.m32,
			m30 = _m.m03,
			m31 = _m.m13,
			m32 = _m.m23,
			m33 = _m.m33
		};
	}

	public static implicit operator Matrix4x4(Matrix4x4G _m)
	{
		return new Matrix4x4
		{
			m00 = _m.m00,
			m10 = _m.m01,
			m20 = _m.m02,
			m30 = _m.m03,
			m01 = _m.m10,
			m11 = _m.m11,
			m21 = _m.m12,
			m31 = _m.m13,
			m02 = _m.m20,
			m12 = _m.m21,
			m22 = _m.m22,
			m32 = _m.m23,
			m03 = _m.m30,
			m13 = _m.m31,
			m23 = _m.m32,
			m33 = _m.m33
		};
	}

	public static Matrix4x4G operator *(Matrix4x4G m1, Matrix4x4G m0)
	{
		return new Matrix4x4G
		{
			m00 = m0.m00 * m1.m00 + m0.m01 * m1.m10 + m0.m02 * m1.m20 + m0.m03 * m1.m30,
			m01 = m0.m00 * m1.m01 + m0.m01 * m1.m11 + m0.m02 * m1.m21 + m0.m03 * m1.m31,
			m02 = m0.m00 * m1.m02 + m0.m01 * m1.m12 + m0.m02 * m1.m22 + m0.m03 * m1.m32,
			m03 = m0.m00 * m1.m03 + m0.m01 * m1.m13 + m0.m02 * m1.m23 + m0.m03 * m1.m33,
			m10 = m0.m10 * m1.m00 + m0.m11 * m1.m10 + m0.m12 * m1.m20 + m0.m13 * m1.m30,
			m11 = m0.m10 * m1.m01 + m0.m11 * m1.m11 + m0.m12 * m1.m21 + m0.m13 * m1.m31,
			m12 = m0.m10 * m1.m02 + m0.m11 * m1.m12 + m0.m12 * m1.m22 + m0.m13 * m1.m32,
			m13 = m0.m10 * m1.m03 + m0.m11 * m1.m13 + m0.m12 * m1.m23 + m0.m13 * m1.m33,
			m20 = m0.m20 * m1.m00 + m0.m21 * m1.m10 + m0.m22 * m1.m20 + m0.m23 * m1.m30,
			m21 = m0.m20 * m1.m01 + m0.m21 * m1.m11 + m0.m22 * m1.m21 + m0.m23 * m1.m31,
			m22 = m0.m20 * m1.m02 + m0.m21 * m1.m12 + m0.m22 * m1.m22 + m0.m23 * m1.m32,
			m23 = m0.m20 * m1.m03 + m0.m21 * m1.m13 + m0.m22 * m1.m23 + m0.m23 * m1.m33,
			m30 = m0.m30 * m1.m00 + m0.m31 * m1.m10 + m0.m32 * m1.m20 + m0.m33 * m1.m30,
			m31 = m0.m30 * m1.m01 + m0.m31 * m1.m11 + m0.m32 * m1.m21 + m0.m33 * m1.m31,
			m32 = m0.m30 * m1.m02 + m0.m31 * m1.m12 + m0.m32 * m1.m22 + m0.m33 * m1.m32,
			m33 = m0.m30 * m1.m03 + m0.m31 * m1.m13 + m0.m32 * m1.m23 + m0.m33 * m1.m33
		};
	}

	public static Vector3G operator *(Matrix4x4G m, Vector3G v)
	{
		float x = v.x;
		float y = v.y;
		float z = v.z;
		return new Vector3G(x * m.m00 + y * m.m10 + z * m.m20 + m.m30, x * m.m01 + y * m.m11 + z * m.m21 + m.m31, x * m.m02 + y * m.m12 + z * m.m22 + m.m32);
	}

	public static Matrix4x4G LookAt(Vector3G eye, Vector3G target, Vector3G up)
	{
		Vector3G vector3G = target - eye;
		vector3G.Normalize();
		Vector3G v = Vector3G.Cross(up, vector3G);
		v.Normalize();
		Vector3G vector3G2 = Vector3G.Cross(vector3G, v);
		return new Matrix4x4G(v.x, v.y, v.z, 0f, vector3G2.x, vector3G2.y, vector3G2.z, 0f, vector3G.x, vector3G.y, vector3G.z, 0f, eye.x, eye.y, eye.z, 1f);
	}

	public static Matrix4x4G TRS(Vector3G t, QuaternionG q, Vector3G s)
	{
		Matrix4x4G result = default(Matrix4x4G);
		float num = 2f * q.x * q.y;
		float num2 = 2f * q.x * q.z;
		float num3 = 2f * q.x * q.w;
		float num4 = 2f * q.y * q.z;
		float num5 = 2f * q.y * q.w;
		float num6 = 2f * q.z * q.w;
		float num7 = q.x * q.x;
		float num8 = q.y * q.y;
		float num9 = q.z * q.z;
		float num10 = q.w * q.w;
		float x = s.x;
		float y = s.y;
		float z = s.z;
		result.m00 = (num7 - num8 - num9 + num10) * x;
		result.m01 = (num + num6) * x;
		result.m02 = (num2 - num5) * x;
		result.m03 = 0f;
		result.m10 = (num - num6) * y;
		result.m11 = (-num7 + num8 - num9 + num10) * y;
		result.m12 = (num4 + num3) * y;
		result.m13 = 0f;
		result.m20 = (num2 + num5) * z;
		result.m21 = (num4 - num3) * z;
		result.m22 = (-num7 - num8 + num9 + num10) * z;
		result.m23 = 0f;
		result.m30 = t.x;
		result.m31 = t.y;
		result.m32 = t.z;
		result.m33 = 1f;
		return result;
	}

	public static Matrix4x4G TRS(Vector3G t, AngleAxisG r, Vector3G s)
	{
		return Matrix4x4G.TRS(t, QuaternionG.AngleAxis(r.angle, r.axis), s);
	}

	public void SetTRS(Vector3G t, QuaternionG q, Vector3G s)
	{
		float num = 2f * q.x * q.y;
		float num2 = 2f * q.x * q.z;
		float num3 = 2f * q.x * q.w;
		float num4 = 2f * q.y * q.z;
		float num5 = 2f * q.y * q.w;
		float num6 = 2f * q.z * q.w;
		float num7 = q.x * q.x;
		float num8 = q.y * q.y;
		float num9 = q.z * q.z;
		float num10 = q.w * q.w;
		float x = s.x;
		float y = s.y;
		float z = s.z;
		this.m00 = (num7 - num8 - num9 + num10) * x;
		this.m01 = (num + num6) * x;
		this.m02 = (num2 - num5) * x;
		this.m03 = 0f;
		this.m10 = (num - num6) * y;
		this.m11 = (-num7 + num8 - num9 + num10) * y;
		this.m12 = (num4 + num3) * y;
		this.m13 = 0f;
		this.m20 = (num2 + num5) * z;
		this.m21 = (num4 - num3) * z;
		this.m22 = (-num7 - num8 + num9 + num10) * z;
		this.m23 = 0f;
		this.m30 = t.x;
		this.m31 = t.y;
		this.m32 = t.z;
		this.m33 = 1f;
	}

	public void SetTRS(Vector3G t, AngleAxisG r, Vector3G s)
	{
		this.SetTRS(t, QuaternionG.AngleAxis(r.angle, r.axis), s);
	}

	public Vector3G MultiplyPoint3x4(Vector3G p)
	{
		float x = p.x;
		float y = p.y;
		float z = p.z;
		return new Vector3G(x * this.m00 + y * this.m10 + z * this.m20 + this.m30, x * this.m01 + y * this.m11 + z * this.m21 + this.m31, x * this.m02 + y * this.m12 + z * this.m22 + this.m32);
	}

	public void MultiplyPoint3x4(Vector3G p, out Vector3G o)
	{
		float x = p.x;
		float y = p.y;
		float z = p.z;
		o.x = x * this.m00 + y * this.m10 + z * this.m20 + this.m30;
		o.y = x * this.m01 + y * this.m11 + z * this.m21 + this.m31;
		o.z = x * this.m02 + y * this.m12 + z * this.m22 + this.m32;
	}

	public Vector3G MultiplyPoint3x4(float px, float py, float pz)
	{
		return new Vector3G(px * this.m00 + py * this.m10 + pz * this.m20 + this.m30, px * this.m01 + py * this.m11 + pz * this.m21 + this.m31, px * this.m02 + py * this.m12 + pz * this.m22 + this.m32);
	}

	public Vector3G MultiplyPoint(Vector3G p)
	{
		float x = p.x;
		float y = p.y;
		float z = p.z;
		float num = 1f / (x * this.m03 + y * this.m13 + z * this.m23 + this.m33);
		return new Vector3G((x * this.m00 + y * this.m10 + z * this.m20 + this.m30) * num, (x * this.m01 + y * this.m11 + z * this.m21 + this.m31) * num, (x * this.m02 + y * this.m12 + z * this.m22 + this.m32) * num);
	}

	public void MultiplyPoint(Vector3G p, out Vector3G o)
	{
		float x = p.x;
		float y = p.y;
		float z = p.z;
		float num = 1f / (x * this.m03 + y * this.m13 + z * this.m23 + this.m33);
		o.x = (x * this.m00 + y * this.m10 + z * this.m20 + this.m30) * num;
		o.y = (x * this.m01 + y * this.m11 + z * this.m21 + this.m31) * num;
		o.z = (x * this.m02 + y * this.m12 + z * this.m22 + this.m32) * num;
	}

	public Vector3G MultiplyPoint(float px, float py, float pz)
	{
		float num = 1f / (px * this.m03 + py * this.m13 + pz * this.m23 + this.m33);
		return new Vector3G((px * this.m00 + py * this.m10 + pz * this.m20 + this.m30) * num, (px * this.m01 + py * this.m11 + pz * this.m21 + this.m31) * num, (px * this.m02 + py * this.m12 + pz * this.m22 + this.m32) * num);
	}

	public Vector3G MultiplyVector(Vector3G v)
	{
		float x = v.x;
		float y = v.y;
		float z = v.z;
		return new Vector3G(x * this.m00 + y * this.m10 + z * this.m20, x * this.m01 + y * this.m11 + z * this.m21, x * this.m02 + y * this.m12 + z * this.m22);
	}

	public void MultiplyVector(Vector3G v, out Vector3G o)
	{
		float x = v.x;
		float y = v.y;
		float z = v.z;
		o.x = x * this.m00 + y * this.m10 + z * this.m20;
		o.y = x * this.m01 + y * this.m11 + z * this.m21;
		o.z = x * this.m02 + y * this.m12 + z * this.m22;
	}

	public Vector3G MultiplyVector(float vx, float vy, float vz)
	{
		return new Vector3G(vx * this.m00 + vy * this.m10 + vz * this.m20, vx * this.m01 + vy * this.m11 + vz * this.m21, vx * this.m02 + vy * this.m12 + vz * this.m22);
	}

	public void Append(Matrix4x4G m0)
	{
		float num = this.m00;
		float num2 = this.m01;
		float num3 = this.m02;
		float num4 = this.m03;
		float num5 = this.m10;
		float num6 = this.m11;
		float num7 = this.m12;
		float num8 = this.m13;
		float num9 = this.m20;
		float num10 = this.m21;
		float num11 = this.m22;
		float num12 = this.m23;
		float num13 = this.m30;
		float num14 = this.m31;
		float num15 = this.m32;
		float num16 = this.m33;
		this.m00 = m0.m00 * num + m0.m01 * num5 + m0.m02 * num9 + m0.m03 * num13;
		this.m01 = m0.m00 * num2 + m0.m01 * num6 + m0.m02 * num10 + m0.m03 * num14;
		this.m02 = m0.m00 * num3 + m0.m01 * num7 + m0.m02 * num11 + m0.m03 * num15;
		this.m03 = m0.m00 * num4 + m0.m01 * num8 + m0.m02 * num12 + m0.m03 * num16;
		this.m10 = m0.m10 * num + m0.m11 * num5 + m0.m12 * num9 + m0.m13 * num13;
		this.m11 = m0.m10 * num2 + m0.m11 * num6 + m0.m12 * num10 + m0.m13 * num14;
		this.m12 = m0.m10 * num3 + m0.m11 * num7 + m0.m12 * num11 + m0.m13 * num15;
		this.m13 = m0.m10 * num4 + m0.m11 * num8 + m0.m12 * num12 + m0.m13 * num16;
		this.m20 = m0.m20 * num + m0.m21 * num5 + m0.m22 * num9 + m0.m23 * num13;
		this.m21 = m0.m20 * num2 + m0.m21 * num6 + m0.m22 * num10 + m0.m23 * num14;
		this.m22 = m0.m20 * num3 + m0.m21 * num7 + m0.m22 * num11 + m0.m23 * num15;
		this.m23 = m0.m20 * num4 + m0.m21 * num8 + m0.m22 * num12 + m0.m23 * num16;
		this.m30 = m0.m30 * num + m0.m31 * num5 + m0.m32 * num9 + m0.m33 * num13;
		this.m31 = m0.m30 * num2 + m0.m31 * num6 + m0.m32 * num10 + m0.m33 * num14;
		this.m32 = m0.m30 * num3 + m0.m31 * num7 + m0.m32 * num11 + m0.m33 * num15;
		this.m33 = m0.m30 * num4 + m0.m31 * num8 + m0.m32 * num12 + m0.m33 * num16;
	}

	public void Prepend(Matrix4x4G m1)
	{
		float num = this.m00;
		float num2 = this.m01;
		float num3 = this.m02;
		float num4 = this.m03;
		float num5 = this.m10;
		float num6 = this.m11;
		float num7 = this.m12;
		float num8 = this.m13;
		float num9 = this.m20;
		float num10 = this.m21;
		float num11 = this.m22;
		float num12 = this.m23;
		float num13 = this.m30;
		float num14 = this.m31;
		float num15 = this.m32;
		float num16 = this.m33;
		this.m00 = num * m1.m00 + num2 * m1.m10 + num3 * m1.m20 + num4 * m1.m30;
		this.m01 = num * m1.m01 + num2 * m1.m11 + num3 * m1.m21 + num4 * m1.m31;
		this.m02 = num * m1.m02 + num2 * m1.m12 + num3 * m1.m22 + num4 * m1.m32;
		this.m03 = num * m1.m03 + num2 * m1.m13 + num3 * m1.m23 + num4 * m1.m33;
		this.m10 = num5 * m1.m00 + num6 * m1.m10 + num7 * m1.m20 + num8 * m1.m30;
		this.m11 = num5 * m1.m01 + num6 * m1.m11 + num7 * m1.m21 + num8 * m1.m31;
		this.m12 = num5 * m1.m02 + num6 * m1.m12 + num7 * m1.m22 + num8 * m1.m32;
		this.m13 = num5 * m1.m03 + num6 * m1.m13 + num7 * m1.m23 + num8 * m1.m33;
		this.m20 = num9 * m1.m00 + num10 * m1.m10 + num11 * m1.m20 + num12 * m1.m30;
		this.m21 = num9 * m1.m01 + num10 * m1.m11 + num11 * m1.m21 + num12 * m1.m31;
		this.m22 = num9 * m1.m02 + num10 * m1.m12 + num11 * m1.m22 + num12 * m1.m32;
		this.m23 = num9 * m1.m03 + num10 * m1.m13 + num11 * m1.m23 + num12 * m1.m33;
		this.m30 = num13 * m1.m00 + num14 * m1.m10 + num15 * m1.m20 + num16 * m1.m30;
		this.m31 = num13 * m1.m01 + num14 * m1.m11 + num15 * m1.m21 + num16 * m1.m31;
		this.m32 = num13 * m1.m02 + num14 * m1.m12 + num15 * m1.m22 + num16 * m1.m32;
		this.m33 = num13 * m1.m03 + num14 * m1.m13 + num15 * m1.m23 + num16 * m1.m33;
	}

	public void Transpose()
	{
		float num = this.m01;
		this.m01 = this.m10;
		this.m10 = num;
		num = this.m02;
		this.m02 = this.m20;
		this.m20 = num;
		num = this.m03;
		this.m03 = this.m30;
		this.m30 = num;
		num = this.m12;
		this.m12 = this.m21;
		this.m21 = num;
		num = this.m13;
		this.m13 = this.m31;
		this.m31 = num;
		num = this.m23;
		this.m23 = this.m32;
		this.m32 = num;
	}

	public bool Invert()
	{
		float num = this.determinant;
		if (Mathf.Abs(num) < MathC.Epsilon)
		{
			return false;
		}
		num = 1f / num;
		float num2 = num * (this.m11 * (this.m22 * this.m33 - this.m32 * this.m23) + this.m21 * (this.m32 * this.m13 - this.m12 * this.m33) + this.m31 * (this.m12 * this.m23 - this.m22 * this.m13));
		float num3 = num * (this.m12 * (this.m20 * this.m33 - this.m30 * this.m23) + this.m22 * (this.m30 * this.m13 - this.m10 * this.m33) + this.m32 * (this.m10 * this.m23 - this.m20 * this.m13));
		float num4 = num * (this.m13 * (this.m20 * this.m31 - this.m30 * this.m21) + this.m23 * (this.m30 * this.m11 - this.m10 * this.m31) + this.m33 * (this.m10 * this.m21 - this.m20 * this.m11));
		float num5 = num * (this.m10 * (this.m31 * this.m22 - this.m21 * this.m32) + this.m20 * (this.m11 * this.m32 - this.m31 * this.m12) + this.m30 * (this.m21 * this.m12 - this.m11 * this.m22));
		float num6 = num * (this.m21 * (this.m02 * this.m33 - this.m32 * this.m03) + this.m31 * (this.m22 * this.m03 - this.m02 * this.m23) + this.m01 * (this.m32 * this.m23 - this.m22 * this.m33));
		float num7 = num * (this.m22 * (this.m00 * this.m33 - this.m30 * this.m03) + this.m32 * (this.m20 * this.m03 - this.m00 * this.m23) + this.m02 * (this.m30 * this.m23 - this.m20 * this.m33));
		float num8 = num * (this.m23 * (this.m00 * this.m31 - this.m30 * this.m01) + this.m33 * (this.m20 * this.m01 - this.m00 * this.m21) + this.m03 * (this.m30 * this.m21 - this.m20 * this.m31));
		float num9 = num * (this.m20 * (this.m31 * this.m02 - this.m01 * this.m32) + this.m30 * (this.m01 * this.m22 - this.m21 * this.m02) + this.m00 * (this.m21 * this.m32 - this.m31 * this.m22));
		float num10 = num * (this.m31 * (this.m02 * this.m13 - this.m12 * this.m03) + this.m01 * (this.m12 * this.m33 - this.m32 * this.m13) + this.m11 * (this.m32 * this.m03 - this.m02 * this.m33));
		float num11 = num * (this.m32 * (this.m00 * this.m13 - this.m10 * this.m03) + this.m02 * (this.m10 * this.m33 - this.m30 * this.m13) + this.m12 * (this.m30 * this.m03 - this.m00 * this.m33));
		float num12 = num * (this.m33 * (this.m00 * this.m11 - this.m10 * this.m01) + this.m03 * (this.m10 * this.m31 - this.m30 * this.m11) + this.m13 * (this.m30 * this.m01 - this.m00 * this.m31));
		float num13 = num * (this.m30 * (this.m11 * this.m02 - this.m01 * this.m12) + this.m00 * (this.m31 * this.m12 - this.m11 * this.m32) + this.m10 * (this.m01 * this.m32 - this.m31 * this.m02));
		float num14 = num * (this.m01 * (this.m22 * this.m13 - this.m12 * this.m23) + this.m11 * (this.m02 * this.m23 - this.m22 * this.m03) + this.m21 * (this.m12 * this.m03 - this.m02 * this.m13));
		float num15 = num * (this.m02 * (this.m20 * this.m13 - this.m10 * this.m23) + this.m12 * (this.m00 * this.m23 - this.m20 * this.m03) + this.m22 * (this.m10 * this.m03 - this.m00 * this.m13));
		float num16 = num * (this.m03 * (this.m20 * this.m11 - this.m10 * this.m21) + this.m13 * (this.m00 * this.m21 - this.m20 * this.m01) + this.m23 * (this.m10 * this.m01 - this.m00 * this.m11));
		float num17 = num * (this.m00 * (this.m11 * this.m22 - this.m21 * this.m12) + this.m10 * (this.m21 * this.m02 - this.m01 * this.m22) + this.m20 * (this.m01 * this.m12 - this.m11 * this.m02));
		this.m00 = num2;
		this.m01 = num6;
		this.m02 = num10;
		this.m03 = num14;
		this.m10 = num3;
		this.m11 = num7;
		this.m12 = num11;
		this.m13 = num15;
		this.m20 = num4;
		this.m21 = num8;
		this.m22 = num12;
		this.m23 = num16;
		this.m30 = num5;
		this.m31 = num9;
		this.m32 = num13;
		this.m33 = num17;
		return true;
	}

	public bool InvertFast()
	{
		float num = this.determinant;
		if (Mathf.Abs(num) < MathC.Epsilon)
		{
			return false;
		}
		num = 1f / num;
		float num2 = num * (this.m11 * this.m22 - this.m12 * this.m21);
		float num3 = num * (this.m21 * this.m02 - this.m22 * this.m01);
		float num4 = num * (this.m01 * this.m12 - this.m02 * this.m11);
		float num5 = 0f;
		float num6 = num * (this.m12 * this.m20 - this.m10 * this.m22);
		float num7 = num * (this.m22 * this.m00 - this.m20 * this.m02);
		float num8 = num * (this.m02 * this.m10 - this.m00 * this.m12);
		float num9 = 0f;
		float num10 = num * (this.m10 * this.m21 - this.m11 * this.m20);
		float num11 = num * (this.m20 * this.m01 - this.m21 * this.m00);
		float num12 = num * (this.m00 * this.m11 - this.m01 * this.m10);
		float num13 = 0f;
		float num14 = num * (this.m10 * (this.m22 * this.m31 - this.m21 * this.m32) + this.m11 * (this.m20 * this.m32 - this.m22 * this.m30) + this.m12 * (this.m21 * this.m30 - this.m20 * this.m31));
		float num15 = num * (this.m20 * (this.m02 * this.m31 - this.m01 * this.m32) + this.m21 * (this.m00 * this.m32 - this.m02 * this.m30) + this.m22 * (this.m01 * this.m30 - this.m00 * this.m31));
		float num16 = num * (this.m30 * (this.m02 * this.m11 - this.m01 * this.m12) + this.m31 * (this.m00 * this.m12 - this.m02 * this.m10) + this.m32 * (this.m01 * this.m10 - this.m00 * this.m11));
		float num17 = 1f;
		this.m00 = num2;
		this.m01 = num3;
		this.m02 = num4;
		this.m03 = num5;
		this.m10 = num6;
		this.m11 = num7;
		this.m12 = num8;
		this.m13 = num9;
		this.m20 = num10;
		this.m21 = num11;
		this.m22 = num12;
		this.m23 = num13;
		this.m30 = num14;
		this.m31 = num15;
		this.m32 = num16;
		this.m33 = num17;
		return true;
	}

	public static Matrix4x4G identity = Matrix4x4.identity;

	public float m00;

	public float m01;

	public float m02;

	public float m03;

	public float m10;

	public float m11;

	public float m12;

	public float m13;

	public float m20;

	public float m21;

	public float m22;

	public float m23;

	public float m30;

	public float m31;

	public float m32;

	public float m33;
}
