// dnSpy decompiler from Assembly-CSharp.dll class: DailyBonusItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyBonusItem : MonoBehaviour
{
	public void SetValues(string dayVal, string prize, Sprite sp)
	{
		this.dayTextField.text = dayVal;
		this.prizeTextField.text = prize;
		this.iconImg.sprite = sp;
	}

	public void HighlightPrizeDay(Sprite sp)
	{
		this.bgImg.sprite = sp;
		base.transform.localScale = Vector2.one * 1.2f;
	}

	public Text dayTextField;

	public Text prizeTextField;

	public Image iconImg;

	public Image bgImg;
}
