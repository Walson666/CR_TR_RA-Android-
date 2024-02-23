// dnSpy decompiler from Assembly-CSharp.dll class: UIManager
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	public UIScreens currPage
	{
		get
		{
			return this._currPage;
		}
		set
		{
			this._currPage = value;
		}
	}

	public static UIGaragePage GaragePage
	{
		get
		{
			return Singleton<UIManager>.Instance.garagePage;
		}
	}

	public void UpdateCoinsCount()
	{
		string valueFormated = GameUtils.GetValueFormated(PlayerDataPersistant.Instance.Coins);
		for (int i = 0; i < this.coinCountText.Length; i++)
		{
			this.coinCountText[i].text = valueFormated;
		}
	}

	public void ShowPage(int i)
	{
		this.ShowPage((UIScreens)i);
	}

	public void ShowPage(UIScreens newPage)
	{
		this.currPage = newPage;
		switch (this.currPage)
		{
		case UIScreens.FirstLand:
			this.landingPage.SetActive(true);
			this.morePanels.HideAllMorePanels();
			UIManager.GaragePage.SetActive(false, true);
			this.inGamePage.SetActive(false);
			this.popupsMan.gameObject.SetActive(false);
			break;
		case UIScreens.GarageScreen:
			this.landingPage.SetActive(false);
			this.morePanels.HideAllMorePanels();
			UIManager.GaragePage.SetActive(true, true);
			this.inGamePage.gameObject.SetActive(false);
			break;
		case UIScreens.Init_Booster:
			this.morePanels.HideAllMorePanels();
			this.inGamePage.gameObject.SetActive(false);
			this.gameInitializer.gameObject.SetActive(true);
			break;
		case UIScreens.PlayModeScreen:
			this.inGamePage.gameObject.SetActive(true);
			break;
		case UIScreens.SpinUI:
			this.morePanels.HideAllMorePanels();
			this.landingPage.SetActive(false);
			this.inGamePage.gameObject.SetActive(false);
			this.garagePage.SetActive(false, false);
			break;
		}
	}

	public void ShowPurchaseScreen()
	{
		this.ShowPopup(1, false);
	}

	private void OnCheckpointEnded()
	{
		Singleton<SoundManager>.Instance.PlayMetersOk();
	}

	public void OnCheckpointEnter()
	{
		Singleton<SoundManager>.Instance.PlayCheckpointSound();
		UnityEngine.Debug.Log("Checkpoint Entered");
		if (Singleton<GameCore>.Instance.gameMode == GameMode.Multi)
		{
			Singleton<GamePlay>.Instance.FinishGame();
		}
		else
		{
			Singleton<GamePlay>.Instance.OnUpdateGameplayTime(false);
			Singleton<GamePlay>.Instance.player.OnCheckpointEnter();
		}
	}

	private void Awake()
	{
        //Changing input mode;
        //this.InputMode = PlayerPrefs.GetInt("INPUT_MODE", 3);
        this.InputMode = 1;
        /*if (GP_Device.IsMobile())
		{
			this.InputMode = 1;

        }
        if (GP_Device.IsDesktop())
        {
            this.InputMode = 3;

        }*/
    }

	private void Start()
	{
		this.ShowPage(UIScreens.FirstLand);
	}

	public void QuitFromGame()
	{
		this.QuitGame();
		this.ShowPage(UIScreens.GarageScreen);
	}

	internal void QuitGame()
	{
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Offgame);
		Singleton<GamePlay>.Instance.GamePlayStarted = false;
		Singleton<GamePlay>.Instance.ResetEnvironment();
	}

	internal void ShowGameOverMulti()
	{
		UIManager.GaragePage.SetActive(false, true);
		this.inGamePage.SetActive(false);
		this.morePanels.MultiMode(false);
	}

	public void RestartFromGame()
	{
		this.ShowPage(UIScreens.Init_Booster);
	}

	public void SetTrafficDirection(int i)
	{
		Singleton<GameCore>.Instance.gameMode = (GameMode)i;
		this.ShowPage(UIScreens.Init_Booster);
	}

	public void SetGameWorld(int i)
	{
		Singleton<GameCore>.Instance.CurrentWorld = i;
		this.ShowPage(UIScreens.Init_Booster);
	}

	public void ShowPopup(int id, bool pause = false)
	{
		this.currPage = UIScreens.PopupScreen;
		this.popupsMan.SetPanelActive(id, pause);
	}

	public void ShowPauseInGame()
	{
		this.ShowPopup(0, true);
	}

	public int InputMode
	{
		get
		{
			return this._inputMode;
		}
		set
		{
			this._inputMode = Mathf.Clamp(value, 0, 2);
			for (int i = 0; i < this.inGamePage.inputModeUI.Length; i++)
			{
				this.inGamePage.inputModeUI[i].SetActive(i == this._inputMode);
			}
			PlayerPrefs.SetInt("INPUT_MODE", this._inputMode);
			PlayerPrefs.Save();
		}
	}

	public GameObject topBar;

	public GameObject landingPage;

	public GameObject gameModeMult;

	public MorePanels morePanels;

	public UIGaragePage garagePage;

	public InGameUIPage inGamePage;

	public GameInitializer gameInitializer;

	public PopupsMan popupsMan;

	public UIDailyBonusPage dailyBonusPage;

	public Text[] coinCountText;

	public Text playerNameText;

	public Text playerLevelText;

	public Image levelProgressBar;

	private UIScreens _currPage;

	public static float delayFactor;

	public static float changePageDelay = 0.1f;

	public const string INPUT_MODE = "INPUT_MODE";

	private int _inputMode;
}
