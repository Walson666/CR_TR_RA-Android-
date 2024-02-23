// dnSpy decompiler from Assembly-CSharp.dll class: PlayerMovement
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class PlayerMovement : MonoBehaviour
{
	public GameObject driver;
	private SpawnManager spawnManager
	{
		get
		{
			return Singleton<GamePlay>.Instance.spawnManager;
		}
	}

	public bool IsOnAir { get; internal set; }

	public bool IsOnCollisionEffect
	{
		get
		{
			return this.timeLostForHit >= 0f;
		}
	}

	public float GetDataFromIndex(PlayerDataType type, int index)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 10f;
		switch (type)
		{
		case PlayerDataType.MaxAcceleration:
			num = this.forwardAccMin;
			num2 = this.forwardAccMax - this.forwardAccMin;
			break;
		case PlayerDataType.MaxSpeed:
			num = this.maxSpeedMin;
			num2 = this.maxSpeedMax - this.maxSpeedMin;
			break;
		case PlayerDataType.MaxResistance:
			num = this.resistanceMin;
			num2 = this.resistanceMax - this.resistanceMin;
			break;
		case PlayerDataType.MaxNitroSpeed:
			num = this.nitroSpeedMin;
			num2 = this.nitroSpeedMax - this.nitroSpeedMin;
			break;
		}
		return num + num2 / num3 * (float)index;
	}

	public float KeepSpeedTimer
	{
		set
		{
			this.keepSpeedTimer = value;
		}
	}

	public bool WrongDirection
	{
		get
		{
			return this._wrongDir;
		}
		set
		{
			if (this._wrongDir != value)
			{
				Singleton<UIManager>.Instance.inGamePage.IsInWrongLane(value);
			}
			this._wrongDir = value;
		}
	}

	public int CurrentLane
	{
		get
		{
			return this.currentLane;
		}
		set
		{
			this.currentLane = value;
		}
	}

	public int CurrLaneTraffic
	{
		get
		{
			return this.CurrentLane / 3;
		}
	}

	public float ForwardAcceleration
	{
		get
		{
			return this.forwardAcc;
		}
	}

	public float Distance
	{
		get
		{
			return this.distance;
		}
	}

	public Vector3 StartDistance
	{
		get
		{
			return this.startDistance;
		}
	}

	public bool PlayerIsDead
	{
		get
		{
			return this.isDead;
		}
		set
		{
			this.isDead = value;
		}
	}

	public bool PlayerIsStopping
	{
		get
		{
			return this.isStopping || Singleton<GamePlay>.Instance.CarHealth <= 0;
		}
		set
		{
			this.isStopping = value;
		}
	}

	public float PlayerOldSpeed
	{
		set
		{
			this.playerOldSpeed = value;
		}
	}

	public bool StateInNitro
	{
		set
		{
			this._stateInNitro = value;
		}
	}

	public float Speed
	{
		get
		{
			return (!(this.rb != null)) ? 0f : this.rb.velocity.z;
		}
	}

	public bool NitroActive
	{
		get
		{
			return this.nitroActive;
		}
	}

	public bool BackwashOn
	{
		get
		{
			return this.backwashOn;
		}
	}

	public float CurrentSpeed
	{
		get
		{
			return this.maxSpeed;
		}
	}

	public bool IsOnLane
	{
		get
		{
			return this.isOnLane;
		}
	}

	public int PrevLane
	{
		get
		{
			return this.prevLane;
		}
	}

	public Rigidbody PlayerRigidbody
	{
		get
		{
			if (this.rb == null)
			{
				this.rb = base.GetComponent<Rigidbody>();
			}
			return this.rb;
		}
	}

	private void Awake()
	{
		this.pEffect = base.GetComponent<PlayerEffects>();
		base.transform.localScale = Vector3.one * Singleton<GameCore>.Instance.CurrScaleValue;
		this.rb = base.GetComponent<Rigidbody>();
		this.carCust = base.GetComponentInChildren<CarCustomizations>();
	}

	public CarID carId
	{
		get
		{
			return this.customizationData.carId;
		}
	}

	private void Start()
	{
		PlayerDataPersistant.Instance.InitPlayerData(this.carId);
		this.forwardAcc = this.GetDataFromIndex(PlayerDataType.MaxAcceleration, PlayerDataPersistant.Instance.GetPlayerData(this.carId).acceleration);
		this.initialMaxSpeed = this.GetDataFromIndex(PlayerDataType.MaxSpeed, PlayerDataPersistant.Instance.GetPlayerData(this.carId).speed);
		this.nitroSpeed = this.GetDataFromIndex(PlayerDataType.MaxNitroSpeed, PlayerDataPersistant.Instance.GetPlayerData(this.carId).nitroSpeed);
		this.resistanceMax = this.GetDataFromIndex(PlayerDataType.MaxResistance, PlayerDataPersistant.Instance.GetPlayerData(this.carId).maxResistance);
		this.carCust.ApplyCustomizations(this.carId);
		this.maxSpeed = this.initialMaxSpeed;
		this.distance = 0f;
		this.endlessPlayer = base.GetComponent<EndlessPlayer>();
		this.prevLane = this.CurrentLane;
		if (this.PlayerIsStopping)
		{
			this.forwardAcc = -Mathf.Abs(this.forwardAcc);
			this.rb.velocity = Vector3.zero + Vector3.forward * this.playerOldSpeed;
			this.playerOldSpeed = 0f;
		}
		if (this._stateInNitro)
		{
			Singleton<GamePlay>.Instance.SendMessage("ActivateNitro");
		}
		this._stateInNitro = false;
	}

	private void OnStartRunning()
	{
		this.forwardAcc = Mathf.Abs(this.forwardAcc);
		this.pEffect.ActivateNitroFx(false);
	}

	public void Reset(float initSpeed, Vector3 initRBspeed, Vector3 newStartDistance, int currLane)
	{
		this.endlessPlayer = base.GetComponent<EndlessPlayer>();
		this.prevLane = this.CurrentLane;
		this.pEffect.ActivateNitroFx(false);
		this.RaceStarted();
		this.startDistance = newStartDistance;
		this.maxSpeed = initSpeed;
		this.rb.velocity = initRBspeed;
		this.prevVel = this.rb.velocity;
		this.CurrentLane = currLane;
	}

	private void Update()
	{
		//Debug.Log("PLAYER_LANE:" + CurrentLane);
		if (this.firstFrame)
		{
			this.firstFrame = false;
			return;
		}
		if (Singleton<GamePlay>.Instance.GamePlayStarted || this.PlayerIsStopping)
		{
			this.distance = this.rb.position.z - this.startDistance.z;
		}
		this.WrongDirection = Singleton<GamePlay>.Instance.IsInWrongDirection(this.CurrLaneTraffic);
		if (this.collisionProtectionTimer >= 0f)
		{
			this.collisionProtectionTimer -= Singleton<TimeManager>.Instance.MasterSource.DeltaTime;
			if (this.collisionProtectionTimer < 0f)
			{
				this.OnCollisionProtectionActivation(false);
			}
		}
        this.ControlThisCar();
    }

	private void FixedUpdate()
	{
		if (this.isDead)
		{
			if (Mathf.Abs(this.rb.transform.position.x) > 8f)
			{
				float x = Mathf.Clamp(this.rb.transform.position.x, -8f, 8f);
				this.rb.transform.position = new Vector3(x, this.rb.transform.position.y, this.rb.transform.position.z);
			}
			return;
		}
		//this.ControlThisCar();
		if ((this.PlayerIsStopping && this.rb.velocity.z <= 5.555556f) || Singleton<GamePlay>.Instance.GameState != GamePlay.GameplayStates.Racing)
		{
			this.inputLeft = false; this.inputDown = (this.inputUp = (this.inputRight = (this.inputLeft )));
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float totalTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		float num = ((!this.inputRight) ? 0f : 1f) + ((!this.inputLeft) ? 0f : -1f);
		if (this.goOneLaneDx || this.goOneLaneSx)
		{
			num = ((!this.goOneLaneDx) ? 0f : 1f) + ((!this.goOneLaneSx) ? 0f : -1f);
			this.goOneLaneSx = false; this.goOneLaneDx = (this.goOneLaneSx );
		}
		if (this.endlessPlayer.lastToken == null)
		{
			return;
		}
		float num2 = 9f * GameCore.playerLanes[this.CurrentLane];
		float num3 = num2 - this.rb.position.x;
		Vector3 force = new Vector3(0f, 0f, 0f);
		this.isOnLane = (Mathf.Abs(num3) < 1f);
		this.pEffect.ShowDrift(this.inputLeft, !this.isOnLane);
		this.pEffect.ShowDrift(this.inputRight, !this.isOnLane);
		int num4 = this.CurrentLane;
		if (num > 0f && this.CurrentLane < GameCore.playerLanes.Length - 1)
		{
			num4++;
		}
		else if (num < 0f && this.CurrentLane > 0)
		{
			num4--;
		}
		if (this.isOnLane)
		{
			this.prevLane = this.CurrentLane;
		}
		if (this.CurrentLane == this.prevLane && this.isOnLane)
		{
			this.CurrentLane = num4;
		}
		else if (num4 == this.prevLane)
		{
			this.CurrentLane = num4;
		}
		float num5 = this.minSideAccRatio + Mathf.Clamp01((this.rb.position.z - this.startSideAccLane) / (this.endSideAccLane - this.startSideAccLane)) * (this.maxSideAccRatio - this.minSideAccRatio);
		if (num3 > this.laneDelta)
		{
			force.x = this.sideAcc * num5 * fixedDeltaTime; // Player turn
		}
		else if (num3 < -this.laneDelta)
		{
			force.x = -this.sideAcc * num5 * fixedDeltaTime; // Player turn
        }
		force.x -= this.rb.velocity.x * this.grip * fixedDeltaTime;
		float num6 = this.maxSpeed * 0.2777778f;
		if ((this.nitroActive && this.nitroRatio >= 0.25f) || (this.backwashOn && this.nitroRatio >= 0.25f))
		{
			num6 = this.nitroSpeed * 1.35f * 0.2777778f;
		}
		if (this.autoAcc)
		{
			if (this.rb.velocity.z <= num6)
			{
				if (this.rb.velocity.z <= num6)
				{
					force.z = this.forwardAcc * fixedDeltaTime;
				}
			}
			else if (this.rb.velocity.z >= num6 + 10f)
			{
				force.z = this.backwardAcc * fixedDeltaTime;
			}
		}
		else if (this.inputUp)
		{
			if (this.rb.velocity.z <= num6)
			{
				force.z = this.forwardAcc * fixedDeltaTime;
			}
		}
		else
		{
			force.z = Mathf.Max(0f, force.z * 0.7f);
		}
		if (this.inputDown)
		{
			//Debug.Log($"{this.rb.velocity.z * 3} > {minSpeed}");
            if ((this.rb.velocity.z * 3) >= minSpeed)
			{
                force.z = -(customizationData.resistance * 10) * fixedDeltaTime;
                //Debug.Log(rb.velocity.z);
            }
			else
			{
				force.z = 0;
			}
             // BACK SPEED
		}
		if (this.keepSpeedTimer >= 0f)
		{
			this.keepSpeedTimer -= fixedDeltaTime;
			if (this.rb.velocity.z <= this.maxSpeed * 0.2777778f)
			{
				force.z = this.forwardAccNitro * fixedDeltaTime;
			}
			if (this.keepSpeedTimer < 0f)
			{
				this.keepSpeedTimer = -1f;
			}
		}
		if (this.timeLostForHit >= 0f)
		{
			this.timeLostForHit -= fixedDeltaTime;
			force.z *= 0.1f;
		}
		this.rb.AddForce(force, ForceMode.VelocityChange);
		this.maxSpeed = Mathf.Min(this.maxSpeed + this.maxSpeedInc * fixedDeltaTime, this.limitMaxSpeed);
		Quaternion rot = Quaternion.Euler(Mathf.Abs(this.rb.velocity.x * PlayerMovement.rotCoeff.x), this.rb.velocity.x * PlayerMovement.rotCoeff.y, this.rb.velocity.x * PlayerMovement.rotCoeff.z);
		this.rb.MoveRotation(rot);
		if (this.PlayerIsStopping && this.rb.velocity.z < 100f && !this.isDead)
		{
			this.OnTimeEndedForReal();
		}
		if (Mathf.Abs(this.rb.transform.position.x) > 8f)
		{
			float x2 = Mathf.Clamp(this.rb.transform.position.x, -8f, 8f);
			this.rb.transform.position = new Vector3(x2, this.rb.transform.position.y, this.rb.transform.position.z);
		}
		this.prevVel = this.rb.velocity;
	}

	private void ControlThisCar()
	{
		this.ControlCarEditor();
		int inputMode = Singleton<UIManager>.Instance.InputMode;
		if(inputMode == 2)
		{
            this.autoAcc = true;
        }
        else
        {
            this.autoAcc = false;
        }
		if(inputMode == 1)
		{
			this.autoAcc = false;
			float inputHorizontal = SimpleInput.GetAxis("Horizontal");
			//Debug.Log("Horizontal:" +  inputHorizontal);
			if(inputHorizontal > 0.3f)
			{
                this.inputLeft = false;
                this.inputRight = true;
            }
			else if(inputHorizontal < -0.3f)
			{
                this.inputLeft = true;
                this.inputRight = false;
            }
			else
			{
                this.inputRight = false; this.inputLeft = (this.inputRight);
            }
		}
        /*if (inputMode != 0)
		{
			if (inputMode != 1)
			{
				if (inputMode == 2)
				{
					this.autoAcc = true;
				}
			}
			else
			{
				this.autoAcc = false;
			}
		}*/
		/*else
		{
			this.autoAcc = false;
			float x = Input.acceleration.x;
			if (x < -0.1f)
			{
				this.inputLeft = true;
				this.inputRight = false;
			}
			else if (x > 0.1f)
			{
				this.inputLeft = false;
				this.inputRight = true;
			}
			else
			{
				this.inputRight = false; this.inputLeft = (this.inputRight );
			}
		}*/
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (this.lastCollisionTime != -1f)
		{
			return;
		}
		float totalTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		if (totalTime - this.avoidFrequentCollisionTime < 1f)
		{
			return;
		}
		this.pEffect.PalyerHasCollided();
		if (collision.collider.CompareTag("Traffic"))
		{
			if (Singleton<GameCore>.Instance.gameMode != GameMode.FreeRide && Singleton<GameCore>.Instance.gameMode != GameMode.Multi)
			{
				Singleton<GamePlay>.Instance.CarHealth--;
			}
			Singleton<GamePlay>.Instance.player.Blink();
		}
		this.ActivateFrontCollisionEffect(collision.contacts[0].point);
		base.gameObject.SendMessage("PlayPlayerSound", PlayerSounds.PlayerSoundsType.Hit, SendMessageOptions.DontRequireReceiver);
		this.avoidFrequentCollisionTime = totalTime;
		this.rb.velocity = this.prevVel * 0.5f;
		this.timeLostForHit = this.GetDataFromIndex(PlayerDataType.MaxResistance, PlayerDataPersistant.Instance.GetPlayerData(this.carId).resistance);
		bool flag = this.CurrentLane == 0 || (this.CurrentLane != GameCore.playerLanes.Length - 1 && UnityEngine.Random.Range(1f, 100f) > 50f);
		float d = (!flag) ? 2500f : -2500f;
		this.rb.AddForce(Vector3.right * d, ForceMode.Impulse);
		this.goOneLaneDx = flag;
		this.goOneLaneSx = !this.goOneLaneDx;
	}

	private void Blink()
	{
		this.pEffect.BlinkCar();
	}

	public void ResumeInStoppingState()
	{
		this.PlayerIsStopping = true;
	}

	internal void OnCheckpointEnter()
	{
		if (this.PlayerIsStopping)
		{
			this.PlayerIsStopping = false;
			this.forwardAcc = Mathf.Abs(this.forwardAcc);
		}
	}

	private void OnTimeEnded()
	{
		if (this.nitroActive)
		{
			this.OnNitroActive(false);
		}
		if (this.backwashOn)
		{
			this.OnBackwashActive(false);
		}
		this.PlayerIsStopping = true;
		Singleton<GamePlay>.Instance.NitroEndCloseToGameOver();
	}

	private void OnTimeEndedForReal()
	{
		this.forwardAcc = Math.Abs(this.forwardAcc);
		this.PlayerIsStopping = false;
		this.rb.velocity = new Vector3(0f, 0f, 0f);
		this.rb.constraints = RigidbodyConstraints.FreezeAll;
		Singleton<LevelManager>.Instance.BroadcastMessage("PlayerIsDead");
		base.gameObject.SendMessage("OnDead");
		PlayerDataPersistant.Instance.BestMeters = (int)this.Distance;
		this.isDead = true;
		Singleton<GamePlay>.Instance.BackToDefaultCar();
	}

	private void PlayerIsResurrected()
	{
		this.PlayerIsStopping = false;
		this.isDead = false;
		this.rb.constraints = (RigidbodyConstraints)116;
	}

	internal void OnNitroActive(bool active)
	{
		this.RemovePreviousEffects();
		this.nitroActive = active;
		if (this.nitroActive)
		{
			this.nitroRatio = 1f;
		}
		else
		{
			this.nitroRatio = 0f;
		}
		this.pEffect.ActivateNitroFx(active);
		if (!active)
		{
			this.OnCollisionProtectionActivation(true);
			Singleton<GamePlay>.Instance.EndExtendGameplay();
		}
	}

	private void OnCollisionProtectionActivation(bool active)
	{
		if (active)
		{
			this.collisionProtectionTimer = Singleton<GamePlay>.Instance.CollisionProtectionTime;
		}
		else if (!this.nitroActive)
		{
			this.collisionProtectionTimer = -1f;
		}
	}

	private void OnBackwashActive(bool active)
	{
		this.backwashOn = active;
		if (this.backwashOn)
		{
			this.nitroRatio = 1f;
		}
		else
		{
			this.nitroRatio = 0f;
		}
	}

	internal void OnNitroUpdate(float ratio)
	{
		this.nitroRatio = ratio;
	}

	private void RemovePreviousEffects()
	{
		if (this.backwashOn)
		{
			this.OnBackwashActive(false);
		}
	}

	private void OnGameover()
	{
		this.pEffect.ActivateNitroFx(false);
		this.inputDown = false; this.inputLeft = (this.inputRight = (this.inputUp = (this.inputDown )));
	}

	internal void RaceStarted()
	{
		this.startDistance = this.rb.position;
		this.keepSpeedTimer = -1f;
	}

	private void ActivateCollisionSparks(Vector3 collisionPoint)
	{
		Singleton<ObjectsPool>.Instance.RequestEffectInPoint(collisionPoint, Vector3.zero, ObjectsPool.ObjectType.Sparks);
	}

	private void ActivateFrontCollisionEffect(Vector3 collisionPoint)
	{
		Singleton<ObjectsPool>.Instance.RequestEffect(base.gameObject.transform, base.gameObject.transform.forward * 1.5f + Vector3.up * 1f, ObjectsPool.ObjectType.SideCollision, true);
	}

	private void ActivateSideCollisionEffect(Vector3 collisionPoint, bool rightSide)
	{
		Vector3 a = (!rightSide) ? (Vector3.right * 1f) : (Vector3.right * -1f);
		Singleton<ObjectsPool>.Instance.RequestEffect(base.gameObject.transform, a + Vector3.up * 1f, ObjectsPool.ObjectType.SideCollision, true);
	}

	internal void OnLeftInput(bool down)
	{
		this.inputLeft = down;
	}

	internal void OnRightInput(bool down)
	{
		this.inputRight = down;
	}

	internal void OnUpInput(bool down)
	{
		this.inputUp = down;
	}

	internal void OnDownInput(bool down)
	{
		this.inputDown = down;
	}

	private void OnReset()
	{
		this.PlayerIsStopping = false;
		this.isDead = false;
		this.isOnLane = true;
		this.distance = 0f;
	}

	private void OnDead()
	{
		this.isDead = true;
	}

	private void OnStartGame()
	{
		this.PlayerIsStopping = false;
		this.isDead = false;
		this.lastCollisionTime = -1f;
		this.avoidFrequentCollisionTime = -1f;
		this.backwashOn = false; this.inputDown = (this.inputUp = (this.inputLeft = (this.inputRight = (this.nitroActive = (this.backwashOn )))));
		this.nitroRatio = 0f;
		base.gameObject.transform.position = Vector3.zero;
		this.rb.position = new Vector3(0f, 0f, 0f);
		this.rb.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
		this.rb.velocity = new Vector3(0f, 0f, 60f);
		this.CurrentLane = GameCore.playerLanes.Length / 2;
		this.prevLane = this.CurrentLane;
		this.maxSpeed = this.initialMaxSpeed;
		this.pEffect.ActivateNitroFx(false);
		this.rb.constraints = RigidbodyConstraints.FreezeAll;
	}

	public void OnStartPlayerRun()
	{
		this.rb.constraints = (RigidbodyConstraints)116;
	}

	public void ShiftPlayerForward()
	{
		this.distance = 0f;
		base.gameObject.transform.position = Vector3.forward * 20f;
	}

	private void ControlCarEditor()
	{
	}
	public CamMode camMode;
    public PlayerCustomizeData customizationData;

	public const float HIGH_SPEED = 63f;

	private bool firstFrame = true;

	private EndlessPlayer endlessPlayer;

	private bool inputLeft;
	public bool InputLeft => inputLeft;

	private bool inputRight;
	public bool InputRight => inputRight;

	private bool isStopping;

	private bool isDead;

	private bool inputUp;

	private bool inputDown;

	private bool autoAcc;

	private float lastCollisionTime = -1f;

	private float avoidFrequentCollisionTime = -1f;

	private int prevLane;

	private float distance;

	private Vector3 startDistance = new Vector3(0f, 0f, 0f);

	private float maxSpeed;

	private bool nitroActive;

	private bool backwashOn;

	private float nitroRatio;

	private bool goOneLaneDx;

	private bool goOneLaneSx;

	private bool isOnLane = true;

	private float nitroSpeed = 0.5f;

	private float playerOldSpeed;

	private Vector3 prevVel;

	private int currentLane;

	private float timeLostForHit = -1f;

	private bool _stateInNitro;

	private float keepSpeedTimer = -1f;

	private float collisionProtectionTimer = -1f;

	private Rigidbody rb;

	internal int forceOnLane = -1;

	private float minSideAccRatio = 1f;

	private float maxSideAccRatio = 1f;

	private float laneDelta = 0.5f;

	private float startSideAccLane = 200f;

	private float endSideAccLane = 300f;

	private float sideAcc = 120f;

	private float forwardAcc = 15f;

	private float forwardAccNitro = 75f;

	private float nitroSpeedMin = 120f;

	private float nitroSpeedMax = 400f;

	private float forwardAccMin = 15f;

	private float forwardAccMax = 25f;

	private float maxSpeedMax = 300f;

	private float resistanceMin = 0.5f;

	private float resistanceMax = 0.75f;

	private float grip = 16f;

	private float maxSpeedInc;

	private float limitMaxSpeed = 350f;

	private float initialMaxSpeed = 150f;

	private float backwardAcc = -75f;

	internal float maxSpeedMin = 180f;

	private float minSpeed = 20;

	public static Vector3 rotCoeff = new Vector3(0.125f, 0.2f, -0.2f);

	private bool _wrongDir;

	internal CarCustomizations carCust;

	private PlayerEffects pEffect;

	private const float MAX_XVAL = 8f;

	private const float speedReductionHit = 0.5f;

	private string HORI = "Horizontal";

	private string VERT = "Vertical";
}
