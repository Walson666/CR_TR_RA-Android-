// dnSpy decompiler from Assembly-CSharp.dll class: MultiModeUI
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MultiModeUI : MonoBehaviour
{
	private void Start()
	{
		this.player1.pCountryText.text = RegionInfo.CurrentRegion.DisplayName;
	}

	private void OnEnable()
	{
		if (this.gameOverMulti)
		{
			this.ResetPlayer2OnGameOver();
			bool flag = this.player1.timeReached <= this.player2.timeReached;
			this.player1.winnerScreen.gameObject.SetActive(true);
			this.player2.winnerScreen.gameObject.SetActive(true);
			this.player1.winnerScreen.sprite = this.bgWinLooseInfo[(!flag) ? 0 : 1];
			this.player2.winnerScreen.sprite = this.bgWinLooseInfo[(!flag) ? 1 : 0];
			this.player1.winnderObj.SetActive(flag);
			this.player2.winnderObj.SetActive(!flag);
			this.chooseScreen.SetActive(false);
			this.SetCoinAnim(flag, !flag, 1, 1);
			base.StartCoroutine(this.IncrementCoroutine(this.betCostText, 0, Configurations.Instance.bets[MultiModeUI.multGameMode].betPrice * 2));
			if (flag)
			{
				PlayerDataPersistant.Instance.Coins += Configurations.Instance.bets[MultiModeUI.multGameMode].betPrice * 2;
			}
		}
		else
		{
			this.searh_delay = UnityEngine.Random.Range(2f, 4f);
			if (this.dummyPreview)
			{
				UnityEngine.Object.Destroy(this.dummyPreview);
			}
			this.betCostText.text = string.Empty;
			this.SetCoinAnim(false, false, 0, 0);
			this.player2.SetActive(false);
			this.chooseScreen.SetActive(true);
			this.player1.Refresh();
			this.player1.winnerScreen.gameObject.SetActive(false);
			this.player2.winnerScreen.gameObject.SetActive(false);
		}
		this.backBtn.SetActive(true);
		this.vsIcon.SetActive(false);
		this.featureUnderDev.SetActive(false);
		this.myCam.SetActive(true);
		this.player1.pLevelText.text = PlayerDataPersistant.Instance.Level.ToString();
		this.player1.worldText.text = this.selectedWorldInfo.worldName;
		this.player2.worldText.text = this.selectedWorldInfo.worldName;
	}

	private Configurations.WorldInfo selectedWorldInfo
	{
		get
		{
			return Configurations.Instance.worldInfo[Configurations.Instance.bets[MultiModeUI.multGameMode].worldId];
		}
	}

	private void OnDisable()
	{
		this.myCam.SetActive(false);
	}

	internal void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
	}

	public void OnClickMatchRandom()
	{
		this.chooseScreen.SetActive(false);
		base.StartCoroutine(this.WaitForSearch());
	}

	public IEnumerator WaitForSearch()
	{
		this.searchScreen.SetActive(true);
		yield return new WaitForSeconds(this.searh_delay);
		this.PostSearch();
		yield break;
	}

	private void SetCoinAnim(bool firstOn, bool secondOn, int first, int second)
	{
		this._animP1Coins.gameObject.SetActive(firstOn);
		this._animP2Coins.gameObject.SetActive(secondOn);
		if (firstOn)
		{
			this._animP1Coins.SetInteger(this.WON_STR, first);
		}
		if (secondOn)
		{
			this._animP2Coins.SetInteger(this.WON_STR, second);
		}
	}

	private void PostSearch()
	{
		this.searchScreen.SetActive(false);
		this.player2.SetActive(true);
		this.SetCoinAnim(true, true, -1, -1);
		this.backBtn.SetActive(false);
		this.vsIcon.SetActive(true);
		this.player2.Dummy();
		int num = UnityEngine.Random.Range(0, Singleton<GamePlay>.Instance.playerCars.Length);
		int num2 = UnityEngine.Random.Range(0, 4);
		int i = UnityEngine.Random.Range(0, 15);
		int num3 = UnityEngine.Random.Range(0, 17);
		CarPreview carPreview = UnityEngine.Object.Instantiate<CarPreview>(UIManager.GaragePage.carCustomize.carPreviews[num], this.myCam.transform);
		this.player2.playerImg.sprite = this.searchScreen.RandomDummyAvatar;
		this.player2.pCountryText.text = this.randomCountryName[UnityEngine.Random.Range(0, this.randomCountryName.Length)];
		this.player2.pNameText.text = this.randomPlayerName[UnityEngine.Random.Range(0, this.randomPlayerName.Length)];
		this.player2.pLevelText.text = UnityEngine.Random.Range(1, 100).ToString();
		carPreview.SetActive(true);
		carPreview.UpdateColor(i);
		carPreview.UpdateRim(num2);
		carPreview.UpdateSticker(num3);
		carPreview.RemoveUselessStuff(num2, num3);
		UnityEngine.Object.Destroy(carPreview.GetComponent<CarPreview>());
		this.dummyPreview = carPreview.gameObject;
		this.dummyPreview.transform.localEulerAngles = this.player2Transform.localEulerAngles;
		this.dummyPreview.transform.localPosition = this.player2Transform.localPosition;
		PlayerDataPersistant.Instance.BuyItem(Configurations.Instance.bets[MultiModeUI.multGameMode].betPrice);
		base.StartCoroutine(this.IncrementCoroutine(this.betCostText, Configurations.Instance.bets[MultiModeUI.multGameMode].betPrice * 2, 0));
	}

	public void ResetPlayer2OnGameOver()
	{
		this.dummyPreview.transform.SetParent(this.myCam.transform);
		this.dummyPreview.transform.localEulerAngles = this.player2Transform.localEulerAngles;
		this.dummyPreview.transform.localPosition = this.player2Transform.localPosition;
		this.dummyPreview.transform.localScale = Vector3.one;
	}

	private IEnumerator IncrementCoroutine(Text l, int targetValue, int startingValue = 0)
	{
		float time = 0f;
		l.text = startingValue.ToString();
		float incrementTime = 1f;
		while (time < incrementTime)
		{
			yield return null;
			time += Time.deltaTime;
			float factor = time / incrementTime;
			l.text = GameUtils.GetValueFormated((int)Mathf.Lerp((float)startingValue, (float)targetValue, factor));
		}
		l.text = targetValue.ToString();
		if (!this.gameOverMulti)
		{
			this.CreateMulCar();
		}
		yield break;
	}

	private void CreateMulCar()
	{
		Singleton<GameCore>.Instance.CurrentWorld = Configurations.Instance.bets[MultiModeUI.multGameMode].worldId;
		Singleton<UIManager>.Instance.garagePage.SetActive(false, false);
		Singleton<UIManager>.Instance.SetTrafficDirection(4);
	}

	public void CancelSearch()
	{
		base.StopAllCoroutines();
		this.searchScreen.SetActive(false);
		this.player2.SetActive(false);
		this.chooseScreen.SetActive(true);
	}

	public void OnClickBack()
	{
		Singleton<UIManager>.Instance.morePanels.MultiMap();
		this.CancelSearch();
	}

	public Text betCostText;

	public UIPlayerInfoMul player1;

	public UIPlayerInfoMul player2;

	public GameObject featureUnderDev;

	public GameObject myCam;

	public GameObject chooseScreen;

	public GameObject backBtn;

	public GameObject vsIcon;

	public Animator _animP1Coins;

	public Animator _animP2Coins;

	public SearchPlayer searchScreen;

	public Transform player2Transform;

	public Sprite[] bgWinLooseInfo;

	public static int multGameMode;

	internal bool gameOverMulti;

	private string WON_STR = "won";

	private string[] randomPlayerName = new string[]
	{
		"limelight",
		"peternon",
		"bricksons",
		"jhona",
		"marker_zinger",
		"fromtheland",
		"dilkesh",
		"brandonara",
		"hushbushno",
		"saltypitch",
		"sweetishdel",
		"progama",
		"natasha",
		"fromtheotherend",
		"yesornot",
		"noandnever",
		"recingdelight"
	};

	private string[] randomCountryName = new string[]
	{
		"China",
		"Denmark",
		"Netherlands",
		"Yogosalava",
		"Britian",
		"United States",
		"India",
		"Iran",
		"Russia",
		"Iraq",
		"Pakistan",
		"Saudi Arabia",
		"UAE",
		"Turkey",
		"Kambodia",
		"Maldeves",
		"Moroco",
		"Canada",
		"Isreal",
		"North Korea"
	};

	private float searh_delay = 1.5f;

	internal GameObject dummyPreview;
}
