// dnSpy decompiler from Assembly-CSharp.dll class: SpawnableObjectClass
using System;

[Serializable]
public class SpawnableObjectClass
{
	public SpawnableObjectClass()
	{
	}

	public SpawnableObjectClass(ObjectsPool.ObjectType _type, int _priority, float _startMinSpawnDistance, float _startMaxSpawnDistance, float _endMinSpawnDistance, float _endMaxSpawnDistance)
	{
		this.type = _type;
		this.newObjectDistance = -1f;
		this.priority = _priority;
		this.startMinSpawnDistance = _startMinSpawnDistance;
		this.startMaxSpawnDistance = _startMaxSpawnDistance;
		this.endMinSpawnDistance = _endMinSpawnDistance;
		this.endMaxSpawnDistance = _endMaxSpawnDistance;
	}

	public float NextSpawnObjectDistance
	{
		get
		{
			return this.newObjectDistance;
		}
		set
		{
			this.newObjectDistance = value;
		}
	}

	public void Reset()
	{
		this.newObjectDistance = -1f;
	}

	public float startMinSpawnDistance;

	public float startMaxSpawnDistance;

	public float endMinSpawnDistance;

	public float endMaxSpawnDistance;

	public int priority;

	public ObjectsPool.ObjectType type;

	public bool alwaysSpawn;

	private float newObjectDistance;
}
