// dnSpy decompiler from Assembly-CSharp.dll class: ExplosionActuate
using System;
using UnityEngine;

public class ExplosionActuate : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag.Equals("Player"))
		{
			UnityEngine.Debug.Log("*** ASSET COLLISION ***");
			GameObject.FindGameObjectWithTag("Player").SendMessage("PlayDebris");
		}
	}
}
