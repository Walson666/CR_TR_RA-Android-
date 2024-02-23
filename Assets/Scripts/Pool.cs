// dnSpy decompiler from Assembly-CSharp.dll class: Pool
using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
	public Pool(int poolDim)
	{
		this.pool = new List<Pool.PoolObject>(poolDim);
	}

	public void AddObject(GameObject go)
	{
		this.pool.Add(new Pool.PoolObject(go));
	}

	public GameObject GetFreeObject()
	{
		int i = UnityEngine.Random.Range(0, this.pool.Count);
		int num = 0;
		while (i < 2147483647)
		{
			if (i >= this.pool.Count)
			{
				i = 0;
			}
			Pool.PoolObject poolObject = this.pool[i];
			if (poolObject.isFree)
			{
				poolObject.isFree = false;
				return poolObject.Object;
			}
			num++;
			if (num == this.pool.Count)
			{
				break;
			}
			i++;
		}
		return null;
	}

	public void SetFreeObject(GameObject freedObject)
	{
		freedObject.transform.parent = null;
		foreach (Pool.PoolObject poolObject in this.pool)
		{
			if (poolObject.Object == freedObject)
			{
				poolObject.isFree = true;
				break;
			}
		}
	}

	public void SetUsedObject(GameObject freedObject)
	{
		freedObject.transform.parent = null;
		foreach (Pool.PoolObject poolObject in this.pool)
		{
			if (poolObject.Object == freedObject)
			{
				poolObject.isFree = false;
				break;
			}
		}
	}

	public List<Pool.PoolObject> pool;

	public class PoolObject
	{
		public PoolObject(GameObject go)
		{
			this.gameObject = go;
			this.isFree = true;
		}

		public GameObject Object
		{
			get
			{
				return this.gameObject;
			}
		}

		private GameObject gameObject;

		public bool isFree;
	}
}
