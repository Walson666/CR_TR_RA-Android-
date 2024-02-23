// dnSpy decompiler from Assembly-CSharp.dll class: GameCore
using System;
using System.Collections;
using UnityEngine;

public class GameCore : Singleton<GameCore>
{
	public int CurrentWorld
	{
		get
		{
			return this._currWorld;
		}
		set
		{
			this._currWorld = value;
		}
	}

	public string TrackObjectName
	{
		get
		{
			return "THISISWORLD" + this.CurrentWorld;
		}
	}

	public Configurations.WorldInfo CurrentWorldInfo
	{
		get
		{
			return Configurations.Instance.worldInfo[this.CurrentWorld];
		}
	}

	private void Awake()
	{
		GameCore.lanes = this.CalculateLanesNonPlayer();
		PlayerDataPersistant.Instance.Load();
	}

	private void Start()
	{
		Application.targetFrameRate = 60;
		Screen.sleepTimeout = -1;
		Singleton<LevelManager>.Instance.Initialize();
	}

	private void OnApplicationPause(bool paused)
	{
		//Debug.LogError("Game SAVE PAUSE");
		Screen.sleepTimeout = ((!paused) ? -1 : -2);
		if (paused)
		{
			//PlayerDataPersistant.Instance.SaveGameData();
		}
		else
		{
			//PlayerDataPersistant.Instance.Load();
			base.StartCoroutine(this.UpdateDeviceMusicPlayingDelayed());
		}
	}

	private IEnumerator UpdateDeviceMusicPlayingDelayed()
	{
		yield return new WaitForEndOfFrame();
		if (Singleton<SoundManager>.Instance != null)
		{
			Singleton<SoundManager>.Instance.UpdateDeviceMusicPlaying();
		}
		yield break;
	}

	public void CreateWorld(int id)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.gameWorldPrefab).name = "THISISWORLD" + id;
		this.previousWorldName = "THISISWORLD" + id;
	}

	private void OnApplicationQuit()
	{
	}

	public static float[] CalculateLanes()
	{
		int num = 15;
		float num2 = 1.5f / (float)(num - 1);
		float[] array = new float[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = -0.75f + (float)i * num2;
		}
		return array;
	}

	public float[] CalculateLanesNonPlayer()
	{
		int num = 5;
		float num2 = 1.5f / (float)(num - 1);
		float[] array = new float[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = -0.75f + (float)i * num2;
		}
		return array;
	}

	public GameObject gameWorldPrefab;

	public EndlessBlock defaultEndlessBlock;

	internal GameMode gameMode;

	private int _currWorld;

	[HideInInspector]
	public GameObject selectedCar;

	[HideInInspector]
	public string previousWorldName;

	public const string PLAYER = "Player";

	public const string GAME_WORLD_PREFIX = "THISISWORLD";

	public const float LANE_MULT = 9f;

	public static float[] lanes;

	public static float[] playerLanes = GameCore.CalculateLanes();

	public const int LANE_COUNT = 5;

	internal float CurrScaleValue = 1f;
}
