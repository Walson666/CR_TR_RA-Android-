// dnSpy decompiler from Assembly-CSharp.dll class: OpponentMovement
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class OpponentMovement : ListNodeG<OpponentMovement>
{
	private SpawnManager spawnManager
	{
		get
		{
			return Singleton<GamePlay>.Instance.spawnManager;
		}
	}

	public Rigidbody OpponentRigidbody
	{
		get
		{
			return this.rb;
		}
	}

	public int CurrentLane
	{
		get
		{
			return this._currentLane;
		}
		set
		{
			this._currentLane = Mathf.Clamp(value, 0, 4); // LANECHANGE
		}
	}

	public float MinDistanceFromFromPlayer
	{
		get
		{
			return this.minPlayerDistance;
		}
	}

	public float CurrentDistanceFromFromPlayer
	{
		get
		{
			if (this.playerRef == null)
			{
				this.playerRef = GameObject.FindGameObjectWithTag("Player");
			}
			return base.gameObject.transform.position.z - this.playerRef.transform.position.z;
		}
	}

	public int PreviousLane
	{
		get
		{
			return this.prevLane;
		}
		set
		{
			this.prevLane = value;
		}
	}

	public int LaneDirection
	{
		set
		{
			this.forwardDirectionOnLane = value;
		}
	}

	public bool IsBonus
	{
		get
		{
			return this.isBonus;
		}
		set
		{
			this.isBonus = value;
		}
	}

	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.ignoreLayers |= (1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Blocks") | 1 << LayerMask.NameToLayer("Bonus") | 1 << LayerMask.NameToLayer("Player"));
		this.currentMaxSpeed = this.maxSpeed;
		this.startPosition = this.rb.position;
		this.backupSideAcc = this.sideAcc;
	}

	private void OnEnable()
	{
		this.spawnManager.__opponents.Add(this);
	}

	private void OnDisable()
	{
		if (Singleton<GamePlay>.Instance)
		{
			this.spawnManager.__opponents.Remove(this);
		}
	}

	private void Start()
	{
		this.OnReset();
	}

	public void MyFixedUpdate()
	{
		Vector3 position = this.rb.position;
		if (Math.Abs(position.x) > 18f)
		{
			this.CarDestroy(true);
		}
		float fixedDeltaTime = Time.fixedDeltaTime;
		float num = 9f * GameCore.lanes[this.CurrentLane];
		float num2 = num - position.x;
		bool flag = Mathf.Abs(num2) < 0.5f;
		if (flag && this.isChangingLane)
		{
			this.isChangingLane = false;
		}
		if (this.playerRef == null)
		{
			this.playerRef = GameObject.FindGameObjectWithTag("Player");
		}
		float num3 = base.gameObject.transform.position.z - this.playerRef.transform.position.z;
		if (num3 < this.minPlayerDistance)
		{
			this.minPlayerDistance = num3;
		}
		Vector3 force = new Vector3(0f, 0f, 0f);
		bool flag2 = base.gameObject.CompareTag("Bonus");
		Vector3 velocity = this.rb.velocity;
		if (num2 > this.laneDelta)
		{
			force.x = this.sideAcc * fixedDeltaTime;
		}
		else if (num2 < -this.laneDelta)
		{
			force.x = -this.sideAcc * fixedDeltaTime;
		}
		force.x -= velocity.x * this.grip * fixedDeltaTime;
		if (Math.Abs(velocity.z) <= this.currentMaxSpeed * 0.2777778f)
		{
			force.z = (float)this.forwardDirectionOnLane * this.forwardAcc * 10f * fixedDeltaTime;
		}
		this.rb.AddForce(force, ForceMode.VelocityChange);
		Vector3 forward;
		if (flag2)
		{
			forward = new Vector3(0f, 1f, 0f);
		}
		else
		{
			forward = new Vector3(velocity.x * this.rotCoeff, 0f, (float)this.forwardDirectionOnLane * 1f);
		}
		this.rb.MoveRotation(Quaternion.LookRotation(forward));
		if (this.backCollisionTime > 0f)
		{
			this.backCollisionTime -= fixedDeltaTime;
			if (this.backCollisionTime < 0f)
			{
				this.sideAcc = this.backupSideAcc;
			}
		}
		if (this.changeLaneTime > 0f)
		{
			this.changeLaneTime -= fixedDeltaTime;
			if (this.changeLaneTime < 0f)
			{
				this.EndChangeLane();
			}
		}
		if (this.canChangeLane && !this.laneAlreadyChanged)
		{
			this.ChangeLaneUpdate(fixedDeltaTime);
		}
	}

	private void ChangeLaneUpdate(float dt)
	{
		if (this.playerRef == null)
		{
			this.playerRef = GameObject.FindGameObjectWithTag("Player");
		}
		float num = base.gameObject.transform.position.z - this.playerRef.transform.position.z;
		if ((num < this.changeLaneDistance || this.forceChangeLane) && this.changeLaneArrowTimer >= 0f)
		{
			if (!this.laneChoosen)
			{
				if ((UnityEngine.Random.Range(0, 100) > 50 || this.CurrentLane == 0) && this.CurrentLane != 5)
				{
					this.CheckLaneBySide(true, out this.finalTargetLane);
					if (this.finalTargetLane < 0 && this.CurrentLane != 0)
					{
						this.CheckLaneBySide(false, out this.finalTargetLane);
					}
				}
				else
				{
					this.CheckLaneBySide(false, out this.finalTargetLane);
					if (this.finalTargetLane < 0 && this.CurrentLane != 5)
					{
						this.CheckLaneBySide(true, out this.finalTargetLane);
					}
				}
				this.laneChoosen = true;
				if (!this.forceChangeLane)
				{
					Singleton<GamePlay>.Instance.spawnManager.OnPauseChangeLaneUpdate();
				}
				if (this.forceChangeLane)
				{
					this.finalTargetLane = ((UnityEngine.Random.value * 100000f % 100f <= 50f) ? (this.CurrentLane + 1) : (this.CurrentLane - 1));
					if (this.finalTargetLane < 0)
					{
						this.finalTargetLane = 1;
					}
					else if (this.finalTargetLane > 5)
					{
						this.finalTargetLane = 4;
					}
				}
			}
			this.changeLaneArrowTimer -= dt;
			if (this.changeLaneArrowTimer < 0f && this.finalTargetLane >= 0)
			{
				this.forceChangeLane = false;
				this.changeLaneDistance = UnityEngine.Random.Range(this.minChangeLaneDistance, this.maxChangeLaneDistance);
				this.laneAlreadyChanged = true;
				this.changeLaneTime = (float)Math.Abs(this.CurrentLane - this.finalTargetLane);
				this.CurrentLane = this.finalTargetLane;
				this.changeLaneArrowTimer = this.changeLaneTime;
				this.sideAcc = 80f;
			}
		}
	}

	private void CheckLaneBySide(bool rightLane, out int finalTargetLane)
	{
		finalTargetLane = -1;
		int num = UnityEngine.Random.Range(1, 5);
		if (rightLane)
		{
			int num2 = Mathf.Min(this.CurrentLane + num, 5);
			for (int i = this.CurrentLane; i < num2; i++)
			{
				int num3 = i + 1;
				if (!this.spawnManager.LaneIsFree(num3))
				{
					break;
				}
				if (!this.CheckLaneByIndex(num3))
				{
					break;
				}
				finalTargetLane = num3;
			}
		}
		else
		{
			int num4 = Mathf.Max(this.CurrentLane - num, 0);
			for (int j = this.CurrentLane; j > num4; j--)
			{
				int num5 = j - 1;
				if (!this.spawnManager.LaneIsFree(num5))
				{
					break;
				}
				if (!this.CheckLaneByIndex(num5))
				{
					break;
				}
				finalTargetLane = num5;
			}
		}
	}

	private bool CheckLaneByIndex(int index)
	{
		float d = 6f;
		Vector3 vector = base.gameObject.transform.position + Vector3.up * 1f + Vector3.right * (float)(index - this.CurrentLane);
		Vector3 direction = vector + Vector3.forward * d;
		RaycastHit raycastHit;
		Physics.Raycast(vector, direction, out raycastHit, 50f, ~this.ignoreLayers);
		return !raycastHit.collider;
	}

	private void OnPauseChangeLaneUpdate()
	{
		if (!this.laneChoosen)
		{
			this.ResetChangeLane();
			this.canChangeLane = false;
		}
	}

	private void EndChangeLane()
	{
		this.sideAcc = this.backupSideAcc;
	}

	private void ResetChangeLane()
	{
		this.canChangeLane = true;
		this.finalTargetLane = -1;
		this.laneChoosen = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		OpponentMovement component = collision.collider.GetComponent<OpponentMovement>();
		if (component != null)
		{
			this.CarDestroy(false);
			return;
		}
		bool flag = collision.gameObject.CompareTag("Player");
		if (this.lastCollisionTime != -1f && !flag)
		{
			return;
		}
		float totalTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		bool flag2 = this.CurrentLane == 0 || this.CurrentLane == 5;
		if (!flag || totalTime - this.lastLateralCollisionTime > 2f)
		{
		}
		if (totalTime - this.lastLateralCollisionTime < 0.1f && this.backCollisionTime > 0f && flag2)
		{
			return;
		}
		if (this.backCollisionTime < 1.3f)
		{
			this.backCollisionTime = -1f;
			this.lastCollisionTime = totalTime;
			this.sideAcc = this.backupSideAcc;
			this.lastLateralCollisionTime = totalTime;
		}
		if (this.backCollisionTime < 0f)
		{
			if (Singleton<GamePlay>.Instance.IsInWrongDirection(this.CurrentLane) && collision.collider.gameObject.tag.Equals("Player"))
			{
				this.rb.velocity = Vector3.zero;
				this.CarDestroy(false);
			}
			else
			{
				this.backCollisionTime = 1.5f;
				this.sideAcc = 20f;
				float d = (UnityEngine.Random.Range(0, 100) <= 50) ? 2500f : -2500f;
				this.rb.AddForce(Vector3.right * d, ForceMode.Impulse);
			}
		}
	}

	private void CarDestroy(bool really = true)
	{
		if (really)
		{
			Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(base.gameObject, ObjectsPool.ObjectType.TrafficVehicle);
		}
	}

	private void OnChangeLane(bool collDx)
	{
		if (collDx)
		{
			this.CurrentLane++;
		}
		else
		{
			this.CurrentLane--;
		}
	}

	private void OnReset()
	{
		base.transform.localScale = Vector3.one * Singleton<GameCore>.Instance.CurrScaleValue;
		this.forwardDirectionOnLane = 1;
		if (Singleton<GamePlay>.Instance.IsInWrongDirection(this.CurrentLane))
		{
			this.forwardDirectionOnLane = -1;
			this.currentMaxSpeed = this.maxSpeedBackwards;
		}
		else
		{
			this.currentMaxSpeed = this.maxSpeed;
		}
		this.prevLane = this.CurrentLane;
		this.rb.position = new Vector3(9f * GameCore.lanes[this.CurrentLane], this.rb.position.y, this.rb.position.z + this.currentMaxSpeed * 0.2777778f * Time.fixedDeltaTime);
		this.rb.velocity = new Vector3(0f, 0f, (float)this.forwardDirectionOnLane * this.currentMaxSpeed * 0.2777778f);
		this.lastCollisionTime = -1f;
		this.backCollisionTime = -1f;
		base.GetComponent<Collider>().enabled = true;
		this.rb.useGravity = false;
		this.minPlayerDistance = float.MaxValue;
		this.ResetChangeLane();
		this.laneAlreadyChanged = true;
		this.canChangeLane = false;
		if (UnityEngine.Random.Range(0, 100) > 80 && Singleton<GameCore>.Instance.gameMode == GameMode.Single)
		{
			this.changeLaneDistance = UnityEngine.Random.Range(this.minChangeLaneDistance, this.maxChangeLaneDistance);
			this.changeLaneArrowTimer = this.arrowChangeLaneTime;
			this.laneAlreadyChanged = false;
			this.canChangeLane = true;
		}
		this.rb.constraints = (RigidbodyConstraints)116;
		base.gameObject.BroadcastMessage("ActivateShadow", true, SendMessageOptions.DontRequireReceiver);
	}

	internal void OnResetVehicle()
	{
		this.OnReset();
		this.OnStartGame();
	}

	private void OnStartGame()
	{
		this.lastCollisionTime = -1f;
		this.backCollisionTime = -1f;
		this.rb.position = this.startPosition;
		this.rb.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, 1f));
		this.rb.velocity = new Vector3(0f, 0f, 0f);
		this.rb.constraints = (RigidbodyConstraints)116;
	}

	internal void ChangeLaneForRoadWorks()
	{
		this.forceChangeLane = true;
		this.canChangeLane = true;
		this.laneAlreadyChanged = false;
		this.changeLaneArrowTimer = UnityEngine.Random.value * 100000f % this.arrowChangeLaneTime;
		this.laneChoosen = false;
	}

	private float lastCollisionTime = -1f;

	private float lastLateralCollisionTime = -1f;

	private int prevLane;

	private Vector3 startPosition;

	private bool isChangingLane;

	private float backCollisionTime = -1f;

	private float backupSideAcc = -1f;

	private GameObject playerRef;

	private bool isBonus;

	private int forwardDirectionOnLane = 1;

	private LayerMask ignoreLayers = 0;

	private float minPlayerDistance = float.MaxValue;

	private bool canChangeLane = true;

	private int finalTargetLane = -1;

	private bool laneChoosen;

	private float changeLaneArrowTimer = -1f;

	private bool laneAlreadyChanged = true;

	private float changeLaneDistance = -1f;

	private float arrowChangeLaneTime = 1f;

	private float minChangeLaneDistance = 100f;

	private float maxChangeLaneDistance = 150f;

	private float changeLaneTime = -1f;

	private bool forceChangeLane = true;

	private int _currentLane;

	private float currentMaxSpeed;

	private Rigidbody rb;

	public float forwardAcc = 5f;

	public float maxSpeed = 150f;

	private float maxSpeedBackwards = 15f;

	private float grip = 16f;

	public float sideAcc = 180f;

	private float laneDelta = 0.5f;

	private float rotCoeff = 0.01f;
}
