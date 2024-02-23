// dnSpy decompiler from Assembly-CSharp.dll class: Booster
using System;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
	private void Start()
	{
	}

	public void Initialize(int index)
	{
	}

	private void BuyClicked()
	{
	}

	public void OnHoverBooster()
	{
	}

	public void UpdatePurchaseStatus()
	{
		this.purchasedImg.SetActive(this.equipped);
		this.buyBtn.gameObject.SetActive(!this.equipped);
	}

	public Button buyBtn;

	public Text ButtonPriceTf;

	public Image iconImage;

	public GameObject purchasedImg;

	[NonSerialized]
	public bool equipped;

	[HideInInspector]
	public BoosterType type;
}
