// dnSpy decompiler from Assembly-CSharp.dll class: InAppItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class InAppItem : MonoBehaviour
{
	public void SetValues(string quantity, string price, Sprite img)
	{
		this.quantityText.text = quantity;
		this.priceText.text = price;
		this.iconImg.sprite = img;
	}

	internal void SetLocalPrice(string localizedPriceString)
	{
		this.priceText.text = localizedPriceString;
	}

	public Image iconImg;

	public Text quantityText;

	public Text priceText;
}
