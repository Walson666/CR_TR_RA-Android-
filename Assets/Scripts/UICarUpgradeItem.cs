// dnSpy decompiler from Assembly-CSharp.dll class: UICarUpgradeItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class UICarUpgradeItem : MonoBehaviour
{
	private void Awake()
	{
		this._id = base.transform.GetSiblingIndex();
		this.titleTxt.text = UICarUpgradeItem.playerData[this._id];
	}

	public int CurrentProgress
	{
		get
		{
			return this._currProgress;
		}
		set
		{
			this._currProgress = value;
			this.moreData.progressBar.fillAmount = (float)this._currProgress / 10f;
			this.costText.text = GameUtils.GetValueFormated(100 * this.CurrentProgress);
			this.addMore.SetActive(this._currProgress < this.max_progress);
			this.maxText.SetActive(this._currProgress >= this.max_progress);
		}
	}

	public void SetProgress(int value, int max, bool owned, Sprite sp)
	{
		this.max_progress = max;
		this.CurrentProgress = value;
		this.iconImg.sprite = sp;
		this._owned = owned;
	}

	public void IncrementValue()
	{
		Singleton<SoundManager>.Instance.Click();
		if (this._owned && this.CurrentProgress != this.max_progress)
		{
			if (PlayerDataPersistant.Instance.CanAfford(BASE_COST * CurrentProgress))
			{
				PlayerDataPersistant.Instance.BuyItem(BASE_COST * CurrentProgress);
				this.CurrentProgress++;
				PlayerDataPersistant.Instance.SetCarUpgradeValue(UIManager.GaragePage.SelectedCar.carID, this._id, this.CurrentProgress);
			}
			else
			{
				Singleton<UIManager>.Instance.ShowPurchaseScreen();
			}
		}
	}

	public Text titleTxt;

	public Text costText;

	public Image iconImg;

	public GameObject maxText;

	public GameObject addMore;

	public static string[] playerData = new string[]
	{
		"Acceleration",
		"Max Speed",
		"Brake",
		"Nitro",
		"Grip"
	};

	private int _id;

	private int _currProgress;

	private int max_progress;

	private bool _owned;

	public UILinkCarUpgradeItem moreData;

	private const int BASE_COST = 100;
}
