// dnSpy decompiler from Assembly-CSharp.dll class: CarPreview
using System;
using UnityEngine;

public class CarPreview : MonoBehaviour
{
	public int CurrRim
	{
		get
		{
			return this._currRim;
		}
		set
		{
			if (this._currRim >= 0 && this._currRim != value)
			{
				this.rims[this._currRim].SetActive(false);
			}
			this._currRim = value;
			this.rims[this._currRim].SetActive(true);
		}
	}

	public int CurrSticker
	{
		get
		{
			return this._currSticker;
		}
		set
		{
			if (this._currSticker >= 0)
			{
				this.stickerSet[this._currSticker].SetActive(false);
			}
			this._currSticker = value;
			this.stickerSet[this._currSticker].SetActive(true);
		}
	}

	private void Awake()
	{
		this.stickerSet = base.GetComponentsInChildren<StickerSet>(true);
		this.rims = base.GetComponentsInChildren<Item>(true);
	}

	public void OnEnable()
	{
		this.ApplyCustomizations(UIManager.GaragePage.SelectedCar.carID);
	}

	public void SetActive(bool state)
	{
		base.gameObject.SetActive(state);
	}

	public void UpdateColor(int i)
	{
		foreach (var item in rend)
		{
			item.material.SetColor("_Color", this.carClrs[i]);
        }
		//this.rend.material.SetColor("_Color", this.carClrs[i]);
	}

	public void UpdateColor(Color clr)
	{
        foreach (var item in rend)
        {
            item.material.SetColor("_Color", clr);
        }
        //this.rend.material.SetColor("_Color", clr);
	}

	public void ApplyCustomizations(CarID carId)
	{
		int currClr = PlayerDataPersistant.Instance.GetPlayerData(carId).currClr;
		this.UpdateColor(UIManager.GaragePage.carCustomize.carPreviews[(int)carId].carClrs[currClr]);
		this.UpdateRim(PlayerDataPersistant.Instance.GetPlayerData(carId).currRim);
		this.UpdateSticker(PlayerDataPersistant.Instance.GetPlayerData(carId).currSticker);
	}

	public void UpdateRim(int id)
	{
		this.CurrRim = id;
	}

	internal void RemoveUselessStuff(int rimID, int stickerId)
	{
		for (int i = 0; i < this.rims.Length; i++)
		{
			if (i != rimID)
			{
				UnityEngine.Object.Destroy(this.rims[i].gameObject);
			}
		}
		for (int j = 0; j < this.stickerSet.Length; j++)
		{
			if (j != stickerId)
			{
				UnityEngine.Object.Destroy(this.stickerSet[j].gameObject);
			}
		}
	}

	public void UpdateSticker(int index)
	{
		this.CurrSticker = index;
	}

	public MeshRenderer[] rend;

	public Color[] carClrs;

	internal Item[] rims;

	internal StickerSet[] stickerSet;

	private int _currRim;

	private int _currSticker;

	public const string CLR = "_Color";
}
