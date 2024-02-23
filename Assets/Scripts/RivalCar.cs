// dnSpy decompiler from Assembly-CSharp.dll class: RivalCar
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RivalCar : MonoBehaviour
{
	private PlayerMovement playerMov
	{
		get
		{
			return Singleton<GamePlay>.Instance.player;
		}
	}

	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.ignoreLayers |= (1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Blocks") | 1 << LayerMask.NameToLayer("Bonus") | 1 << LayerMask.NameToLayer("Player"));
		this.Initialize(false);
		this.currentMaxSpeed = this.maxSpeed;
		this.startPosition = this.rb.position;
		this.backupSideAcc = this.sideAcc;
	}

	private bool CheckLaneBySide(bool isRightLane, Vector3 startPos, Vector3 endPos)
	{
		Vector3 b = Vector3.left * 3.33f;
		if (isRightLane)
		{
			b = Vector3.right * 3.33f;
		}
		RaycastHit raycastHit;
		Physics.Raycast(startPos + b, endPos + b, out raycastHit, 20f, ~this.ignoreLayers);
		return raycastHit.collider;
	}

	private void Update()
	{
		if (!this.reached)
		{
			this.multiTimer += Time.deltaTime;
			this.timeSpent = this.multiTimer % 60f;
		}
	}

	private void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		this.MyFixedUpdate();
		this.rivalRayCastTimer -= fixedDeltaTime;
		if (!this.isChangingLane)
		{
			this.rivalRayCastTimer = this.rivalRayCastTime;
			float num = 4f;
			if (this.rb.velocity.z / 0.2777778f > 250f)
			{
				num = 15f;
			}
			if (Singleton<GamePlay>.Instance.IsInWrongDirection(this.CurrentLane))
			{
				num = 100f;
			}
			bool flag = false;
			bool flag2 = false;
			Vector3 vector = base.gameObject.transform.position + Vector3.forward * 3.4f + Vector3.up * 1f;
			Vector3 endPos = vector + Vector3.forward * num;
			RaycastHit raycastHit;
			bool flag3 = Physics.Raycast(vector, Vector3.forward, out raycastHit, num, ~this.ignoreLayers);
			if (flag3 && raycastHit.collider.gameObject.GetComponent<RivalCar>() == null)
			{
				if ((UnityEngine.Random.Range(0, 100) > 50 || this.CurrentLane == 0) && this.CurrentLane != 3)
				{
					flag = this.CheckLaneBySide(true, vector, endPos);
					if (flag)
					{
						flag2 = this.CheckLaneBySide(false, vector, endPos);
					}
				}
				else
				{
					flag2 = this.CheckLaneBySide(false, vector, endPos);
					if (flag2)
					{
						flag = this.CheckLaneBySide(true, vector, endPos);
					}
				}
				if (!flag)
				{
					this.isChangingLane = true;
					this.OnChangeLaneRival(1);
				}
				else if (!flag2)
				{
					this.isChangingLane = true;
					this.OnChangeLaneRival(-1);
				}
			}
			if (this.playerMov.PlayerIsStopping)
			{
				this.SetRivalParameters(90f, 280f);
			}
			float num2 = base.transform.position.z - this.playerMov.transform.position.z;
			this.rb.drag = ((num2 <= this.keepDistance) ? 0.1f : 0.3f);
			float maxspeed = Mathf.Clamp(this.playerMov.PlayerRigidbody.velocity.z / 0.2777778f, 0f, this.clampSpeedAt);
			this.SetRivalParameters(this.playerMov.ForwardAcceleration, maxspeed);
		}
		bool flag4 = this.CurrentLane == this.playerMov.CurrLaneTraffic;
		if (flag4)
		{
			this.changeLaneTimer = UnityEngine.Random.Range(this.minChangeLaneTimer, this.maxChangeLaneTimer);
		}
		else
		{
			this.changeLaneTimer -= fixedDeltaTime;
			if (this.changeLaneTimer < 0f)
			{
				this.CurrentLane = this.playerMov.CurrLaneTraffic;
			}
		}
	}

	private void Initialize(bool updateGameplay)
	{
		this.rivalBackupValues = new List<float>();
		this.rivalBackupValues.Add(this.forwardAcc);
		this.rivalBackupValues.Add(this.maxSpeed);
		this.SetRivalParameters(15f, 380f);
	}

	internal void OnChangeLaneEnd()
	{
		this.isChangingLane = false;
	}

	public void SetupCar(GameObject obj, int lane, Sprite sp)
	{
		this.playerAvatarImg.sprite = sp;
		this.reached = false;
		this.multiTimer = 0f;
		this.keepDistance = (float)((UnityEngine.Random.Range(0, 100) <= 50) ? UnityEngine.Random.Range(8, 12) : UnityEngine.Random.Range(-15, -5));
		UnityEngine.Debug.LogFormat("Keep Distance is >>>>>>>>>>>> {0}", new object[]
		{
			this.keepDistance
		});
		this.Initialize(true);
		this.CurrentLane = lane;
		this.chosenCar = obj;
		this.chosenCar.transform.SetParent(base.transform);
		this.chosenCar.transform.localRotation = Quaternion.identity;
		this.chosenCar.transform.localScale = Vector3.one;
		this.chosenCar.transform.localPosition = Vector3.zero;
	}

	public void DeleteStuffOnIt()
	{
		UnityEngine.Object.Destroy(this.chosenCar);
	}

	private SpawnManager spawnManager
	{
		get
		{
			return Singleton<GamePlay>.Instance.spawnManager;
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
			this._currentLane = Mathf.Clamp(value, 0, 4);
		}
	}

	private void Start()
	{
		this.OnReset();
	}

	public void MyFixedUpdate()
	{
		Vector3 position = this.rb.position;
		float fixedDeltaTime = Time.fixedDeltaTime;
		float num = 9f * GameCore.lanes[this.CurrentLane];
		float num2 = num - position.x;
		bool flag = Mathf.Abs(num2) < 0.5f;
		if (flag && this.isChangingLane)
		{
			this.isChangingLane = false;
			this.OnChangeLaneEnd();
		}
		float num3 = base.gameObject.transform.position.z - this.playerRef.position.z;
		if (num3 < this.minPlayerDistance)
		{
			this.minPlayerDistance = num3;
		}
		Vector3 force = new Vector3(0f, 0f, 0f);
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
		Vector3 forward = new Vector3(velocity.x * this.rotCoeff, 0f, (float)this.forwardDirectionOnLane * 1f);
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

	private Transform playerRef
	{
		get
		{
			return Singleton<GamePlay>.Instance.player.transform;
		}
	}

	private void ChangeLaneUpdate(float dt)
	{
		float num = base.gameObject.transform.position.z - this.playerRef.position.z;
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
	}

	public void SetRivalParameters(float _forwardAcc, float _maxspeed)
	{
		this.forwardAcc = _forwardAcc;
		this.maxSpeed = _maxspeed;
		this.currentMaxSpeed = this.maxSpeed;
	}

	private void OnChangeLaneRival(int deltaLane = 0)
	{
		this.isChangingLane = true;
		if (this.CurrentLane == 5)
		{
			this.CurrentLane = 4;
		}
		else if (this.CurrentLane == 0)
		{
			this.CurrentLane = 1;
		}
		else
		{
			
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
		this.rb.constraints = (RigidbodyConstraints)116;
		base.gameObject.BroadcastMessage("ActivateShadow", true, SendMessageOptions.DontRequireReceiver);
	}

	internal void OnResetVehicle()
	{
		this.rivalRayCastTimer = -1f;
		this.isChangingLane = false;
		this.SetRivalParameters(this.rivalBackupValues[0], this.rivalBackupValues[1]);
		this.rivalBackupValues.Clear();
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

	public float clampSpeedAt;

	internal float keepDistance = 10f;

	private List<float> rivalBackupValues;

	private float rivalRayCastTime = 0.3f;

	private float rivalRayCastTimer = -1f;

	private bool isChangingLane;

	private Rigidbody rb;

	private float minChangeLaneTimer = 3f;

	private float maxChangeLaneTimer = 5f;

	private float changeLaneTimer = -1f;

	private float multiTimer;

	internal float timeSpent;

	internal bool reached;

	public Image playerAvatarImg;

	internal GameObject chosenCar;

	private float lastCollisionTime = -1f;

	private float lastLateralCollisionTime = -1f;

	private Vector3 startPosition;

	private float backCollisionTime = -1f;

	private float backupSideAcc = -1f;

	private int forwardDirectionOnLane = 1;

	private LayerMask ignoreLayers = 0;

	private float minPlayerDistance = float.MaxValue;

	private bool canChangeLane = true;

	private int finalTargetLane = -1;

	private bool laneChoosen;

	private float changeLaneArrowTimer = -1f;

	private bool laneAlreadyChanged = true;

	private float changeLaneDistance = -1f;

	private float minChangeLaneDistance = 100f;

	private float maxChangeLaneDistance = 150f;

	private float changeLaneTime = -1f;

	private bool forceChangeLane = true;

	private int _currentLane;

	private float currentMaxSpeed;

	public float forwardAcc = 5f;

	public float maxSpeed = 150f;

	private float grip = 16f;

	public float sideAcc = 180f;

	private float laneDelta = 0.5f;

	private float rotCoeff = 0.01f;
}
