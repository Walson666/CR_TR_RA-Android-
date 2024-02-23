// dnSpy decompiler from Assembly-CSharp.dll class: UIDailyBonusPage
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIDailyBonusPage : MonoBehaviour
{
	private void Awake()
	{
		this.bonusObj = this.bonusItemParent.GetComponentsInChildren<DailyBonusItem>();
		for (int i = 0; i < this.bonusObj.Length; i++)
		{
			this.bonusObj[i].SetValues("Day" + (i + 1), Configurations.Instance.dailyRewardPrices[i].ToString(), this.dailyBonusIcons[i]);
		}
	}

	private void OnEnable()
	{
		this.HighlightCurrentBonus();
	}

	public void DailyItemClicked()
	{
		PlayerDataPersistant.Instance.Coins += Configurations.Instance.dailyRewardPrices[this.currDayIndx];
		this.bonusObj[this.currDayIndx].transform.localScale = Vector3.one;
		base.gameObject.SetActive(false);
	}

	private void HighlightCurrentBonus()
	{
		this.title.text = string.Format("CONGRATS!YOU EARNED DAILY BONUS OF {0}", Configurations.Instance.dailyRewardPrices[this.currDayIndx]);
		foreach (DailyBonusItem dailyBonusItem in this.bonusObj)
		{
			dailyBonusItem.bgImg.sprite = this.bonusBgSp[0];
			dailyBonusItem.transform.localScale = Vector2.one;
		}
		this.bonusObj[this.currDayIndx].HighlightPrizeDay(this.bonusBgSp[1]);
	}

	public GameObject bonusItemParent;

	public GameObject bonusItemPrefab;

	public Text title;

	public Sprite[] dailyBonusIcons;

	public Sprite[] bonusBgSp;

	protected DailyBonusItem[] bonusObj;

	protected int currDayIndx;
}
