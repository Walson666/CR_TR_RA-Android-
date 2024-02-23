// dnSpy decompiler from Assembly-CSharp.dll class: CustomPanel
using System;
using UnityEngine;
using UnityEngine.UI;

public class CustomPanel : MonoBehaviour
{
	public void SetActive(bool val)
	{
		this.selectIcon.sprite = UIManager.GaragePage.carCustomize.sp[(!val) ? 0 : 1];
		base.gameObject.SetActive(val);
	}

	public Image selectIcon;
}
