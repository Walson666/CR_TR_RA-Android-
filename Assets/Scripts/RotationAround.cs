// dnSpy decompiler from Assembly-CSharp.dll class: RotationAround
using System;
using UnityEngine;

public class RotationAround : MonoBehaviour
{
	private void Awake()
	{
		this.originalABAngles = new float[this.ABAngles.Length];
		this.ABAngles.CopyTo(this.originalABAngles, 0);
	}

	private void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		RotationAround.RotationType rotationType = this.type;
		if (rotationType != RotationAround.RotationType.Loop)
		{
			if (rotationType == RotationAround.RotationType.ABClassic)
			{
				if (this.ABAngles.Length > 0)
				{
					this.RotationStep(fixedDeltaTime);
					float num = 0f;
					if (this.RotateX)
					{
						num = base.transform.localRotation.eulerAngles.x;
					}
					else if (this.RotateY)
					{
						num = base.transform.localRotation.eulerAngles.y;
					}
					else if (this.RotateZ)
					{
						num = base.transform.localRotation.eulerAngles.z;
					}
					if (num < 180f && num > this.ABAngles[0])
					{
						this.rotationDir *= -1;
					}
					else if (num > 180f && num < this.ABAngles[1])
					{
						this.rotationDir *= -1;
					}
				}
			}
		}
		else
		{
			this.RotationStep(fixedDeltaTime);
		}
	}

	private void RotationStep(float deltaTime)
	{
		base.transform.position += base.transform.rotation * this.Pivot;
		if (this.RotateX)
		{
			base.transform.rotation *= Quaternion.AngleAxis(this.speed * (float)this.rotationDir * deltaTime, Vector3.right);
		}
		if (this.RotateY)
		{
			base.transform.rotation *= Quaternion.AngleAxis(this.speed * (float)this.rotationDir * deltaTime, Vector3.up);
		}
		if (this.RotateZ)
		{
			base.transform.rotation *= Quaternion.AngleAxis(this.speed * (float)this.rotationDir * deltaTime, Vector3.forward);
		}
		base.transform.position -= base.transform.rotation * this.Pivot;
	}

	public RotationAround.RotationType type;

	public Vector3 Pivot;

	public bool RotateX = true;

	public bool RotateY;

	public bool RotateZ;

	public float speed = 45f;

	public float[] ABAngles = new float[0];

	protected float[] originalABAngles;

	protected int initAngle;

	protected int rotationDir = 1;

	[Serializable]
	public enum RotationType
	{
		Loop,
		ABClassic
	}
}
