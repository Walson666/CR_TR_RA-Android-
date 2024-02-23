// dnSpy decompiler from Assembly-CSharp.dll class: PlayerWheelsRotation
using System;
using UnityEngine;

public class PlayerWheelsRotation : MonoBehaviour
{
	private void Start()
	{
		this.playerRef = base.GetComponentInParent<PlayerMovement>();
	}

	private void FixedUpdate()
	{
		if (this.playerRef == null)
		{
			return;
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float magnitude = this.playerRef.PlayerRigidbody.velocity.magnitude;
		base.transform.Rotate(Vector3.right, magnitude * fixedDeltaTime / this.wheelRadius * 57.29578f, Space.Self);
	}

	private PlayerMovement playerRef;

	public float wheelRadius = 1f;
}
