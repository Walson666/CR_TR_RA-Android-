// dnSpy decompiler from Assembly-CSharp.dll class: UIGaragePage
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIGaragePage : MonoBehaviour
{
	public void MoveLeftRightGarage(bool isNext)
	{
		Singleton<SoundManager>.Instance.Click();
		if (isNext)
		{
			if (this.CurrCarID < Singleton<GamePlay>.Instance.playerCars.Length - 1)
			{
				this.CurrCarID++;
			}
			else
			{
				this.CurrCarID = 0;
			}
		}
		else if (this.CurrCarID > 0)
		{
			this.CurrCarID--;
		}
		else
		{
			this.CurrCarID = Singleton<GamePlay>.Instance.playerCars.Length - 1;
		}
	}

	private void OnEnable()
	{
		this.ChangeSelectedCar();
		CurrCarID = PlayerDataPersistant.Instance.CurentCar;
	}

	public void ChangeSelectedCar()
	{
		this.carCustomize.Refresh();
		this.priceTxt.text = GameUtils.GetValueFormated(this.SelectedCar.cost);
		PlayerCustomizeData playerData = PlayerDataPersistant.Instance.GetPlayerData(this.SelectedCar.carID);
		this.carCustomize.SetActive(playerData.owned);
		this.purchaseUI.SetActive(!playerData.owned);
	}

	public int CurrCarID
	{
		get
		{
			return this._carID;
			//return PlayerDataPersistant.Instance.CurentCar;
		}
		set
		{
			this._carID = value;
			this.ChangeSelectedCar();
			PlayerDataPersistant.Instance.CurentCar = value;

        }
	}

	public CarData SelectedCar
	{
		get
		{
			return Singleton<GamePlay>.Instance.playerCars[this.CurrCarID];
		}
	}

	public void OnClickNextScreen()
	{
		//Debug.LogError("Next");
		Singleton<SoundManager>.Instance.Click();
		PlayerDataPersistant.Instance.SaveGameData(false);
		Singleton<SoundManager>.Instance.Click();
		Singleton<GameCore>.Instance.selectedCar = this.SelectedCar.car.gameObject;
		if (LandingPage.arenaMode)
		{
			Singleton<UIManager>.Instance.morePanels.MultiMap();
		}
		else
		{
			Singleton<UIManager>.Instance.morePanels.SingleMap();
		}
	}

	public void OnClickBuy()
	{
		Singleton<SoundManager>.Instance.Click();
		if (PlayerDataPersistant.Instance.CanAfford((float)this.SelectedCar.cost))
		{
			PlayerDataPersistant.Instance.BuyItem(this.SelectedCar.cost);
			PlayerDataPersistant.Instance.UpdateValues(this.SelectedCar.car.customizationData);
			this.ChangeSelectedCar();
		}
		else
		{
			Singleton<UIManager>.Instance.ShowPurchaseScreen();
		}
	}

	public void OnClickBackBtn()
	{
		Singleton<SoundManager>.Instance.Click();
		Singleton<UIManager>.Instance.ShowPage(UIScreens.FirstLand);
	}

	public void SetupObjectsForNewCamera()
	{
		base.gameObject.SetActive(true);

		this.mobileGarageCam.enabled = false;
		this.pcGarageCam.enabled = false;

		this.UICanvas.SetActive(false);
		this.carCustomize.previewParent.SetActive(true);
	}

	public void SetActive(bool ui, bool state)
	{
		base.gameObject.SetActive(state);
		if (state)
		{
			this.carCustomize.previewParent.SetActive(true);
			this.UICanvas.SetActive(ui);
			base.StopAllCoroutines();
			if (!ui)
			{
				this.mobileGarageCam.enabled = ui;
				this.pcGarageCam.enabled =ui;

			}
			base.StartCoroutine(this.ChangeTransform(this.pcGarageCam.transform, ui, 0.5f));
		}
	}

	public IEnumerator ChangeTransform(Transform obj, bool enableCamOrbit, float delay = 0.5f)
	{
		Transform targetObj = (!enableCamOrbit) ? this.landTrans : this.garageTrans;
		float elapsedTime = 0f;
		while (elapsedTime < delay)
		{
			this.et = elapsedTime / delay;
			obj.localPosition = Vector3.Lerp(obj.localPosition, targetObj.localPosition, Mathf.SmoothStep(0f, 1f, this.et));
			obj.localRotation = Quaternion.Slerp(obj.localRotation, targetObj.localRotation, Mathf.SmoothStep(0f, 1f, this.et));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		obj.localPosition = targetObj.localPosition;
		obj.localRotation = targetObj.localRotation;
		this.pcGarageCam.enabled = enableCamOrbit;
		yield break;
	}

	public CameraOrbit mobileGarageCam;
	public CarOrbitCamera pcGarageCam;


	public const int CAR_UPGRADE_COST = 100;

	public GameObject UICanvas;

	public GameObject purchaseUI;

	public Text priceTxt;

	public CarCustomizeManager carCustomize;

	private int _carID;

	private float et;

	public Transform landTrans;

	public Transform garageTrans;
}
