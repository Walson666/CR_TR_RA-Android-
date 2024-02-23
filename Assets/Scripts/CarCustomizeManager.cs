// dnSpy decompiler from Assembly-CSharp.dll class: CarCustomizeManager
using System;
using UnityEngine;
using UnityEngine.UI;

public class CarCustomizeManager : MonoBehaviour
{
	public bool PurchaseBtn
	{
		set
		{
			this.purchaseUI.SetActive(value);
			this.backBtn.SetActive(!value);
		}
	}

	private void Awake()
	{
		this.carPreviews = this.previewParent.GetComponentsInChildren<CarPreview>(true);
		this.upgrades = this.customPanels[0].GetComponentsInChildren<UICarUpgradeItem>();
		this.colorItems = this.customPanels[1].GetComponentsInChildren<UICarCustomizeItem>();
		this.stickerItems = this.customPanels[2].GetComponentsInChildren<UICarCustomizeItem>();
		this.rimItems = this.customPanels[3].GetComponentsInChildren<UICarCustomizeItem>();
	}

	private void ResetValues()
	{
		this._selectedItem[0] = this.colorItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currClr];
		this._selectedItem[1] = this.stickerItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currSticker];
		this._selectedItem[2] = this.rimItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currRim];
	}

	internal UICarCustomizeItem SelectedItem
	{
		get
		{
			return (this.custPanel < 0) ? null : this._selectedItem[this.custPanel];
		}
		set
		{
			if (this._selectedItem[this.custPanel] != null)
			{
				this._selectedItem[this.custPanel].selectIcon.SetActive(false);
			}
			this._selectedItem[this.custPanel] = value;
			this._selectedItem[this.custPanel].selectIcon.SetActive(true);
		}
	}

	internal void HandleCustomization(UICarCustomizeItem custItem)
	{
		this.SelectedItem = custItem;
		this.UpdateCustomization(this.SelectedItem.customiztionType, this.SelectedItem.id);
		if (this.SelectedItem.Owned)
		{
			this.EquipCustomization();
		}
		else
		{
			this.PurchaseBtn = true;
			this.custCostText.text = GameUtils.GetValueFormated(this.GetItemCost);
		}
	}

	public void EquipCustomization()
	{
		PlayerDataPersistant.Instance.SetCarCurrentCustomizationValue(UIManager.GaragePage.SelectedCar.carID, this.SelectedItem);
	}

	private int GetItemCost
	{
		get
		{
			return this.CUST_PRICE[(int)this.SelectedItem.customiztionType] * this.SelectedItem.id;
		}
	}

	public void OnClickPurchaseCustomization()
	{
		Singleton<SoundManager>.Instance.Click();
		if (PlayerDataPersistant.Instance.CanAfford((float)this.GetItemCost))
		{
			this.PurchaseBtn = false;
			PlayerDataPersistant.Instance.BuyItem(this.GetItemCost);
			this.SelectedItem.Owned = true;
			PlayerDataPersistant.Instance.SetCarCustomizationValue(this.SelectedCar.carID, this.SelectedItem);
			this.EquipCustomization();
		}
		else
		{
			Singleton<UIManager>.Instance.ShowPurchaseScreen();
		}
	}

	private void UpdateCustomization(CustomiztionType t, int id)
	{
		this.PurchaseBtn = false;
		switch (t)
		{
		default:
			this.carPreviews[UIManager.GaragePage.CurrCarID].UpdateColor(id);
			break;
		case CustomiztionType.Sticker:
			this.carPreviews[UIManager.GaragePage.CurrCarID].UpdateSticker(id);
			break;
		case CustomiztionType.Rim:
			this.carPreviews[UIManager.GaragePage.CurrCarID].UpdateRim(id);
			break;
		}
	}

	public void ShowCustomizationUI(int id)
	{
		if (id >= 0)
		{
			this.RemoveUnownedCustomizations();
		}
		this.custPanel = id - 1;
		for (int i = 0; i < this.customPanels.Length; i++)
		{
			this.customPanels[i].SetActive(id == i);
		}
		this.PurchaseBtn = false;
	}

	public CarData SelectedCar
	{
		get
		{
			return UIManager.GaragePage.SelectedCar;
		}
	}

	internal void Refresh()
	{
		for (int i = 0; i < this.upgrades.Length; i++)
		{
			this.upgrades[i].SetProgress(PlayerDataPersistant.Instance.GetCarUpgradeValue(this.SelectedCar.carID, i), PlayerDataPersistant.Instance.GetMaxValue(this.SelectedCar.carID, i), PlayerDataPersistant.Instance.CarList[this.SelectedCar.carID].owned, this.upgradeIcon[i]);
		}
		if (PlayerDataPersistant.Instance.CarList[this.SelectedCar.carID].owned)
		{
			for (int j = 0; j < this.colorItems.Length; j++)
			{
				this.colorItems[j].SetColorValue(this.carPreviews[this.SelectedCar.intID].carClrs[j], PlayerDataPersistant.Instance.GetCustomizationStatus(this.SelectedCar.carID, CustomiztionType.Color, j), PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currClr == j);
			}
			for (int k = 0; k < this.stickerItems.Length; k++)
			{
				this.stickerItems[k].SetItemImage(this.stickers[k], PlayerDataPersistant.Instance.GetCustomizationStatus(this.SelectedCar.carID, CustomiztionType.Sticker, k), PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currSticker == k);
			}
			for (int l = 0; l < this.rimItems.Length; l++)
			{
				this.rimItems[l].SetItemImage(this.rims[l], PlayerDataPersistant.Instance.GetCustomizationStatus(this.SelectedCar.carID, CustomiztionType.Rim, l), PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currRim == l);
			}
		}
		this.colorItems[0].Owned = true;
		this.stickerItems[0].Owned = true;
		this.rimItems[0].Owned = true;
		this.ShowCustomizationUI(-1);
		this.ResetValues();
		for (int m = 0; m < this.carPreviews.Length; m++)
		{
			this.carPreviews[m].SetActive(m == UIManager.GaragePage.CurrCarID);
		}
	}

	public void BackFromPurchaseUI()
	{
		Singleton<SoundManager>.Instance.Click();
		Singleton<UIManager>.Instance.ShowPage(UIScreens.FirstLand);
	}

	private void RemoveUnownedCustomizations()
	{
		if (this.SelectedItem && !this.SelectedItem.Owned)
		{
			switch (this.SelectedItem.customiztionType)
			{
			default:
				this.SelectedItem = this.colorItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currClr];
				break;
			case CustomiztionType.Sticker:
				this.SelectedItem = this.stickerItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currSticker];
				break;
			case CustomiztionType.Rim:
				this.SelectedItem = this.rimItems[PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID).currRim];
				break;
			}
			this.UpdateCustomization(this.SelectedItem.customiztionType, this.SelectedItem.id);
		}
	}

	public void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
	}

	private UICarUpgradeItem[] upgrades;

	private UICarCustomizeItem[] colorItems;

	private UICarCustomizeItem[] stickerItems;

	private UICarCustomizeItem[] rimItems;

	public GameObject previewParent;

	public CustomPanel[] customPanels;

	public GameObject purchaseUI;

	public GameObject backBtn;

	public Text custCostText;

	internal CarPreview[] carPreviews;

	internal UICarCustomizeItem[] _selectedItem = new UICarCustomizeItem[3];

	private int custPanel;

	public Sprite[] upgradeIcon;

	public Sprite[] stickers;

	public Sprite[] rims;

	public Sprite[] sp;

	public Sprite[] spCust;

	internal int[] CUST_PRICE = new int[]
	{
		100,
		100,
		200
	};
}
