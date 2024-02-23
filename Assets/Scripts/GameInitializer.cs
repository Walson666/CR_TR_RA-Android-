// dnSpy decompiler from Assembly-CSharp.dll class: GameInitializer
using System;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
	private void OnEnable()
	{
		this.OnClickedPlay();
	}

	public void OnClickedPlay()
	{
		Singleton<GamePlay>.Instance.ReactivatePlayer();
		Singleton<GamePlay>.Instance.ResetRunParameter();
		Singleton<GamePlay>.Instance.LoadNewLevel(false);
	}

	public void LevelLoaded()
	{
		Singleton<UIManager>.Instance.ShowPage(UIScreens.PlayModeScreen);
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Racing);
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Start);
		this.ActivatePlayer(Singleton<GameCore>.Instance.selectedCar, true);
		Singleton<GamePlay>.Instance.CreatePlayerCarByRef(Singleton<GameCore>.Instance.selectedCar);
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.ReadyToRace);
		Singleton<GamePlay>.Instance.ChangeState(GamePlay.GameplayStates.Racing);
		this.GoNow();
	}

	public void GoNow()
	{
		Singleton<UIManager>.Instance.inGamePage.NowStartGame();
		Singleton<GamePlay>.Instance.StartReadyGoSequence();
		base.gameObject.SetActive(false);
	}

	private void ActivatePlayer(GameObject playerRef, bool activate)
	{
		playerRef.SetActive(activate);
		playerRef.tag = ((!activate) ? "Untagged" : "Player");
		playerRef.GetComponent<BoxCollider>().enabled = activate;
		playerRef.GetComponent<EndlessPlayer>().enabled = activate;
		playerRef.GetComponent<PlayerSounds>().enabled = activate;
		GameObject gameObject = playerRef.transform.Find("veh_shadow").gameObject;
		gameObject.GetComponent<PlayerShadow>().enabled = activate;
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.GetComponent<PlayerShadow>().shadowVerticalOffset, gameObject.transform.localPosition.z);
	}
}
