// dnSpy decompiler from Assembly-CSharp.dll class: EffectActivity
using System;
using System.Collections;
using UnityEngine;

public class EffectActivity : MonoBehaviour
{
	private void Awake()
	{
		this.pSys = base.gameObject.GetComponent<ParticleSystem>();
		if (this.pSys != null)
		{
			this.pSys.Stop(true);
		}
	}

	private void Spawn(EffectActivity.EffectSpawnParameters spawn)
	{
		base.transform.position = spawn.SpawnTransform.position + spawn.SpawnOffset;
		base.transform.rotation = spawn.SpawnTransform.rotation;
		if (spawn.ParentToTransform)
		{
			base.transform.parent = spawn.SpawnTransform;
		}
		if (!base.transform.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.pSys != null)
		{
			this.pSys.Clear(true);
			this.pSys.Play(true);
			base.StartCoroutine(this.ReleaseCoroutine());
		}
		else
		{
			base.StartCoroutine(this.ReleaseCoroutineNoParticles());
		}
	}

	private void SpawnInPoint(Vector3 spawnPoint)
	{
		base.transform.position = spawnPoint;
		if (this.pSys != null)
		{
			this.pSys.Play(true);
		}
		base.StartCoroutine(this.ReleaseCoroutine());
	}

	private IEnumerator ReleaseCoroutine()
	{
		yield return new WaitForSeconds(this.pSys.main.duration);
		base.StartCoroutine(this.ReleaseCoroutineWParticles());
		yield break;
	}

	private IEnumerator ReleaseCoroutineWParticles()
	{
		yield return new WaitForSeconds(0.5f);
		if (this.pSys != null)
		{
			this.pSys.Stop(true);
		}
		Singleton<ObjectsPool>.Instance.EffectFinshed(base.gameObject);
		yield break;
	}

	private IEnumerator ReleaseCoroutineNoParticles()
	{
		yield return new WaitForSeconds(2f);
		Singleton<ObjectsPool>.Instance.EffectFinshed(base.gameObject);
		yield break;
	}

	private void OnGameover()
	{
		Singleton<ObjectsPool>.Instance.EffectFinshed(base.gameObject);
	}

	public ObjectsPool.ObjectType effectType;

	private ParticleSystem pSys;

	public class EffectSpawnParameters
	{
		public EffectSpawnParameters(Transform spawnTransform, Vector3 spawnOffset, bool parentToTransform)
		{
			this.spawnTransform = spawnTransform;
			this.spawnOffset = spawnOffset;
			this.shouldParentToTransform = parentToTransform;
		}

		public Transform SpawnTransform
		{
			get
			{
				return this.spawnTransform;
			}
		}

		public Vector3 SpawnOffset
		{
			get
			{
				return this.spawnOffset;
			}
		}

		public bool ParentToTransform
		{
			get
			{
				return this.shouldParentToTransform;
			}
		}

		public Vector3 SpawnPoint
		{
			get
			{
				return this.spawnPoint;
			}
			set
			{
				this.spawnPoint = value;
			}
		}

		private Transform spawnTransform;

		private Vector3 spawnOffset;

		private Vector3 spawnPoint;

		private bool shouldParentToTransform;
	}
}
