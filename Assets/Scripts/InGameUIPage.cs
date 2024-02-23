// dnSpy decompiler from Assembly-CSharp.dll class: InGameUIPage
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGameUIPage : MonoBehaviour
{
	private PlayerMovement pD
	{
		get
		{
			return Singleton<GamePlay>.Instance.player;
		}
	}

	private void Awake()
	{
		this.pauseButton.onClick.AddListener(new UnityAction(this.PauseClicked));
	}

	private void OnEnable()
	{
		Singleton<GamePlay>.Instance.MainCamera.gameObject.SetActive(true);
		//this.slowmoBtn.SetActive(Singleton<GameCore>.Instance.gameMode != GameMode.Multi);
		slowmoBtn.SetActive(false);
		Singleton<UIManager>.Instance.topBar.SetActive(false);
		this.gameOverPage.SetActive(false);
		this.ActivatePausePanel(false);
		this.IsInWrongLane(false);
		this.timerSheetObj.SetActive(false);
		this.timerObj.SetActive(Singleton<GameCore>.Instance.gameMode == GameMode.TimeTrial);
		this.coinsBg.SetActive(Singleton<GameCore>.Instance.gameMode != GameMode.FreeRide);
		Singleton<UIManager>.Instance.inGamePage.pauseButton.interactable = false;
		Singleton<GamePlay>.Instance.OnInitGameplayTime();
		this.UpdateIngameCoinsText();
		this.SetSpeedData(true);
		this.UpdateDistanceTime();
		this.pauseButton.gameObject.SetActive(Singleton<GameCore>.Instance.gameMode != GameMode.Multi);
		this.quitBtn.SetActive(Singleton<GameCore>.Instance.gameMode == GameMode.Multi);
		this.asked4Quit = false;
	}

	private void OnDisable()
	{
		Singleton<UIManager>.Instance.topBar.SetActive(true);
		if (Singleton<GamePlay>.Instance)
		{
			Singleton<GamePlay>.Instance.MainCamera.gameObject.SetActive(false);
		}
	}

	public void NowStartGame()
	{
		this.UpdateIngameCoinsText();
		this.SetSpeedData(true);
		this.UpdateDistanceTime();
	}

	private void UpdateDistanceTime()
	{
		this.timeText.text = GameUtils.MMSSFF(Singleton<GamePlay>.Instance.GamePlayTime);
		this.meterText.text = string.Format("{0:0000000}", this.InterfaceDistance);
	}

	private void LateUpdate()
	{
		this.UpdateSharedData();
		this.updateTimer -= Singleton<TimeManager>.Instance.MasterSource.DeltaTime;
		if (this.updateTimer < 0f)
		{
			this.updateTimer = 0.12f;
			this.UpdateDistanceTime();
		}
		this.SetSpeedData(false);
	}


    private void OnSetAlphaOnCombo(float _currAlpha)
	{
		float num = Mathf.Clamp01(_currAlpha);
	}

	public void UpdateIngameCoinsText()
	{
		this.coinsText.text = Singleton<GamePlay>.Instance.CoinsCollected.ToString();
	}

	public void ActivatePausePanel(bool activate)
	{
		if (activate)
		{
			Singleton<UIManager>.Instance.ShowPauseInGame();
			Singleton<TimeManager>.Instance.MasterSource.Pause();
		}
		else if (Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			Singleton<TimeManager>.Instance.MasterSource.Resume();
		}
	}

	public void PauseClicked()
	{
		base.StartCoroutine(this.PauseGameTask());
	}

	private IEnumerator PauseGameTask()
	{
		Singleton<SoundManager>.Instance.Click();
		yield return new WaitForEndOfFrame();
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Paused);
		this.ActivatePausePanel(true);
		yield break;
	}

	public void TriggerNitro(bool nitroRun)
	{
		Singleton<GamePlay>.Instance.NitroButtonPressed = nitroRun;
	}

	public void TriggerNitroEffect(bool nitroEffect)
	{
		if (Singleton<GamePlay>.Instance.NitroRatio > 0.1f && nitroEffect)
            Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(true);

		else
            Singleton<GamePlay>.Instance.AnimeEffect.gameObject.SetActive(false);
    }

	public void TriggerSlowMo()
	{
		Singleton<GamePlay>.Instance.slowmoed = !Singleton<GamePlay>.Instance.slowmoed;
	}

	public void MissionCompleted()
	{
		Singleton<SoundManager>.Instance.PlayGeneralGameSound(SoundManager.GeneralGameSounds.MissionCompleted);
	}

	public void OnLeftPointer(bool down)
	{
		this.pD.OnLeftInput(down);
	}

	public void OnRightPointer(bool down)
	{
		this.pD.OnRightInput(down);
	}

	public void OnUpPointer(bool down)
	{
		this.pD.OnUpInput(down);
	}

	public void OnDownPointer(bool down)
	{
		this.pD.OnDownInput(down);
	}

	public void GameOver()
	{
		this.gameOverPage.SetActive(true);
	}

	private void SetSpeedData(bool first = false)
	{
		if (first)
		{
			this.speedText.text = "0";
		}
		else
		{
			if (Mathf.Abs(this.pD.Speed - this.prevVal) > 1f)
			{
				this.prevVal = this.pD.Speed;
				this.speedText.text = Mathf.CeilToInt(this.prevVal * 3f).ToString();
			}
			this.highSpeedObj.SetActive(this.pD.Speed > 63f);
		}
	}

	public void OnClickResume()
	{
		this.ActivatePausePanel(false);
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Racing);
	}

	public void SetActive(bool state)
	{
		base.gameObject.SetActive(state);
	}

	public void RessurectPlayer()
	{
		Singleton<GamePlay>.Instance.RefillHealth();
		Singleton<GamePlay>.Instance.RessurectingPlayerNow(10);
		this.gameOverPage.SetActive(false);
	}

	public int InterfaceDistance
	{
		get
		{
			return this.interfaceDistance;
		}
	}

	public void OnRestartRace()
	{
		this.interfaceDistance = 0;
	}

	public void IsInWrongLane(bool val)
	{
		this.wrongLaneObj.SetActive(val);
	}

	public void ShowHideOnTimerSheet(bool isPositive, bool status)
	{
		this.isPositiveTimer = isPositive;
		this.timerSheetObj.SetActive(status);
	}

	public void UpdateSharedData()
	{
		if (!Singleton<TimeManager>.Instance.MasterSource.IsPaused && (Singleton<GamePlay>.Instance.GamePlayStarted || this.pD.PlayerIsStopping) && this.pD.Distance > 0f)
		{
			this.interfaceDistance = (int)this.pD.Distance;
		}
	}

	private bool asked4Quit
	{
		get
		{
			return this._quitAsked;
		}
		set
		{
			this._quitAsked = value;
			this.quitSureObj.SetActive(this._quitAsked);
		}
	}

	public void OnClickQuitMulti()
	{
		if (!this.asked4Quit)
		{
			base.StartCoroutine(this.WaitAndHide());
		}
		else
		{
			Singleton<UIManager>.Instance.QuitFromGame();
		}
	}

	private IEnumerator WaitAndHide()
	{
		this.asked4Quit = true;
		yield return new WaitForSeconds(3f);
		this.asked4Quit = false;
		yield break;
	}

	public Text coinsText;

	public Text nitroCountText;

	public Text speedText;

	public Text meterText;

	public Text timeText;

	public GameObject highSpeedObj;

	public GameObject wrongLaneObj;

	public GameObject coinsBg;

	public GameObject timerObj;

	public GameObject slowmoBtn;

	public MiniTimeTrialTimer timerSheetObj;

	public GameOverPage gameOverPage;

	public Image nitroBarIndicator;

	public Image slowmoBar;

	public Button pauseButton;

	private float updateTimer = 0.1f;

	private float prevVal;

	public GameObject[] inputModeUI;

	private int interfaceDistance;

	internal float timeWrongLane;

	internal float timeHighSpeed;

	internal bool isPositiveTimer;

	public GameObject quitSureObj;

	public GameObject quitBtn;

	private bool _quitAsked;
}
