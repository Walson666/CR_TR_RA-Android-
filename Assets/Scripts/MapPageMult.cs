// dnSpy decompiler from Assembly-CSharp.dll class: MapPageMult
using System;
using UnityEngine;
using UnityEngine.UI;

public class MapPageMult : MonoBehaviour
{
	private WorldBetInfo[] bets
	{
		get
		{
			return Configurations.Instance.bets;
		}
	}

	private void Awake()
	{
		this.worldItemList = base.GetComponentsInChildren<UIWorldItemMul>(true);
		for (int i = 0; i < this.worldItemList.Length; i++)
		{
			this.worldItemList[i].SetValues(i, this.bets[i].title, this.bets[i].betPrice, string.Format("{0} M", this.bets[i].distance), this.bets[i].playerCount.ToString(), Configurations.Instance.worldInfo[this.bets[i].worldId].worldIcon);
		}
		this.scrollRect.horizontalNormalizedPosition = 0f;
	}

	public void BtnClickedPlayBuy(int worldID)
	{
		if (PlayerDataPersistant.Instance.CanAfford((float)this.bets[worldID].betPrice))
		{
			MultiModeUI.multGameMode = worldID;
			Singleton<UIManager>.Instance.morePanels.MultiMode(true);
		}
		else
		{
			this.notEnoughCoins.SetActive(true);
		}
		if (worldID == 5)
		{
			PlayGameCenterManager.Instance.UnlockAchievement("CgkIj5y4oIoOEAIQBQ", 100.0);
		}
	}

	private UIWorldItemMul[] worldItemList;

	public GameObject notEnoughCoins;

	public ScrollRect scrollRect;
}
