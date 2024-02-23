// dnSpy decompiler from Assembly-CSharp.dll class: Item
using System;
using UnityEngine;

public class Item : MonoBehaviour
{
	public void SetActive(bool s)
	{
		base.gameObject.SetActive(s);
	}

	public void AddPlayerRot()
	{
		for (int i = 0; i < 4; i++)
		{
			base.transform.GetChild(i).gameObject.AddComponent<PlayerWheelsRotation>();
		}
	}
}
