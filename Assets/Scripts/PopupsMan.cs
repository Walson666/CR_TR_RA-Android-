// dnSpy decompiler from Assembly-CSharp.dll class: PopupsMan
using System;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PopupsMan : MonoBehaviour
{
	private void OnEnable()
	{
		//this.controlDropDown.value = Singleton<UIManager>.Instance.InputMode;
		this.qualityDropDown.value = this.QualityLevel;
	}


    private void Start()
    {
        /*if(GP_Socials.IsSupportsShare())
			ShareButton.SetActive(true);
		else
			ShareButton.SetActive(false);*/
        ShareButton.SetActive(false);
    }

    public int QualityLevel
	{
		get
		{
			if (QualitySettings.GetQualityLevel() == 5)
			{
				return 2;
			}
			if (QualitySettings.GetQualityLevel() == 2)
			{
				return 1;
			}
			return 0;
		}
	}

	public void SetPanelActive(int id, bool fromGame = false)
	{
		this.FromGamePlay = fromGame;
		base.gameObject.SetActive(true);
		for (int i = 0; i < this.popups.Length; i++)
		{
			this.popups[i].SetActive(id == i);
		}
	}

	public void OnClickInputMode(int id)
	{
		//Debug.Log($"Input mode is {id}");
		Singleton<SoundManager>.Instance.Click();
		Singleton<UIManager>.Instance.InputMode = id;
	}

	public void GetFreeItemWithVideo()
	{
	}

	public void AddIAPCoins(int i)
	{
		PlayerDataPersistant.Instance.Coins += Configurations.Instance.iapItems[i].coins;
	}

	public bool FromGamePlay
	{
		get
		{
			return this._fromGame;
		}
		set
		{
			this._fromGame = value;
			this.gamePauseBtns.SetActive(this._fromGame);
		}
	}

	public void OnClickBack()
	{
		Singleton<SoundManager>.Instance.Click();
		base.gameObject.SetActive(false);
		if (this.FromGamePlay)
		{
			this.OnClickQuitResume(false);
		}
	}

	public void OnClickQuitResume(bool quit)
	{
		Singleton<SoundManager>.Instance.Click();
		if (quit)
		{
			Singleton<UIManager>.Instance.QuitFromGame();
		}
		else
		{
			Singleton<UIManager>.Instance.inGamePage.OnClickResume();
		}
		base.gameObject.SetActive(false);
	}

	public void ChangeQualityLevel(int lvl)
	{
		if (lvl == 2)
		{
			lvl = 5;
		}
		if (lvl == 1)
		{
			lvl = 2;
		}
		if (QualitySettings.GetQualityLevel() != lvl)
		{
			QualitySettings.SetQualityLevel(lvl);
			Singleton<GamePlay>.Instance.player.BroadcastMessage("ActivateShadow", lvl != 5, SendMessageOptions.DontRequireReceiver);
		}
		UnityEngine.Debug.LogFormat("Quality Level is {0}", new object[]
		{
			this.QualityLevel
		});
	}

	public void OnClickShare()
    {
		//GP_Socials.Share();
    }

	public GameObject[] popups;

	public GameObject gamePauseBtns;

	public Dropdown controlDropDown;

	public Dropdown qualityDropDown;

	public GameObject ShareButton;


	private bool _fromGame;

	public enum PurchaseItemType
	{
		None = -1,
		Money
	}
}
