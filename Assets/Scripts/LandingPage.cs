// dnSpy decompiler from Assembly-CSharp.dll class: LandingPage
using System;
using UnityEngine;

public class LandingPage : MonoBehaviour
{
	public void OnClickPlay()
	{
		Singleton<UIManager>.Instance.ShowPage(UIScreens.GarageScreen);
	}

	public void OnClickRate()
	{
		string str = "com.curio.crazytrafficracing";
		string url = "market://details?id=" + str;
		Application.OpenURL(url);
	}

	public void OnClickOptions()
	{
		Singleton<UIManager>.Instance.ShowPopup(0, false);
	}

	public void OnClickAchievements()
	{
		PlayGameCenterManager.Instance.ShowAchievementsUI();
	}

	public void OnClickLeaderboard()
	{
		PlayGameCenterManager.Instance.ShowLeaderboardUI();
	}

	public void SetActive(bool state)
	{
		base.gameObject.SetActive(state);
	}

	public void OnClickChooseGame(bool arenaM)
	{
		LandingPage.arenaMode = arenaM;
		Singleton<UIManager>.Instance.ShowPage(UIScreens.GarageScreen);
	}

	public static bool arenaMode;
}
