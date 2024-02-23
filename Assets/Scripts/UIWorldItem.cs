// dnSpy decompiler from Assembly-CSharp.dll class: UIWorldItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldItem : MonoBehaviour
{
	public void SetValues(int id, Sprite sp = null)
	{
		this.worldId = id;
		PlayerDataPersistant instance = PlayerDataPersistant.Instance;
		if (this.worldId >= Configurations.Instance.WorldCount || Configurations.Instance.worldInfo[this.worldId].worldName == "Coming Soon!")
		{
			this.worldTitle.text = "Coming Soon!";
			this.priceText.text = "$$";
			this.worldIcon.sprite = sp;
			return;
		}
		this.worldIcon.sprite = Configurations.Instance.worldInfo[this.worldId].worldIcon;
		this.worldTitle.text = Configurations.Instance.worldInfo[this.worldId].worldName;
		this.priceText.text = Configurations.Instance.worldInfo[this.worldId].coinsNeededToUnlock.ToString();
		if (!instance.IsWorldLocked(this.worldId) || Configurations.Instance.worldInfo[this.worldId].coinsNeededToUnlock <= 0)
		{
			this.UnlockWorld();
		}
	}

	public int myIndex
	{
		get
		{
			return base.transform.GetSiblingIndex();
		}
	}

	public void UnlockWorld()
	{
		this.priceText.gameObject.SetActive(false);
		foreach (GameObject gameObject in this.lockItems)
		{
			gameObject.SetActive(false);
		}
	}

	public GameObject[] lockItems;

	public Text worldTitle;

	public Text priceText;

	public Image worldIcon;

	private int worldId;
}
