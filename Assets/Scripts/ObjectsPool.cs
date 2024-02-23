// dnSpy decompiler from Assembly-CSharp.dll class: ObjectsPool
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : Singleton<ObjectsPool>
{
	public bool IsABonus(ObjectsPool.ObjectType checkType)
	{
		return checkType == ObjectsPool.ObjectType.BonusCoin;
	}

	private void FillPools()
	{
		this.poolsByType = new Dictionary<ObjectsPool.ObjectType, Pool>();
		this.instancesCountersByType = new Dictionary<ObjectsPool.ObjectType, int>();
		for (int i = 0; i < this.poolList.Count; i++)
		{
			this.InitializePool(this.poolList[i].quantity, this.poolList[i].objType);
		}
	}

	public void DestroyPoolElements()
	{
		foreach (KeyValuePair<ObjectsPool.ObjectType, Pool> keyValuePair in this.poolsByType)
		{
			List<Pool.PoolObject> pool = keyValuePair.Value.pool;
			foreach (Pool.PoolObject poolObject in pool)
			{
				UnityEngine.Object.Destroy(poolObject.Object);
			}
			pool.Clear();
		}
	}

	private void ResetPool()
	{
	}

	private void InitializePool(int poolDim, ObjectsPool.ObjectType type)
	{
		Pool pool = this.CreatePool(poolDim, type);
		this.instancesCountersByType.Add(type, 0);
		if (pool != null)
		{
			pool = this.InstantiateObjects(type, pool, poolDim);
		}
		this.poolsByType.Add(type, pool);
	}

	private Pool CreatePool(int poolDim, ObjectsPool.ObjectType type)
	{
		Pool result;
		if (poolDim <= 0 || !this.IsObjectPrefabAvailable(type))
		{
			result = null;
		}
		else
		{
			result = new Pool(poolDim);
		}
		return result;
	}

	private Pool InstantiateObjects(ObjectsPool.ObjectType type, Pool pool, int poolDim)
	{
		int num = this.ObjectTypePrefabNumber(type);
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < poolDim; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ObjectTypeToPrefab(type, i), Vector3.zero, Quaternion.identity);
				gameObject.name = string.Concat(new object[]
				{
					type.ToString(),
					"_",
					this.instancesCountersByType[type],
					"_",
					i
				});
				if (type == ObjectsPool.ObjectType.Smoke || type == ObjectsPool.ObjectType.Sparks)
				{
					EffectActivity effectActivity = gameObject.AddComponent<EffectActivity>();
					effectActivity.effectType = type;
				}
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.SetActiveRecursivelyL(false);
				this.instancesCountersByType[type] = this.instancesCountersByType[type] + 1;
				pool.AddObject(gameObject);
			}
		}
		return pool;
	}

	private int ObjectTypePrefabNumber(ObjectsPool.ObjectType type)
	{
		int result = 0;
		for (int i = 0; i < this.poolList.Count; i++)
		{
			if (this.poolList[i].objType == type)
			{
				result = this.poolList[i].obj_Prefab.Count;
				break;
			}
		}
		return result;
	}

	private bool IsObjectPrefabAvailable(ObjectsPool.ObjectType type)
	{
		return 0 != this.ObjectTypePrefabNumber(type);
	}

	private GameObject ObjectTypeToPrefab(ObjectsPool.ObjectType type, int index)
	{
		GameObject result = null;
		for (int i = 0; i < this.poolList.Count; i++)
		{
			if (this.poolList[i].objType == type)
			{
				result = this.poolList[i].obj_Prefab[index];
				break;
			}
		}
		return result;
	}

	private GameObject GetUnusedObject(ObjectsPool.ObjectType type)
	{
		Pool pool;
		if (this.poolsByType.TryGetValue(type, out pool))
		{
			GameObject gameObject = this.GetUnusedObjectFromPool(pool);
			if (gameObject == null)
			{
				UnityEngine.Debug.Log("Pool limit exceeded for " + type + " Increasing size of pool now!");
				int num = 0;
				if (type == ObjectsPool.ObjectType.TrafficVehicle)
				{
					num = UnityEngine.Random.Range(0, this.ObjectTypePrefabNumber(type));
				}
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ObjectTypeToPrefab(type, num), Vector3.zero, Quaternion.identity);
				gameObject.name = string.Concat(new object[]
				{
					type.ToString(),
					"_",
					this.instancesCountersByType[type],
					"_",
					num
				});
				if (type == ObjectsPool.ObjectType.Smoke || type == ObjectsPool.ObjectType.Sparks)
				{
					EffectActivity effectActivity = gameObject.AddComponent<EffectActivity>();
					effectActivity.effectType = type;
				}
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				gameObject.SetActiveRecursivelyL(false);
				this.instancesCountersByType[type] = this.instancesCountersByType[type] + 1;
				pool.AddObject(gameObject);
				pool.SetUsedObject(gameObject);
			}
			return gameObject;
		}
		return null;
	}

	private GameObject GetUnusedObjectFromPool(Pool pool)
	{
		if (pool == null)
		{
			return null;
		}
		return pool.GetFreeObject();
	}

	public GameObject RequestObject(ObjectsPool.ObjectType type)
	{
		GameObject unusedObject = this.GetUnusedObject(type);
		unusedObject.SetActiveRecursivelyL(true);
		return unusedObject;
	}

	public void RequestEffect(Transform spawnTransform, Vector3 offset, ObjectsPool.ObjectType type, bool parentToTransform)
	{
		GameObject unusedObject = this.GetUnusedObject(type);
		if (unusedObject == null)
		{
			return;
		}
		if (type == ObjectsPool.ObjectType.Smoke)
		{
			offset += Vector3.up * 1f;
		}
		unusedObject.SetActiveRecursivelyL(true);
		unusedObject.SendMessage("Spawn", new EffectActivity.EffectSpawnParameters(spawnTransform, offset, parentToTransform));
	}

	public void RequestEffectInPoint(Vector3 point, Vector3 offset, ObjectsPool.ObjectType type)
	{
		GameObject unusedObject = this.GetUnusedObject(type);
		if (unusedObject == null)
		{
			return;
		}
		unusedObject.SetActiveRecursivelyL(true);
		unusedObject.SendMessage("SpawnInPoint", point);
	}

	public void EffectFinshed(GameObject effectGO)
	{
		ObjectsPool.ObjectType effectType = effectGO.GetComponent<EffectActivity>().effectType;
		this.NotifyDestroyingParent(effectGO, effectType);
	}

	public void NotifyDestroyingExplosion(GameObject parent = null)
	{
		if (parent == null)
		{
			for (int i = 2; i <= 3; i++)
			{
				Pool pool = this.poolsByType[(ObjectsPool.ObjectType)i];
				foreach (Pool.PoolObject poolObject in pool.pool)
				{
					if (!poolObject.isFree)
					{
						pool.SetFreeObject(poolObject.Object);
						poolObject.Object.SetActiveRecursivelyL(false);
					}
				}
			}
		}
		else
		{
			List<GameObject> list = new List<GameObject>();
			List<ObjectsPool.ObjectType> list2 = new List<ObjectsPool.ObjectType>();
			for (int j = 0; j < parent.transform.childCount; j++)
			{
				GameObject gameObject = parent.transform.GetChild(j).gameObject;
				ObjectsPool.ObjectType objectType = ObjectsPool.ObjectType.Count;
				if (gameObject.name.Contains("Smoke"))
				{
					objectType = ObjectsPool.ObjectType.Smoke;
				}
				if (objectType != ObjectsPool.ObjectType.Count)
				{
					list2.Add(objectType);
					list.Add(gameObject);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				Pool pool2;
				if (this.poolsByType.TryGetValue(list2[k], out pool2))
				{
					pool2.SetFreeObject(list[k]);
					list[k].SetActiveRecursivelyL(false);
				}
			}
		}
	}

	public void NotifyDestroyingParent(GameObject currObject, ObjectsPool.ObjectType type)
	{
		if (type == ObjectsPool.ObjectType.TrafficVehicle && currObject.GetComponent<OpponentMovement>() != null)
		{
			currObject.GetComponent<OpponentMovement>().OnResetVehicle();
		}
		if (type == ObjectsPool.ObjectType.MultModeCar && currObject.GetComponent<RivalCar>() != null)
		{
			currObject.GetComponent<RivalCar>().OnResetVehicle();
		}
		Pool pool;
		if (this.poolsByType.TryGetValue(type, out pool))
		{
			pool.SetFreeObject(currObject);
			currObject.SetActiveRecursivelyL(false);
		}
		else
		{
			UnityEngine.Debug.LogError("ERROR - NO OBJECTS FOUND IN POOL Of Name>>> " + currObject.name);
		}
	}

	public void AddNewItems()
	{
		this.FillPools();
	}

	public List<ObjectsPool.PoolData> poolList;

	private Dictionary<ObjectsPool.ObjectType, Pool> poolsByType;

	private Dictionary<ObjectsPool.ObjectType, int> instancesCountersByType;

	[Serializable]
	public class PoolData
	{
		public ObjectsPool.ObjectType objType;

		public List<GameObject> obj_Prefab;

		public int quantity;
	}

	public enum ObjectType
	{
		BonusCoin,
		PickupCoins,
		Smoke,
		Sparks,
		SideCollision,
		Checkpoint,
		TrafficVehicle,
		MultModeCar,
		RoadTimerSheet,
		Count
	}
}
