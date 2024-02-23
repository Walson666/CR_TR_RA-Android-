// dnSpy decompiler from Assembly-CSharp.dll class: SpawnManager
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	public List<GameObject> ActiveOpponents
	{
		get
		{
			return this.opponents;
		}
	}

	public float NextCheckpointPosition
	{
		get
		{
			return this.nextCheckpointPosition;
		}
	}

	private void Awake()
	{
		this.UpdateTrafficDataByPlayerLevel();
		this.opponents = new List<GameObject>();
		this.wantToSpawn = new List<SpawnableObjectClass>();
		this.freeLanes = new float[]
		{
			-1f,
			-1f,
			-1f,
			-1f,
			-1f
		};
	}

	public int CurrDist_KM
	{
		get
		{
			return Configurations.Instance.bets[MultiModeUI.multGameMode].distance;
		}
	}

	private void UpdateTrafficDataByPlayerLevel()
	{
		for (int i = 0; i < this.objectsToSpawn.Count; i++)
		{
			if (this.objectsToSpawn[i].type == ObjectsPool.ObjectType.Checkpoint)
			{
				if (Singleton<GameCore>.Instance.gameMode == GameMode.Multi)
				{
					UnityEngine.Debug.LogWarningFormat("Current Distance in KM {0}", new object[]
					{
						this.CurrDist_KM
					});
					this.objectsToSpawn[i].startMinSpawnDistance = (float)this.CurrDist_KM;
					this.objectsToSpawn[i].startMaxSpawnDistance = (float)this.CurrDist_KM;
					this.objectsToSpawn[i].endMinSpawnDistance = (float)this.CurrDist_KM;
					this.objectsToSpawn[i].endMaxSpawnDistance = (float)this.CurrDist_KM;
				}
				else
				{
					this.objectsToSpawn[i].startMinSpawnDistance = (float)this.checkpointDistance;
					this.objectsToSpawn[i].startMaxSpawnDistance = (float)this.checkpointDistance;
					this.objectsToSpawn[i].endMinSpawnDistance = (float)this.checkpointDistance;
					this.objectsToSpawn[i].endMaxSpawnDistance = (float)this.checkpointDistance;
				}
				this.objectsToSpawn[i].NextSpawnObjectDistance = -1f;
			}
			else if (this.objectsToSpawn[i].type == ObjectsPool.ObjectType.TrafficVehicle)
			{
                //Debug.LogError("UpdateTrafficByPlayerLevel");
                this.vehicleTrafficIndex = i;
				this.backupTrafficValues = new float[4];
				this.backupTrafficValues[0] = this.objectsToSpawn[this.vehicleTrafficIndex].startMinSpawnDistance;
				this.backupTrafficValues[1] = this.objectsToSpawn[this.vehicleTrafficIndex].startMaxSpawnDistance;
				this.backupTrafficValues[2] = this.objectsToSpawn[this.vehicleTrafficIndex].endMinSpawnDistance;
				this.backupTrafficValues[3] = this.objectsToSpawn[this.vehicleTrafficIndex].endMaxSpawnDistance;
				this.backupTrafficValues[3] = this.objectsToSpawn[this.vehicleTrafficIndex].endMaxSpawnDistance;
			}
		}
	}

	internal void RaceStarted()
	{
		this.UpdateTrafficDataByPlayerLevel();
		this.lastPositionSpawn = 80f;
		this.lastOpponentSpawn = null;
		for (int i = 0; i < this.objectsToSpawn.Count; i++)
		{
			if (this.objectsToSpawn[i].type == ObjectsPool.ObjectType.TrafficVehicle)
			{
				this.objectsToSpawn[i].startMinSpawnDistance = this.backupTrafficValues[0];
				this.objectsToSpawn[i].startMaxSpawnDistance = this.backupTrafficValues[1];
				this.objectsToSpawn[i].endMinSpawnDistance = this.backupTrafficValues[2];
				this.objectsToSpawn[i].endMaxSpawnDistance = this.backupTrafficValues[3];
				this.objectsToSpawn[i].NextSpawnObjectDistance = -1f;
			}
			else if (this.objectsToSpawn[i].type == ObjectsPool.ObjectType.BonusCoin)
			{
				this.objectsToSpawn[i].NextSpawnObjectDistance = -1f;
			}
		}
	}

	public void SpawnTrafficRandomly(float startDistance, int quantity, int distanceBetween)
	{
		//Debug.LogError("SPAWNTRAFFICRANDOMLY");
		Vector3 a = Vector3.forward * startDistance;
		for (int i = 0; i < quantity; i++)
		{
			this.SpawnObject(new SpawnableObjectClass(ObjectsPool.ObjectType.TrafficVehicle, 0, 0f, 0f, 0f, 0f), 1, 0f, -1, a + Vector3.forward * (float)distanceBetween * (float)i);
		}
	}

	private void FixedUpdate()
	{
		OpponentMovement opponentMovement = this.__opponents.Head;
		while (opponentMovement != null)
		{
			opponentMovement.MyFixedUpdate();
			opponentMovement = opponentMovement.next;
		}
		BonusActivity bonusActivity = this.__bonuses.Head;
		while (bonusActivity != null)
		{
			bonusActivity.MyFixedUpdate();
			bonusActivity = bonusActivity.next;
		}
		if (this.player == null)
		{
			this.OnChangePlayerCar();
		}
		float num = this.player.Distance;
		num = this.player.gameObject.transform.position.z;
		int i = 0;
		int count = this.objectsToSpawn.Count;
		while (i < count)
		{
			SpawnableObjectClass spawnableObjectClass = this.objectsToSpawn[i];
			if (this.started && (spawnableObjectClass.NextSpawnObjectDistance == -1f || num >= spawnableObjectClass.NextSpawnObjectDistance))
			{
				float num2 = 1f;
				float num3 = spawnableObjectClass.startMinSpawnDistance + num2 * (spawnableObjectClass.endMinSpawnDistance - spawnableObjectClass.startMinSpawnDistance);
				float num4 = spawnableObjectClass.startMaxSpawnDistance + num2 * (spawnableObjectClass.endMaxSpawnDistance - spawnableObjectClass.startMaxSpawnDistance);
				if (spawnableObjectClass.type == ObjectsPool.ObjectType.TrafficVehicle)
				{
					//Debug.LogError("UpdateTrafficNitro");
					num3 *= this._nitroTrafficMultiplier;
					num4 *= this._nitroTrafficMultiplier;
				}
				if (spawnableObjectClass.NextSpawnObjectDistance != -1f)
				{
					this.wantToSpawn.Add(spawnableObjectClass);
				}
				if (spawnableObjectClass.type == ObjectsPool.ObjectType.Checkpoint && spawnableObjectClass.NextSpawnObjectDistance == -1f)
				{
					spawnableObjectClass.NextSpawnObjectDistance = num + num3 - 200f;
				}
				else
				{
					spawnableObjectClass.NextSpawnObjectDistance += UnityEngine.Random.Range(num3, num3);
				}
				if (spawnableObjectClass.type == ObjectsPool.ObjectType.Checkpoint)
				{
					this.nextCheckpointPosition = spawnableObjectClass.NextSpawnObjectDistance;
				}
			}
			i++;
		}
		bool flag = num - this.lastPositionSpawn > 15f;
		if (this.lastOpponentSpawn != null && flag)
		{
            //flag = (this.lastOpponentSpawn.OpponentRigidbody.position.z - this.player.PlayerRigidbody.position.z < 195f);
            flag = (Mathf.Abs(this.lastOpponentSpawn.OpponentRigidbody.position.z + this.player.PlayerRigidbody.position.z) > 195f);
            //flag = true;

		}
		bool flag2 = false;
		int count2 = this.wantToSpawn.Count;
		int num5 = -1;
		if (count2 > 0)
		{
			int num6 = 999;
			int num7 = -1;
			for (int j = 0; j < count2; j++)
			{
				if (this.wantToSpawn[j].type == ObjectsPool.ObjectType.Checkpoint)
				{
					num5 = j;
				}
				else if (this.wantToSpawn[j].alwaysSpawn)
				{
					flag2 = true;
					if (flag)
					{
						flag = true;
						num6 = this.wantToSpawn[j].priority;
						num7 = j;
					}
					else
					{
						this.spawnDelayed = this.wantToSpawn[j];
					}
				}
				else if (this.wantToSpawn[j].priority < num6 && flag && !flag2)
				{
					num6 = this.wantToSpawn[j].priority;
					num7 = j;
				}
			}
			if (num7 >= 0 /*&& this.spawnDelayed == null*/)
			{
				int quantity = (this.wantToSpawn[num7].type != ObjectsPool.ObjectType.BonusCoin) ? 1 : 5;
				float distanceBetween = (this.wantToSpawn[num7].type != ObjectsPool.ObjectType.BonusCoin) ? 0f : 5f;
				this.lastOpponentSpawn = this.SpawnObject(this.wantToSpawn[num7], quantity, distanceBetween, -1, default(Vector3)); // SPAWN TRAFFIC CAR
                this.lastPositionSpawn = num;
			}
			if (num5 >= 0)
			{
				this.SpawnObject(this.wantToSpawn[num5], 1, 0f, -1, default(Vector3)); // SPAWN TRAFFIC CAR
            }
			this.wantToSpawn.Clear();
			if (this.spawnDelayed != null)
			{
				this.wantToSpawn.Add(this.spawnDelayed);
				this.spawnDelayed = null;
			}
		}
	}

	private void ForceSpawn()
	{
		this.lastPositionSpawn = this.player.PlayerRigidbody.position.z;
	}

	private void Update()
	{
		float deltaTime = Singleton<TimeManager>.Instance.MasterSource.DeltaTime;
		this.updateTimer -= deltaTime;
		if (this.updateTimer < 0f)
		{
			this.updateTimer = 0.01f;
			float z = this.player.PlayerRigidbody.position.z;
			int num = this.carCounter;
			int num2 = Mathf.Clamp(num + this.carsEvaluatedPerFrame, 0, this.opponents.Count - 1);
			this.carCounter = ((num2 != this.opponents.Count - 1) ? num2 : 0);
			for (int i = num; i < num2; i++)
			{
				if (i >= 0 && i < this.opponents.Count)
				{
					GameObject gameObject = this.opponents[i];
					this.oppRb = gameObject.GetComponent<Rigidbody>();
					this.oppType = gameObject.GetComponent<SpawnableObjectType>().type;
					this.oppKin = gameObject.GetComponent<OpponentMovement>();
					if (gameObject != null)
					{
						bool flag = false;
						bool flag2 = false;
						bool flag3 = false;
						if (this.oppType == ObjectsPool.ObjectType.TrafficVehicle)
						{
							flag2 = (z - this.oppRb.position.z > 8f || this.oppRb.position.y > 10f);
							if (this.oppKin.MinDistanceFromFromPlayer > 0f && this.player.Distance > 200f)
							{
								flag3 = (this.oppKin.CurrentDistanceFromFromPlayer >= 250f);
							}
						}
						else if (this.oppRb != null)
						{
							flag2 = (z - this.oppRb.position.z > (float)this.objectDistanceForRemoving || this.oppRb.position.y > 10f);
						}
						if (flag2 || flag || flag3)
						{
                            //Debug.LogError("DestroyWehicle");
                            Singleton<ObjectsPool>.Instance.NotifyDestroyingExplosion(gameObject);
							Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(gameObject, this.oppType);
							this.opponents.RemoveAt(i);
						}
					}
				}
			}
		}
		for (int j = 0; j < this.freeLanes.Length; j++)
		{
			if (this.freeLanes[j] >= 0f)
			{
				this.freeLanes[j] -= deltaTime;
			}
		}
	}

	private OpponentMovement SpawnObject(SpawnableObjectClass currObj, int quantity = 0, float distanceBetween = 0f, int forceLane = -1, Vector3 positionOffset = default(Vector3))
	{
		//Debug.LogError("SPAWN TRAFFIC CAR");
		GameObject gameObject = null;
		ObjectsPool.ObjectType type = currObj.type;
		int num = (forceLane >= 0) ? forceLane : this.ChooseFreeLane(false);
		float z = this.player.PlayerRigidbody.position.z;
		for (int i = 0; i < quantity; i++)
		{
			gameObject = Singleton<ObjectsPool>.Instance.RequestObject(type);
			gameObject.transform.position = new Vector3(0f, 0f, z + 200f + (float)i * distanceBetween) + positionOffset;
			gameObject.transform.rotation = Quaternion.identity;
			if (Singleton<ObjectsPool>.Instance.IsABonus(type))
			{
				gameObject.transform.position += Vector3.forward * 5f;
			}
			if (gameObject.GetComponent<SpawnableObjectType>() == null)
			{
				gameObject.AddComponent<SpawnableObjectType>();
			}
			gameObject.GetComponent<SpawnableObjectType>().type = type;
			OpponentMovement component = gameObject.GetComponent<OpponentMovement>();
			if (type != ObjectsPool.ObjectType.Checkpoint && component != null)
			{
				component.CurrentLane = num;
				component.SendMessage("OnReset");
			}
			if (Singleton<ObjectsPool>.Instance.IsABonus(type))
			{
				if (type == ObjectsPool.ObjectType.BonusCoin)
				{
					this.freeLanes[num] = 2f;
				}
				gameObject.gameObject.SendMessage("ResetBonus", num);
			}
			if (type == ObjectsPool.ObjectType.RoadTimerSheet)
			{
				gameObject.gameObject.SendMessage("ResetSheet", num);
			}
			this.opponents.Add(gameObject);
		}
		return gameObject.GetComponent<OpponentMovement>();
	}

	internal void SpawnSingleObstacle(SpawnManager.SpawnObstacleData data)
	{
		GameObject gameObject = Singleton<ObjectsPool>.Instance.RequestObject(data.obstaclesType);
		float zOffset = data.zOffset;
		gameObject.transform.position = new Vector3(9f * GameCore.lanes[data.selectedLane], 0f, this.player.PlayerRigidbody.position.z + 200f + zOffset);
		float num = 0f;
		gameObject.transform.rotation = Quaternion.Euler(0f, 180f + num, 0f);
		this.freeLanes[data.selectedLane] = 2f;
		if (gameObject.GetComponent<SpawnableObjectType>() == null)
		{
			gameObject.AddComponent<SpawnableObjectType>();
		}
		gameObject.GetComponent<SpawnableObjectType>().type = data.obstaclesType;
		this.opponents.Add(gameObject);
	}

	internal void AvoidLaneForRoadWork(int selectedLane)
	{
		for (int i = 0; i < this.opponents.Count; i++)
		{
			GameObject gameObject = this.opponents[i];
			ObjectsPool.ObjectType type = gameObject.GetComponent<SpawnableObjectType>().type;
			if (type == ObjectsPool.ObjectType.TrafficVehicle)
			{
				OpponentMovement component = gameObject.GetComponent<OpponentMovement>();
				if (component.CurrentLane == selectedLane)
				{
					component.ChangeLaneForRoadWorks();
				}
			}
		}
	}

	private int ChooseFreeLane(bool isRival = false)
	{
		int min = 0;
		int max = 5; // LANECHANGE
        int num;
        /*if (isRival)
		{
			do
			{
				num = UnityEngine.Random.Range(min, max);
			}
			while (this.freeLanes[num] >= 0f || !this.LaneIsFree(num) || num == this.player.CurrentLane);
		}
		else
		{
			do
			{
				num = UnityEngine.Random.Range(min, max);
			}
			while (this.freeLanes[num] >= 0f || !this.LaneIsFree(num));
		}*/

        num = UnityEngine.Random.Range(min, max);
        return num;
	}

	public bool LaneIsFree(int index)
	{
		return true;
	}

	internal void OnPauseChangeLaneUpdate()
	{
		for (int i = this.opponents.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.opponents[i];
			if (gameObject != null)
			{
				ObjectsPool.ObjectType type = gameObject.GetComponent<SpawnableObjectType>().type;
				if (gameObject != null && type == ObjectsPool.ObjectType.TrafficVehicle)
				{
					gameObject.SendMessage("OnPauseChangeLaneUpdate");
				}
			}
		}
	}

	private void OnChangePlayerCar()
	{
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}

	private void OnStartGame()
	{
		this.started = true;
		this.lastPositionSpawn = 80f;
		this.lastOpponentSpawn = null;
		if (Singleton<GameCore>.Instance.gameMode == GameMode.TimeTrial)
		{
			if (this.objectsToSpawn.Count == 3)
			{
				this.objectsToSpawn.Add(this.roadSheetSpawn);
			}
		}
		else if (this.objectsToSpawn.Count == 4)
		{
			this.objectsToSpawn.RemoveAt(3);
		}
		for (int i = 0; i < this.objectsToSpawn.Count; i++)
		{
			SpawnableObjectClass spawnableObjectClass = this.objectsToSpawn[i];
			spawnableObjectClass.Reset();
		}
		for (int j = 0; j < this.freeLanes.Length; j++)
		{
			this.freeLanes[j] = -1f;
		}
		this.objectsToSpawn[this.vehicleTrafficIndex].startMinSpawnDistance = this.backupTrafficValues[0];
		this.objectsToSpawn[this.vehicleTrafficIndex].startMaxSpawnDistance = this.backupTrafficValues[1];
		this.objectsToSpawn[this.vehicleTrafficIndex].endMinSpawnDistance = this.backupTrafficValues[2];
		this.objectsToSpawn[this.vehicleTrafficIndex].endMaxSpawnDistance = this.backupTrafficValues[3];
	}

	private void OnGameover()
	{
		this.started = false;
		this.DestroyAllObjects();
	}

	private void PlayerIsResurrected()
	{
		this.lastPositionSpawn += 80f;
		Singleton<ObjectsPool>.Instance.NotifyDestroyingExplosion(null);
		for (int i = this.opponents.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.opponents[i];
			if (gameObject != null)
			{
				ObjectsPool.ObjectType type = gameObject.GetComponent<SpawnableObjectType>().type;
				if (gameObject != null && type != ObjectsPool.ObjectType.Checkpoint && !Singleton<ObjectsPool>.Instance.IsABonus(type))
				{
					Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(gameObject, type);
					this.opponents.RemoveAt(i);
				}
			}
		}
	}

	private void DestroyAllObjects()
	{
		Singleton<ObjectsPool>.Instance.NotifyDestroyingExplosion(null);
		for (int i = this.opponents.Count - 1; i >= 0; i--)
		{
			GameObject gameObject = this.opponents[i];
			if (gameObject != null)
			{
				Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(gameObject, gameObject.GetComponent<SpawnableObjectType>().type);
				this.opponents.RemoveAt(i);
			}
		}
	}

	internal void ActivateMulCar(GameObject chosenCar, Sprite sp)
	{
		ObjectsPool.ObjectType type = ObjectsPool.ObjectType.MultModeCar;
		GameObject gameObject = Singleton<ObjectsPool>.Instance.RequestObject(type);
		RivalCar component = gameObject.GetComponent<RivalCar>();
		float z = this.player.PlayerRigidbody.position.z;
		gameObject.transform.position = new Vector3(0f, 0f, z - 10f);
		gameObject.transform.rotation = Quaternion.identity;
		if (gameObject.GetComponent<SpawnableObjectType>() == null)
		{
			gameObject.AddComponent<SpawnableObjectType>();
		}
		gameObject.GetComponent<SpawnableObjectType>().type = type;
		component.SetupCar(chosenCar, this.ChooseFreeLane(true), sp);
	}

	public PlayerMovement player;

	internal int checkpointDistance = 100000;

	private int objectDistanceForRemoving = 30;

	public List<SpawnableObjectClass> objectsToSpawn;

	public SpawnableObjectClass roadSheetSpawn;

	private List<GameObject> opponents;

	private SpawnableObjectClass spawnDelayed;

	private List<SpawnableObjectClass> wantToSpawn;

	private float[] freeLanes;

	private bool started;

	private float lastPositionSpawn;

	private OpponentMovement lastOpponentSpawn;

	private float nextCheckpointPosition = -1f;

	private float _nitroTrafficMultiplier = 1f;

	private int vehicleTrafficIndex;

	private float[] backupTrafficValues;

	public ListG<OpponentMovement> __opponents = new ListG<OpponentMovement>();

	public ListG<BonusActivity> __bonuses = new ListG<BonusActivity>();

	private float updateTimer = 0.1f;

	private Rigidbody oppRb;

	private ObjectsPool.ObjectType oppType;

	private OpponentMovement oppKin;

	private int carCounter;

	private int carsEvaluatedPerFrame = 4;

	private float initialPlayerDistance;

	public struct SpawnObstacleData
	{
		public ObjectsPool.ObjectType obstaclesType;

		public int selectedLane;

		public float zOffset;
	}
}
