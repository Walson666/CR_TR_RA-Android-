// dnSpy decompiler from Assembly-CSharp.dll class: UIPlayerBarMult
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerBarMult : MonoBehaviour
{
	internal void SetProgress(int v1, Sprite sp)
	{
		this.bar.fillAmount = (float)v1 / 10f;
		this.icon.sprite = sp;
	}

	public Image icon;

	public Image bar;
}
