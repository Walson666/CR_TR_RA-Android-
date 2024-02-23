// dnSpy decompiler from Assembly-CSharp.dll class: GameOverPage
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPage : MonoBehaviour
{
	private void Awake()
	{
		this.gameOverItems = this.gameOverItemsParent.GetComponentsInChildren<GameOverItem>();
	}

	private void ComputeProgress()
	{
		this.gameOverItems[0].SetValue(Singleton<UIManager>.Instance.inGamePage.InterfaceDistance);
		this.gameOverItems[1].SetValue(Singleton<GamePlay>.Instance.nearMissCount);
		this.gameOverItems[2].SetValue((int)Singleton<UIManager>.Instance.inGamePage.timeHighSpeed);
		this.gameOverItems[3].SetValue(Singleton<GamePlay>.Instance.CoinsCollected);
		Singleton<UIManager>.Instance.inGamePage.timeWrongLane = 0f;
		this.gameOverItems[4].SetValue(Singleton<GameCore>.Instance.gameMode == GameMode.Double, (int)Singleton<UIManager>.Instance.inGamePage.timeWrongLane);
		if (Singleton<GameCore>.Instance.gameMode == GameMode.Double && (int)Singleton<UIManager>.Instance.inGamePage.timeWrongLane >= 30)
		{
			PlayGameCenterManager.Instance.UnlockAchievement("CgkIj5y4oIoOEAIQAw", 100.0);
		}
		if (Singleton<GameCore>.Instance.gameMode == GameMode.TimeTrial && Singleton<UIManager>.Instance.inGamePage.InterfaceDistance >= 5000)
		{
			PlayGameCenterManager.Instance.UnlockAchievement("CgkIj5y4oIoOEAIQBA", 100.0);
		}
		this.totalCurrency = 0;
		foreach (GameOverItem gameOverItem in this.gameOverItems)
		{
			this.totalCurrency += gameOverItem.currency;
		}
		this.totalScore = Mathf.CeilToInt((float)(Singleton<UIManager>.Instance.inGamePage.InterfaceDistance + Singleton<GamePlay>.Instance.nearMissCount) + Singleton<UIManager>.Instance.inGamePage.timeHighSpeed + (float)Singleton<GamePlay>.Instance.CoinsCollected + ((Singleton<GameCore>.Instance.gameMode != GameMode.Double) ? 0f : Singleton<UIManager>.Instance.inGamePage.timeWrongLane));
		this.totalCurrencyWonText.text = GameUtils.GetValueFormated(this.totalCurrency);
		base.StopAllCoroutines();
		base.StartCoroutine(this.IncrementCoroutine(this.totalCurrencyWonText, this.totalCurrency, 0));
		int gameMode = (int)Singleton<GameCore>.Instance.gameMode;
		if (gameMode < 3 && PlayerDataPersistant.Instance.gameModeScores[gameMode] < this.totalScore)
		{
			PlayerDataPersistant.Instance.gameModeScores[gameMode] = this.totalScore;
			/*if(gameMode == 1)
			{
				if(GP_Player.IsLoggedIn())
				{
					GP_Player.Set("score", PlayerDataPersistant.Instance.gameModeScores[gameMode]);
				}
                //Debug.LogError("NEW RECORD 1");
            }*/
			/*if (gameMode != 0)
			{
				if (gameMode != 1)
				{
					if (gameMode == 2)
					{
						PlayGameCenterManager.Instance.PostToLeaderboard((long)this.totalScore, "CgkIj5y4oIoOEAIQAg");
						Debug.LogError("NEW RECORD 1");
					}
				}
				else
				{
					PlayGameCenterManager.Instance.PostToLeaderboard((long)this.totalScore, "CgkIj5y4oIoOEAIQAQ");
				}
			}
			else
			{
				PlayGameCenterManager.Instance.PostToLeaderboard((long)this.totalScore, "CgkIj5y4oIoOEAIQAA");
			}*/
		}
		PlayerDataPersistant.Instance.Coins += Singleton<GamePlay>.Instance.CoinsCollected;
		if (Singleton<GameCore>.Instance.gameMode != GameMode.FreeRide)
		{
			PlayerDataPersistant.Instance.LevelProgress += (float)Singleton<UIManager>.Instance.inGamePage.InterfaceDistance * UnityEngine.Random.Range(0.001f, 0.005f);
		}
	}

	private void OnDisable()
	{
		if (Singleton<TimeManager>.Instance != null && Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			Singleton<TimeManager>.Instance.MasterSource.Resume();
		}
	}

	private void OnEnable()
	{
		this.ShowGameOver(!Singleton<GamePlay>.Instance.canRessurect);
	}

	public void OnClickAgainstCoins()
	{
		if (PlayerDataPersistant.Instance.CanAfford(500f))
		{
			PlayerDataPersistant.Instance.Coins -= 500;
			Singleton<UIManager>.Instance.inGamePage.RessurectPlayer();
		}
		else
		{
			Singleton<UIManager>.Instance.ShowPurchaseScreen();
		}
	}

	public void PlayAd()
	{
	}

	public void DontContinue()
	{
		this.ShowGameOver(true);
	}

	private void ShowGameOver(bool endGame)
	{
		this.saveMePanel.SetActive(!endGame);
		this.gameOverPanel.SetActive(endGame);
		if (endGame)
		{
			if (Singleton<TimeManager>.Instance.MasterSource.IsPaused)
			{
				Singleton<TimeManager>.Instance.MasterSource.Resume();
			}
			this.ComputeProgress();
            PlayerDataPersistant.Instance.Coins += totalCurrency;
            PlayerDataPersistant.Instance.SaveGameData(true);
			//Debug.LogError("GAMEOVER");
        }
		else
		{
			Singleton<TimeManager>.Instance.MasterSource.Pause();
		}
	}

	public void OnClickQuitRestart(bool quit)
	{
		if (quit)
		{
			Singleton<UIManager>.Instance.QuitFromGame();
            rewardManager.ShowInterstitialAd();
        }
		else
		{
            if (Singleton<TimeManager>.Instance.MasterSource.IsPaused)
            {
                Singleton<TimeManager>.Instance.MasterSource.Resume();
            }
            base.gameObject.SetActive(false);
            Singleton<UIManager>.Instance.RestartFromGame();
        }
        //Debug.LogError("Coins count:" + totalCurrency);
    }

	public void SetActive(bool state)
	{
		base.gameObject.SetActive(state);
	}

	private IEnumerator IncrementCoroutine(Text l, int targetValue, int startingValue = 0)
	{
		float time = 0f;
		l.text = startingValue.ToString();
		float incrementTime = 1.2f;
		while (time < incrementTime)
		{
			yield return null;
			time += Time.deltaTime;
			float factor = time / incrementTime;
			l.text = GameUtils.GetValueFormated((int)Mathf.Lerp((float)startingValue, (float)targetValue, factor));
		}
		l.text = targetValue.ToString();
		yield break;
	}

	public Button saveMeBtn;

	public GameObject saveMePanel;

	public GameObject gameOverPanel;

	public Transform gameOverItemsParent;

	public Text totalCurrencyWonText;

	private GameOverItem[] gameOverItems;

	private int totalCurrency;

	private int totalScore;

	public const int SAVE_ME_COST = 500;

	public RewardManager rewardManager;
}
