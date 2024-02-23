// dnSpy decompiler from Assembly-CSharp.dll class: UIPlayerInfoMul
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfoMul : MonoBehaviour
{
	internal float timeReached
	{
		get
		{
			return this._timeReached;
		}
		set
		{
			this._timeReached = value;
			this.multiTimeText.text = GameUtils.MMSSFF(this._timeReached);
		}
	}

	public void Refresh()
	{
		for (int i = 0; i < this.playerUpgradeData.Length; i++)
		{
			this.playerUpgradeData[i].SetProgress(PlayerDataPersistant.Instance.GetCarUpgradeValue(UIManager.GaragePage.SelectedCar.carID, i), UIManager.GaragePage.carCustomize.upgradeIcon[i]);
		}
	}

	public void Dummy()
	{
		for (int i = 0; i < this.playerUpgradeData.Length; i++)
		{
			this.playerUpgradeData[i].SetProgress(UnityEngine.Random.Range(4, 10), UIManager.GaragePage.carCustomize.upgradeIcon[i]);
		}
	}

	internal void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
	}

	public Text pNameText;

	public Text pCountryText;

	public Text pLevelText;

	public Text worldText;

	public Text multiTimeText;

	public Image playerImg;

	public UIPlayerBarMult[] playerUpgradeData;

	public Image winnerScreen;

	public GameObject winnderObj;

	private float _timeReached;
}
