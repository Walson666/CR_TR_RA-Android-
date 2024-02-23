// dnSpy decompiler from Assembly-CSharp.dll class: CustomToggle
using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
	public void Activate(bool state)
	{
		this.img.gameObject.SetActive(state);
	}

	public bool Enable
	{
		set
		{
			this.img.sprite = this.sp[(!value) ? 0 : 1];
		}
	}

	public Image img;

	public Sprite[] sp;
}
