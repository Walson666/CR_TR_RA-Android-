// dnSpy decompiler from Assembly-CSharp.dll class: MorePanels
using System;
using UnityEngine;
using UnityEngine.UI;
//using YG;

public class MorePanels : MonoBehaviour
{
	public void HideAllMorePanels()
	{
		base.gameObject.SetActive(false);
		this.multiModeCanvas.SetActive(false);
	}

	public void SetActive(int popupId, bool root = true)
	{
		base.gameObject.SetActive(root);
		if (root)
		{
			this.multiModeCanvas.SetActive(false);
			for (int i = 0; i < this.screens.Length; i++)
			{
				this.screens[i].SetActive(i == popupId);
			}
		}
	}

	public void SingleMap()
	{
		UIManager.GaragePage.SetActive(false, false);
		this.SetActive(0, true);
	}

	public void SingleMode()
	{
		this.SetActive(1, true);
		for (int i = 0; i < this.bestScoreTexts.Length; i++)
		{
			string text = string.Empty;
			//switch (YandexGame.EnvironmentData.language)
			text = Singleton<Translation>.Instance.Get("BEST");
			this.bestScoreTexts[i].text = string.Format("{1}: {0}", GameUtils.GetValueFormated(PlayerDataPersistant.Instance.gameModeScores[i]), text);
		}
	}

	public void MultiMap()
	{
		UIManager.GaragePage.SetActive(false, false);
		this.SetActive(2, true);
	}

	public void MultiMode(bool init = true)
	{
		this.multiModeCanvas.gameOverMulti = !init;
		UIManager.GaragePage.SetupObjectsForNewCamera();
		base.gameObject.SetActive(false);
		this.multiModeCanvas.SetActive(true);
	}

	public GameObject[] screens;

	public MultiModeUI multiModeCanvas;

	public Text[] bestScoreTexts;
}
