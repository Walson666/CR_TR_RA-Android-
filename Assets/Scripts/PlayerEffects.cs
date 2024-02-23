// dnSpy decompiler from Assembly-CSharp.dll class: PlayerEffects
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
	private bool EffectAlredyCreated(Transform parentToAttach, ParticleSystem effectRef)
	{
		bool result = false;
		for (int i = 0; i < parentToAttach.childCount; i++)
		{
			Transform child = parentToAttach.GetChild(i);
			if (child.name.Equals(effectRef.name))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private bool EffectAlredyCreated(Transform parentToAttach, GameObject effectRef)
	{
		bool result = false;
		for (int i = 0; i < parentToAttach.childCount; i++)
		{
			Transform child = parentToAttach.GetChild(i);
			if (child.name.Equals(effectRef.name))
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private ParticleSystem EffectAlredyCreated(Transform parentToAttach, ParticleSystem effectRef, bool b)
	{
		ParticleSystem result = null;
		for (int i = 0; i < parentToAttach.childCount; i++)
		{
			Transform child = parentToAttach.GetChild(i);
			if (child.name.StartsWith(effectRef.name))
			{
				result = child.GetComponent<ParticleSystem>();
				break;
			}
		}
		return result;
	}

	private Transform GetExistingEffect(Transform parentToAttach, string effectRefName)
	{
		Transform transform = parentToAttach.GetChild(0).transform;
		for (int i = 0; i < parentToAttach.childCount; i++)
		{
			Transform child = parentToAttach.GetChild(i);
			if (child.name.Equals(effectRefName))
			{
				transform = child.transform;
				break;
			}
		}
		return transform;
	}

	private void InitializeEffect(out GameObject effect, GameObject effectRef, string placeHolderName, bool attachToPlayer = true)
	{
		Transform transform = base.gameObject.transform.Find(placeHolderName).transform;
		if (!attachToPlayer || !this.EffectAlredyCreated(transform, effectRef))
		{
			effect = UnityEngine.Object.Instantiate<GameObject>(effectRef, Vector3.zero, effectRef.transform.rotation);
			if (attachToPlayer)
			{
				effect.transform.parent = base.gameObject.transform.Find(placeHolderName).transform;
			}
			effect.transform.localPosition = Vector3.zero;
			effect.transform.localRotation = effectRef.transform.localRotation;
			effect.transform.name = effectRef.name;
		}
		else
		{
			effect = this.GetExistingEffect(transform, effectRef.name).gameObject;
		}
	}

	public void ActivateNitroFx(bool active)
	{
		for (int i = 0; i < this.rearNitro.Length; i++)
		{
			this.rearNitro[i].gameObject.SetActive(active);
		}
	}

	private void OnStartGame()
	{
		if (this.shadow != null)
		{
			this.shadow.SetActive(true);
		}
	}

	private void OnActivateLights(bool active)
	{
		this.lightsActive = active;
		if (this.tunnelLights != null)
		{
			foreach (GameObject gameObject in this.tunnelLights)
			{
				gameObject.SetActive(active);
			}
		}
	}

	private void Start()
	{
		this.pD = base.gameObject.GetComponent<PlayerMovement>();
		this.ActivateNitroFx(false);
		this.tunnelLights = new List<GameObject>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.tag == "HeadLights")
				{
					this.tunnelLights.Add(transform.gameObject);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.OnActivateLights(false);
		this.ShowDrift(false, false);
		this.BackWashStart();
	}

	private void Update()
	{
		if (this.lightsActive && !this.tunnelLights[0].activeInHierarchy)
		{
			this.OnActivateLights(true);
		}
	}

	private void Awake()
	{
		this.ignoreLayers |= (1 << LayerMask.NameToLayer("Ignore Raycast") | 1 << LayerMask.NameToLayer("Blocks") | 1 << LayerMask.NameToLayer("Bonus") | 1 << LayerMask.NameToLayer("Player"));
	}

	private void BackWashStart()
	{
		this.isInDraft = false;
		this.draftStartSource = base.gameObject.AddComponent<AudioSource>();
		this.draftStartSource.volume = 0.9f;
		this.draftStartSource.rolloffMode = AudioRolloffMode.Linear;
		this.draftStartSource.minDistance = 1f;
		this.draftStartSource.maxDistance = 30f;
		this.draftStartSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
		this.draftStartSource.playOnAwake = false;
		this.draftStartSource.clip = Singleton<SoundManager>.Instance.pickupBackwash;
		this.waitForDraftTimer = this.waitForDraftDuration;
	}

	private void FixedUpdate()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		if (!Singleton<GamePlay>.Instance.GamePlayStarted && !this.pD.PlayerIsStopping)
		{
			if (this.isInDraft)
			{
				this.isInDraft = false;
				this.lastHitCollider = null;
			}
			return;
		}
		if (!this.pD.PlayerIsDead && !this.pD.NitroActive && !this.pD.IsOnCollisionEffect)
		{
			float d = (!this.pD.WrongDirection) ? 3f : 30f;
			Vector3 origin = base.gameObject.transform.position + Vector3.forward * d + Vector3.up * 1f;
			Vector3 direction = base.gameObject.transform.position + Vector3.forward * 10f + Vector3.up * 1f;
			RaycastHit raycastHit;
			Physics.Raycast(origin, direction, out raycastHit, 10f, ~this.ignoreLayers);
			if (raycastHit.collider && this.lastHitCollider != raycastHit.collider)
			{
				if (this.waitForDraftTimer >= 0f)
				{
					this.waitForDraftTimer -= fixedDeltaTime;
				}
				else
				{
					this.isInDraft = true;
					if (this.lastHitCollider != raycastHit.collider)
					{
						this.endDraftTimer = -1f;
						Singleton<GamePlay>.Instance.UpdateNearMiss(1);
						this.lastHitCollider = raycastHit.collider;
						Singleton<SoundManager>.Instance.PlaySource(this.draftStartSource, false);
					}
				}
			}
			else
			{
				if (this.isInDraft && this.endDraftTimer < 0f)
				{
					this.endDraftTimer = this.draftDuration;
				}
				this.waitForDraftTimer = this.waitForDraftDuration;
			}
			if (this.endDraftTimer >= 0f)
			{
				this.endDraftTimer -= fixedDeltaTime;
				if (this.endDraftTimer < 0f)
				{
					this.endDraftTimer = -1f;
					if (this.isInDraft)
					{
						this.waitForDraftTimer = this.waitForDraftDuration;
					}
					this.isInDraft = false;
					this.lastHitCollider = null;
				}
			}
		}
		else if (this.isInDraft)
		{
			this.isInDraft = false;
			this.lastHitCollider = null;
		}
	}

	public void PalyerHasCollided()
	{
		this.endDraftTimer = -1f;
		this.isInDraft = false;
	}

	public void ShowDrift(bool left, bool active)
	{
		this.leftDrift.SetActive(left && active);
		this.rightDrift.SetActive(!left && active);
	}

	private void OnStartRunning()
	{
		this.lastHitCollider = null;
	}

	private void OnDead()
	{
		base.StopAllCoroutines();
	}

	internal void BlinkCar()
	{
		if (!this.alreadyBlinking)
		{
			base.StartCoroutine(this.SetBlinkOnOffRoutine());
		}
	}

	private IEnumerator SetBlinkOnOffRoutine()
	{
		this.alreadyBlinking = true; this.blink = (this.alreadyBlinking );
		base.StartCoroutine(this.BlinkRoutine());
		yield return new WaitForSeconds(2f);
		this.pD.carCust.SetActive(true);
		this.alreadyBlinking = false; this.blink = (this.alreadyBlinking );
		yield break;
	}

	private IEnumerator BlinkRoutine()
	{
		bool state = true;
		while (this.blink)
		{
			state = !state;
			if (this.pD)
			{
				this.pD.carCust.SetActive(state);
			}
			yield return new WaitForSeconds(0.08f);
		}
		yield break;
	}

	public GameObject shadow;

	public ParticleSystem[] rearNitro;

	public GameObject leftDrift;

	public GameObject rightDrift;

	private PlayerMovement pD;

	private bool lightsActive;

	private List<GameObject> tunnelLights;

	private float draftDuration = 1.7f;

	internal float draftDeltaSpeed = 1f;

	internal float draftAcceleration = 1f;

	private AudioSource draftStartSource;

	private bool isInDraft;

	private float endDraftTimer = -1f;

	private Collider lastHitCollider;

	private LayerMask ignoreLayers = 0;

	private float waitForDraftDuration;

	private float waitForDraftTimer = -1f;

	private bool blink;

	private bool alreadyBlinking;
}
