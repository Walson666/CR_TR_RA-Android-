// dnSpy decompiler from Assembly-CSharp.dll class: Checkpoint
using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private void OnEnable()
	{
		this.rivalEntered = false; this.entered = (this.rivalEntered );
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && !this.entered)
		{
			this.entered = true;
			Singleton<UIManager>.Instance.OnCheckpointEnter();
		}
		if (other.CompareTag("RivalCar") && !this.rivalEntered)
		{
			this.rivalEntered = true;
			other.GetComponent<RivalCar>().reached = true;
		}
	}

	private bool entered;

	private bool rivalEntered;
}
