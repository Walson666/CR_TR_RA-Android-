// dnSpy decompiler from Assembly-CSharp.dll class: UIWorldItemMul
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldItemMul : MonoBehaviour
{
	public void SetValues(int id, string name, int price, string distance, string playerCount, Sprite sp = null)
	{
		this.worldIcon.sprite = sp;
		this.worldTitle.text = name;
		this.priceText.text = price.ToString();
		this.targetText.text = distance;
		this.onlinePlayersText.text = playerCount;
		this.winnerPrizeText.text = (price * 2).ToString();
	}

	public void OnClickWorldInfo()
	{
	}

	public Text worldTitle;

	public Text targetText;

	public Text priceText;

	public Text winnerPrizeText;

	public Text onlinePlayersText;

	public Image worldIcon;
}
