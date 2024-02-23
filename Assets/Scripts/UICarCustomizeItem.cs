// dnSpy decompiler from Assembly-CSharp.dll class: UICarCustomizeItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class UICarCustomizeItem : MonoBehaviour
{
	public int id
	{
		get
		{
			return base.transform.GetSiblingIndex();
		}
	}

	public bool Owned
	{
		get
		{
			return this._owned;
		}
		set
		{
			this._owned = value;
			this.buyIcon.sprite = UIManager.GaragePage.carCustomize.spCust[(!this._owned) ? 0 : 1];
		}
	}

	public void OnClick()
	{
		Singleton<SoundManager>.Instance.Click();
		UIManager.GaragePage.carCustomize.HandleCustomization(this);
	}

	public void SetColorValue(Color clr, bool owned = false, bool selected = false)
	{
		this.ImgCustomize.color = clr;
		this.Owned = owned;
		this.selectIcon.SetActive(selected);
	}

	public void SetItemImage(Sprite sp, bool owned = false, bool selected = false)
	{
		this.ImgCustomize.sprite = sp;
		this.Owned = owned;
		this.selectIcon.SetActive(selected);
	}

	public CustomiztionType customiztionType;

	public Image ImgCustomize;

	public Image buyIcon;

	public GameObject selectIcon;

	private bool _owned;
}
