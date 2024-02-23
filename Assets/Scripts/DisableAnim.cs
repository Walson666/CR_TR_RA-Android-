// dnSpy decompiler from Assembly-CSharp.dll class: DisableAnim
using System;
using UnityEngine;

public class DisableAnim : MonoBehaviour
{
	public void EndAnim()
	{
		base.gameObject.SetActive(false);
	}

	public void EndAnimMission()
	{
		base.gameObject.SetActive(false);
	}
}
