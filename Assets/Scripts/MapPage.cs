// dnSpy decompiler from Assembly-CSharp.dll class: MapPage
using System;
using UnityEngine;

public class MapPage : MonoBehaviour
{
	private void Awake()
	{
		this.worldItemList = base.GetComponentsInChildren<UIWorldItem>(true);
		for (int i = 0; i < this.worldItemList.Length; i++)
		{
			this.worldItemList[i].SetValues(i, this.defaultSprite);
		}
	}

	public void BtnClickedPlayBuy(int worldID)
	{
		if (PlayerDataPersistant.Instance.IsWorldLocked(worldID))
		{
            //PlayerDataPersistant.Instance.UnlockWorld(worldID);
            int coinsNeededToUnlock = Configurations.Instance.worldInfo[worldID].coinsNeededToUnlock;
			bool flag = PlayerDataPersistant.Instance.CanAfford((float)coinsNeededToUnlock);
			if (flag)
			{
				this.worldItemList[worldID].UnlockWorld();
				this.UnlockWorld(worldID, coinsNeededToUnlock, this.worldItemList[worldID]);
			}
			else
			{
				Singleton<UIManager>.Instance.ShowPurchaseScreen();
			}
		}
		else
		{
			Singleton<GameCore>.Instance.CurrentWorld = worldID;
			Singleton<UIManager>.Instance.morePanels.SingleMode();
		}
	}

	private void UnlockWorld(int worldId, int worldPrice, UIWorldItem worlItem)
	{
		PlayerDataPersistant.Instance.BuyItem(worldPrice);
		PlayerDataPersistant.Instance.UnlockWorld(worldId);
		worlItem.SetValues(worldId, null);
	}

	private void UnlockWorld(int worldId, UIWorldItem worlItem)
	{
		PlayerDataPersistant.Instance.UnlockWorld(worldId);
		worlItem.SetValues(worldId, null);
	}

	public Sprite defaultSprite;

	private UIWorldItem[] worldItemList;
}
