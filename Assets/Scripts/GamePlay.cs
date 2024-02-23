// dnSpy decompiler from Assembly-CSharp.dll class: GamePlay
using System;
using System.Collections;
using UnityEngine;

public class GamePlay : Singleton<GamePlay>
{
	public float NitroRatio
	{
		get
		{
			return this._nitroRatio;
		}
		set
		{
			this._nitroRatio = Mathf.Clamp(value, -0.1f, 1f);
			Singleton<UIManager>.Instance.inGamePage.nitroBarIndicator.fillAmount = Mathf.Clamp(this._nitroRatio, 0f, 1f);
			if (this.NitroRatio < 0f || !this.NitroButtonPressed)
			{
				this.player.OnNitroActive(false);
			}
		}
	}

	private float SlowmoRatio
	{
		get
		{
			return this._slowmoRatio;
		}
		set
		{
			this._slowmoRatio = Mathf.Clamp(value, -0.1f, 1f);
			Singleton<UIManager>.Instance.inGamePage.slowmoBar.fillAmount = Mathf.Clamp(this._slowmoRatio, 0f, 1f);
			if (this._slowmoRatio < 0.1f)
			{
				this.slowmoed = false;
			}
		}
	}

	public int nearMissCount
	{
		get
		{
			return this._nearMissCount;
		}
	}

	public float CollisionProtectionTime
	{
		get
		{
			return this.collisionProtectTime;
		}
	}

	public int CarHealth
	{
		get
		{
			return this._carHealth;
		}
		set
		{
			this._carHealth = Mathf.Max(value, 0);
			if (this._carHealth == 0)
			{
				this.FinishGame();
			}
		}
	}

	public ParticleSystem AnimeEffect;

	internal void FinishGame()
	{
		this.GamePlayStarted = false;
		this.player.SendMessage("OnTimeEnded");
		this.canRessurect = ((Singleton<GameCore>.Instance.gameMode == GameMode.Single || Singleton<GameCore>.Instance.gameMode == GameMode.Double) && this.resurrectCount < 5);
		if (Singleton<GameCore>.Instance.gameMode != GameMode.Multi)
		{
			Singleton<UIManager>.Instance.inGamePage.GameOver();
		}
		else
		{
			Singleton<UIManager>.Instance.morePanels.multiModeCanvas.player1.timeReached = this.player.GetComponent<TimeTrackMulti>().Seconds;
			RivalCar rivalCar = UnityEngine.Object.FindObjectOfType<RivalCar>();
			Singleton<UIManager>.Instance.morePanels.multiModeCanvas.player2.timeReached = ((!rivalCar.reached) ? (Singleton<UIManager>.Instance.morePanels.multiModeCanvas.player1.timeReached * 1.1f) : rivalCar.timeSpent);
			Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(rivalCar.gameObject, ObjectsPool.ObjectType.MultModeCar);
			Singleton<UIManager>.Instance.ShowGameOverMulti();
			Singleton<UIManager>.Instance.QuitGame();
		}
	}

	public int CoinsCollected
	{
		get
		{
			return this.coinsForSession;
		}
		set
		{
			this.coinsForSession = value;
			Singleton<UIManager>.Instance.inGamePage.UpdateIngameCoinsText();
		}
	}

	public GamePlay.GameplayStates GameState
	{
		get
		{
			return this.gameState;
		}
	}

	public float GamePlayTime
	{
		get
		{
			return this._gamePlayTime;
		}
		set
		{
			this._gamePlayTime = Mathf.Max(0f, value);
		}
	}

	public int CheckpointCounter
	{
		get
		{
			return this.checkpointCounter;
		}
	}

	public void StartReadyGoSequence()
	{
		base.StartCoroutine(this.OnReadyEnded());
		this.spawnManager.SpawnTrafficRandomly(0f, 32, 12);
	}

	public IEnumerator OnReadyEnded()
	{
		if (!Singleton<SoundManager>.Instance.IsInGameMusicPlaying())
		{
			Singleton<SoundManager>.Instance.PlayInGameMusic();
		}
		yield return new WaitForSeconds(0.1f);
		this.RaceStarted();
		yield break;
	}

	public void CreatePlayerCarByRef(GameObject selectedCar)
	{
		this.carSelectedRef = selectedCar;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(selectedCar);
		gameObject.transform.parent = Singleton<LevelManager>.Instance.Root;
		gameObject.tag = "Untagged";
		this.ChangePlayerCar(gameObject, Vector3.zero);
	}

