// dnSpy decompiler from Assembly-CSharp.dll class: BonusActivity
using System;
using UnityEngine;

public class BonusActivity : ListNodeG<BonusActivity>
{
	private SpawnManager spawnManager
	{
		get
		{
			return Singleton<GamePlay>.Instance.spawnManager;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (this.spawnObjType == null)
		{
			this.spawnObjType = base.gameObject.GetComponent<SpawnableObjectType>();
		}
		if (other.gameObject.CompareTag("Player"))
		{
			ObjectsPool.ObjectType type = this.spawnObjType.type;
			Vector3 offset = Vector3.forward * 1.7f + Vector3.up;
			if (type == ObjectsPool.ObjectType.BonusCoin)
			{
				Singleton<ObjectsPool>.Instance.RequestEffect(other.gameObject.transform, offset, ObjectsPool.ObjectType.PickupCoins, true);
			}
			BonusActivity.BonusData bonusData = new BonusActivity.BonusData();
			bonusData.type = type;
			bonusData.coinsToGain = this.coinsGiven;
			Singleton<LevelManager>.Instance.BroadcastMessage("OnBonusCollected", bonusData);
			if (!this.nonPoolObject)
			{
				Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(base.gameObject, type);
			}
		}
	}

	private void Awake()
	{
		this.playerRef = GameObject.FindGameObjectWithTag("Player");
		this.playerRb = this.playerRef.GetComponent<Rigidbody>();
		this.playerKinematics = this.playerRef.GetComponent<PlayerMovement>();
		this.rb = base.GetComponent<Rigidbody>();
		this.spawnObjType = base.gameObject.GetComponent<SpawnableObjectType>();
	}

	private void OnEnable()
	{
		this.spawnManager.__bonuses.Add(this);
	}

	private void OnDisable()
	{
		if (Singleton<GamePlay>.Instance)
		{
			this.spawnManager.__bonuses.Remove(this);
		}
	}

	private void Start()
	{
		this.startPosition = base.transform.localPosition;
		this.startTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
	}

	public void MyFixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (this.playerRef == null)
		{
			this.playerRef = GameObject.FindGameObjectWithTag("Player");
			this.playerRb = this.playerRef.GetComponent<Rigidbody>();
			this.playerKinematics = this.playerRef.GetComponent<PlayerMovement>();
		}
		float num = this.bonusSpeed * 0.2777778f;
		Vector3 velocity = this.playerRb.velocity;
		if (this.activateSlowingDown)
		{
			float num2 = Mathf.Max(0f, velocity.z - 22.2222233f);
			Vector3 velocity2 = this.rb.velocity;
			num = velocity2.z + (num2 - velocity2.z) * fixedDeltaTime * 80f;
		}
		this.rb.velocity = new Vector3(0f, 0f, num);
		if (!this.activateSlowingDown)
		{
			if (this.playerKinematics.PlayerIsDead || this.playerKinematics.PlayerIsStopping)
			{
				this.activateSlowingDown = true;
			}
		}
		else if (velocity.z >= this.bonusSpeed * 0.2777778f && velocity.z >= num)
		{
			this.activateSlowingDown = false;
		}
		if (null == this.spawnObjType)
		{
			this.spawnObjType = base.gameObject.GetComponent<SpawnableObjectType>();
		}
		if (this.verticalSpeed != 0f)
		{
			float num3 = Singleton<TimeManager>.Instance.MasterSource.TotalTime - this.startTime;
			float y = this.startPosition.y + this.verticalDistance * Mathf.Sin(num3 * this.verticalSpeed);
			base.transform.localPosition = new Vector3(this.startPosition.x, y, this.startPosition.z);
		}
	}

	private void OnChangePlayerCar()
	{
		this.playerRef = GameObject.FindGameObjectWithTag("Player");
	}

	public void ResetBonus(int lane)
	{
		base.gameObject.transform.position = new Vector3(0f, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
		base.gameObject.transform.position += new Vector3(9f * GameCore.lanes[lane], 0f, 0f);
		this.activateSlowingDown = false;
	}

	public bool nonPoolObject;

	public float bonusSpeed = 125f;

	public float verticalSpeed;

	private int coinsGiven = 1;

	private GameObject playerRef;

	private Vector3 startPosition;

	private float startTime;

	private float verticalDistance = 0.2f;

	private bool activateSlowingDown;

	private Rigidbody rb;

	private Rigidbody playerRb;

	private PlayerMovement playerKinematics;

	private SpawnableObjectType spawnObjType;

	public class BonusData
	{
		public ObjectsPool.ObjectType type;

		public int coinsToGain;
	}
}