	public void CreatePlayerCar()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.car1);
		gameObject.transform.parent = Singleton<LevelManager>.Instance.Root;
		gameObject.tag = "Untagged";
		this.ChangePlayerCar(gameObject, Vector3.zero);
	}

	public void OnChangeCarEvent(Vector3 fakeCarPosition)
	{
		bool flag = fakeCarPosition != Vector3.zero || fakeCarPosition == Vector3.zero;
		if (flag)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.carSelectedRef);
			gameObject.transform.parent = Singleton<LevelManager>.Instance.Root;
			gameObject.tag = "Untagged";
			this.ChangePlayerCar(gameObject, fakeCarPosition);
		}
	}

	private void ChangePlayerCar(GameObject newCar, Vector3 fakeCarPosition)
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		bool playerIsDead = this.player.PlayerIsDead;
		bool nitroActive = this.player.NitroActive;
		bool playerIsStopping = this.player.PlayerIsStopping;
		float z = this.player.PlayerRigidbody.velocity.z;
		gameObject.SetActive(false);
		gameObject.tag = "Untagged";
		newCar.tag = "Player";
		newCar.name = "Player";
		newCar.SetActive(true);
		PlayerMovement component = gameObject.GetComponent<PlayerMovement>();
		this.player = newCar.GetComponent<PlayerMovement>();
		Vector3 initRBspeed = Vector3.zero;
		if (fakeCarPosition != Vector3.zero)
		{
			newCar.transform.position = fakeCarPosition;
			initRBspeed = component.PlayerRigidbody.velocity;
		}
		else
		{
			if (!playerIsStopping && !playerIsDead && Singleton<UIManager>.Instance.inGamePage.isActiveAndEnabled)
			{
				initRBspeed = Vector3.forward * 10; // Start Speed
			}
			newCar.transform.position = gameObject.transform.position;
		}
		this.player.Reset(component.CurrentSpeed, initRBspeed, component.StartDistance, component.CurrentLane);
		UnityEngine.Object.Destroy(gameObject);
		GameObject gameObject2 = GameObject.Find(Singleton<GameCore>.Instance.TrackObjectName).gameObject;
		gameObject2.GetComponent<EndlessTrack>().controller = newCar.GetComponent<EndlessPlayer>();
		if (Singleton<UIManager>.Instance.inGamePage.isActiveAndEnabled)
		{
			gameObject2.GetComponent<EndlessTrack>().controller.StartRunning(gameObject2.GetComponent<EndlessTrack>().FirstBranch);
		}
		Singleton<LevelManager>.Instance.BroadcastMessage("OnChangePlayerCar");
		if (this.GamePlayStarted)
		{
			this.player.SendMessage("OnChangePlayerEffect");
			if (nitroActive)
			{
				this.player.StateInNitro = true;
			}
		}
		else
		{
			if (playerIsDead)
			{
				this.player.PlayerIsDead = true;
			}
			if (playerIsStopping)
			{
				this.player.PlayerIsStopping = true;
				this.player.PlayerOldSpeed = z;
			}
			if (this.GamePlayTime <= 0f && fakeCarPosition == Vector3.zero && Singleton<UIManager>.Instance.inGamePage.isActiveAndEnabled)
			{
				this.player.ResumeInStoppingState();
			}
		}
	}

	internal void RefillHealth()
	{
		this.CarHealth = 3;
	}

	public void OnInitGameplayTime()
	{
		this.resurrectCount = 0;
		this.canRessurect = true;
		this.CarHealth = 3;
		this.CoinsCollected = 0;
		if (this.player.Distance == 0f)
		{
			this.checkpointCounter = 0;
			Singleton<UIManager>.Instance.inGamePage.OnRestartRace();
		}
		this.freezeGameplayTimeTimer = 1.5f;
		this.endingTimePlayed = false;
		if (this.player.Distance == 0f)
		{
			this.checkpointCounter++;
		}
	}

	private void OnResetGameplayTime()
	{
		switch (Singleton<GameCore>.Instance.gameMode)
		{
		case GameMode.Single:
		case GameMode.Double:
		case GameMode.FreeRide:
			this.GamePlayTime = 10000f;
			break;
		case GameMode.TimeTrial:
			this.GamePlayTime = 70f;
			break;
		case GameMode.Multi:
			this.GamePlayTime = (float)(Configurations.Instance.bets[MultiModeUI.multGameMode].distance * 1000);
			break;
		}
		this.OnUpdateGameplayTime(true);
	}

	internal void OnUpdateGameplayTime(bool justStarted)
	{
		if (!justStarted)
		{
			this.GamePlayTime *= 2f;
		}
		this.freezeGameplayTimeTimer = 1.5f;
		this.endingTimePlayed = false;
		this.checkpointCounter++;
		this.GamePlayStarted = true;
		this.wasGamePlayStarted = true;
	}

	private void OnGameover()
	{
		this.SlowmoRatio = -1f;
		this.NitroRatio = -1f;
		this.GamePlayStarted = false;
		Singleton<SoundManager>.Instance.StopEndingTimeSound();
	}

	public void NitroEndCloseToGameOver()
	{
		this.NitroRatio = -1f;
		this.SlowmoRatio = -1f;
	}

	public void RessurectingPlayerNow(int extraTime = 10)
	{
		this.GamePlayTime += (float)extraTime;
		this.freezeGameplayTimeTimer = -1f;
		this.endingTimePlayed = false;
		this.GamePlayStarted = true;
		this.player.gameObject.SendMessage("OnStartRunning");
		Singleton<LevelManager>.Instance.BroadcastMessage("PlayerIsResurrected");
		this.player.KeepSpeedTimer = 2f;
	}

	private void OnBonusCollected(BonusActivity.BonusData bonData)
	{
		if (this.player.PlayerIsDead)
		{
			return;
		}
		if (bonData.type == ObjectsPool.ObjectType.BonusCoin)
		{
			this.CoinsCollected += bonData.coinsToGain;
			Singleton<SoundManager>.Instance.PlayBonusCollected(bonData.type);
		}
	}

	public void ActivateNitroBooster()
	{
		Singleton<SoundManager>.Instance.Nitro();
		this.ExtendGameplay();
		this.player.OnNitroActive(true);
	}

	public bool IsInWrongDirection(int currentLane)
	{
		return Singleton<GameCore>.Instance.gameMode == GameMode.Double && currentLane > 2;
	}

	private void ExtendGameplay()
	{
		if (!this.GamePlayStarted)
		{
			this.GamePlayStarted = true;
			this.wasGamePlayStarted = false;
		}
	}

	internal void EndExtendGameplay()
	{
		if (!this.wasGamePlayStarted)
		{
			this.GamePlayStarted = false;
		}
	}

	internal void RaceStarted()
	{
		this.player.RaceStarted();
		Singleton<UIManager>.Instance.inGamePage.pauseButton.interactable = true;
		this.NitroRatio = -1f;
		this.SlowmoRatio = -1f;
		this.checkpointCounter = 0;
		this.freezeGameplayTimeTimer = 1.5f;
		this.OnResetGameplayTime();
		if (Singleton<GameCore>.Instance.gameMode == GameMode.Multi)
		{
			this.spawnManager.ActivateMulCar(Singleton<UIManager>.Instance.morePanels.multiModeCanvas.dummyPreview, Singleton<UIManager>.Instance.morePanels.multiModeCanvas.player2.playerImg.sprite);
		}
		this.spawnManager.RaceStarted();
	}

	public void ChangeState(GamePlay.GameplayStates newState)
	{
		if (newState == this.gameState)
		{
			return;
		}
		if (this.gameState == GamePlay.GameplayStates.Paused)
		{
			this.ResumeGameplay();
		}
		switch (newState)
		{
		case GamePlay.GameplayStates.Start:
			this.SwitchToStart();
			break;
		case GamePlay.GameplayStates.Paused:
			this.SwitchToPaused();
			break;
		case GamePlay.GameplayStates.Racing:
			this.SwitchToRacing();
			break;
		case GamePlay.GameplayStates.GameOver:
			this.SwitchToGameOver();
			break;
		case GamePlay.GameplayStates.ReadyToRace:
			this.SwitchToReadyToRace();
			break;
		case GamePlay.GameplayStates.Offgame:
			this.SwitchToOffgame();
			break;
		}
	}

	private void ResumeGameplay()
	{
		if (Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			Singleton<TimeManager>.Instance.MasterSource.Resume();
		}
	}

	private void SwitchToOffgame()
	{
		Singleton<SoundManager>.Instance.ResetIngameMusic();
		Singleton<SoundManager>.Instance.PlayOffGameMusic();
		Singleton<LevelManager>.Instance.gameObject.BroadcastMessage("OnGameover", SendMessageOptions.RequireReceiver);
		this.EnvironmentSceneGo.BroadcastMessage("OnGameover", SendMessageOptions.RequireReceiver);
		this.player.gameObject.SetActive(false);
		this.gameState = GamePlay.GameplayStates.Offgame;
	}

	internal void ReactivatePlayer()
	{
		this.player.gameObject.SetActive(true);
	}

	private void SwitchToStart()
	{
		Singleton<LevelManager>.Instance.gameObject.BroadcastMessage("OnGameover");
		this.EnvironmentSceneGo.BroadcastMessage("OnGameover");
		this.player.gameObject.SetActive(true);
		Singleton<LevelManager>.Instance.gameObject.BroadcastMessage("OnStartGame");
		this.EnvironmentSceneGo.BroadcastMessage("OnStartGame");
		this.player.ShiftPlayerForward();
		this.player.OnStartPlayerRun();
		this.gameState = GamePlay.GameplayStates.Start;
	}

	public void ResetEnvironment()
	{
		Singleton<LevelManager>.Instance.gameObject.BroadcastMessage("OnGameover");
		this.EnvironmentSceneGo.BroadcastMessage("OnGameover");
		this.player.gameObject.SetActive(false);
	}

	private void SwitchToReadyToRace()
	{
		this.gameState = GamePlay.GameplayStates.ReadyToRace;
	}

	private void SwitchToRacing()
	{
		GamePlay.GameplayStates gameplayStates = this.gameState;
		if (gameplayStates != GamePlay.GameplayStates.ReadyToRace)
		{
		}
		this.gameState = GamePlay.GameplayStates.Racing;
	}

	private void SwitchToPaused()
	{
		this.gameState = GamePlay.GameplayStates.Paused;
	}

	private void SwitchToGameOver()
	{
		GamePlay.GameplayStates gameplayStates = this.gameState;
		if (gameplayStates != GamePlay.GameplayStates.Start)
		{
			if (gameplayStates == GamePlay.GameplayStates.Racing)
			{
				Singleton<LevelManager>.Instance.gameObject.BroadcastMessage("OnGameover");
				this.EnvironmentSceneGo.BroadcastMessage("OnGameover");
			}
		}
		this.gameState = GamePlay.GameplayStates.GameOver;
	}

	private void Awake()
	{
		this.player = UnityEngine.Object.FindObjectOfType<PlayerMovement>();
	}

	private void Start()
	{
		//Shader.WarmupAllShaders();
	}

	public void LoadNewLevel(bool preload)
	{
		int num = this.envLoadedId;
		this.envLoadedId = Singleton<GameCore>.Instance.CurrentWorld;
		bool flag = num == this.envLoadedId;
		if (flag && !preload)
		{
			Singleton<UIManager>.Instance.gameInitializer.LevelLoaded();
		}
		else
		{
			if (this.EnvironmentSceneGo != null)
			{
				Singleton<ObjectsPool>.Instance.DestroyPoolElements();
				GameObject.Find(Singleton<GameCore>.Instance.previousWorldName).GetComponent<EndlessTrack>().DestroyAllFromScene();
				UnityEngine.Object.Destroy(this.EnvironmentSceneGo);
			}
			base.StartCoroutine(this.LoadEnvironment(preload));
		}
	}

	private IEnumerator LoadEnvironment(bool preload)
	{
		if (!preload)
		{
			yield return Resources.UnloadUnusedAssets();
			Singleton<GameCore>.Instance.CreateWorld(this.envLoadedId);
		}
		yield return new WaitForEndOfFrame();
		this.EnvironmentSceneGo = GameObject.Find("THISISWORLD" + this.envLoadedId).gameObject;
		Singleton<ObjectsPool>.Instance.AddNewItems();
		if (preload)
		{
			this.InitializeGameplay();
		}
		else
		{
			Singleton<UIManager>.Instance.gameInitializer.LevelLoaded();
		}
		yield break;
	}

	private void InitializeGameplay()
	{
		this.CreatePlayerCar();
		this.gameState = GamePlay.GameplayStates.Offgame;
		this.NitroRatio = -1f;
		this.SlowmoRatio = -1f;
		this.checkpointCounter = 0;
	}

	private void Update()
	{
		float deltaTime = Singleton<TimeManager>.Instance.MasterSource.DeltaTime;
		if (this.GamePlayStarted && !this.player.PlayerIsDead)
		{
			if (this.freezeGameplayTimeTimer < 0f)
			{
				this.GamePlayTime -= deltaTime;
			}
			if (this.GamePlayTime < 5f && !this.endingTimePlayed)
			{
				this.endingTimePlayed = true;
				Singleton<SoundManager>.Instance.PlayEndingTimeSound();
			}
			if (this.GamePlayTime <= 0f && this.wasGamePlayStarted && !this.player.NitroActive)
			{
				this.FinishGame();
			}
			if (this.freezeGameplayTimeTimer >= 0f)
			{
				this.freezeGameplayTimeTimer -= deltaTime;
			}
		}
		if (this.NitroButtonPressed && this.NitroRatio >= 0f)
		{
			this.NitroRatio -= deltaTime * this.nitroDur;
			this.player.OnNitroUpdate(this.NitroRatio);
		}
		else if (!this.NitroButtonPressed)
		{
			this.NitroRatio += deltaTime * this.nitroRefillDur;
		}
		if (this.slowmoed && this.SlowmoRatio >= 0f)
		{
			this.SlowmoRatio -= deltaTime * this.slowmoDur;
		}
		else if (!this.slowmoed)
		{
			this.SlowmoRatio += deltaTime * this.slowmoRefillDur;
		}
	}

	internal bool NitroButtonPressed
	{
		get
		{
			return this._nitroPressed;
		}
		set
		{
			if (value && value != this._nitroPressed && (double)this.NitroRatio > 0.1)
			{
				this.ActivateNitroBooster();
			}
			if (!value)
			{
                SoundManager.Instance.StopNitro();
            }
			this._nitroPressed = value;
		}
	}

	internal bool slowmoed
	{
		get
		{
			return this._slowmoed;
		}
		set
		{
			if (value && value != this._slowmoed && this.SlowmoRatio > 0f)
			{
				this.MainCamera.OnSlowMoActive(true);
			}
			else if (!value && this._slowmoed != value)
			{
				this.MainCamera.OnSlowMoActive(false);
			}
			this._slowmoed = value;
		}
	}

	internal void BackToDefaultCar()
	{
		Singleton<SoundManager>.Instance.PlayGeneralGameSound(SoundManager.GeneralGameSounds.BackFromBoost);
		this.OnChangeCarEvent(Vector3.zero);
		if (this.GamePlayTime <= 0f)
		{
			this.player.transform.position = new Vector3(this.player.transform.position.x, 0f, this.player.transform.position.z);
		}
		else
		{
			this.player.SendMessage("OnCollisionProtectionActivation", true);
		}
	}

	private bool CheckpointCheck()
	{
		float num = this.spawnManager.NextCheckpointPosition - this.player.gameObject.transform.position.z;
		return num > 120f;
	}

	public void ResetRunParameter()
	{
		this._nearMissCount = 0;
		this.OnResetGameplayTime();
	}

	public void UpdateNearMiss(int value)
	{
		this._nearMissCount += value;
	}

	public GameObject car1;

	public CarData[] playerCars;

	internal float coinsExponent = 1.6f;

	internal float coinsBase = 1f;

	internal float coinsStep = 1f;

	private GameObject EnvironmentSceneGo;

	public SpawnManager spawnManager;

	private GamePlay.GameplayStates gameState;

	internal PlayerMovement player;

	public const int MAX_HEALTH = 2;

	private float collisionProtectTime = 1f;

	private float _nitroRatio = -1f;

	private float _slowmoRatio = -1f;

	private float freezeGameplayTimeTimer = -1f;

	private bool wasGamePlayStarted = true;

	private int checkpointCounter;

	private float prevFixedDeltaTime;

	private bool endingTimePlayed;

	private int coinsForSession;

	private GameObject carSelectedRef;

	private int envLoadedId = -1;

	private int _nearMissCount;

	private float nitroDur = 0.2f;

	private float nitroRefillDur = 0.04f;

	private float slowmoDur = 0.2f;

	private float slowmoRefillDur = 0.04f;

	public FollowPlayer MainCamera;

	private int _carHealth = 3;

	internal bool canRessurect;

	private int resurrectCount;

	internal bool GamePlayStarted;

	private float _gamePlayTime = 100f;

	private bool _nitroPressed;

	private bool _slowmoed;

	public enum GameplayStates
	{
		Start,
		Paused,
		Racing,
		GameOver,
		ReadyToRace,
		Offgame
	}
}
