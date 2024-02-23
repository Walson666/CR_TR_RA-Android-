// dnSpy decompiler from Assembly-CSharp.dll class: PlayerSounds
using System;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
	public AudioSource FreeAudioSource(AudioSource[] sources)
	{
		for (int i = 0; i < sources.Length; i++)
		{
			if (!sources[i].isPlaying)
			{
				return sources[i];
			}
		}
		return null;
	}

	public AudioClip GetRandomClip(AudioClip[] clipsArray)
	{
		return clipsArray[UnityEngine.Random.Range(0, 10000 % clipsArray.Length)];
	}

	private void OnStartGame()
	{
		this.engineSource.Stop();
		this.engineSource.volume = 1f;
		this.engineSource.pitch = this.minEnginePitch;
	}

	private void OnChangePlayerCar()
	{
		this.engineSource.Stop();
		this.engineSource.pitch = this.minEnginePitch;
		this.engineSourceDelayTimer = 1f;
		this.engineSource.volume = 1f;
	}

	private void OnPausePlayerEngine()
	{
		this.engineSource.volume = 0f;
	}

	private void OnRestartPlayerEngine()
	{
		this.engineSource.volume = 1f;
	}

	public void UpdatePlayerEngineVolume(float vol)
	{
		this.engineSource.volume = vol;
	}

	private void PlayPlayerSound(PlayerSounds.PlayerSoundsType soundType)
	{
		AudioSource audioSource = this.FreeAudioSource(this.playerSources);
		if (audioSource != null)
		{
			if (soundType != PlayerSounds.PlayerSoundsType.Hit)
			{
				if (soundType == PlayerSounds.PlayerSoundsType.Nitro)
				{
					audioSource.clip = Singleton<SoundManager>.Instance.nitroStartClip;
				}
			}
			else
			{
				audioSource.clip = Singleton<SoundManager>.Instance.bang;
			}
			Singleton<SoundManager>.Instance.PlaySource(audioSource, false);
		}
	}

	private void OnNitroActive(bool active)
	{
		if (active)
		{
			this.PlayPlayerSound(PlayerSounds.PlayerSoundsType.Nitro);
		}
	}

	private void OnBackwashActive(bool active)
	{
		if (active)
		{
			this.PlayPlayerSound(PlayerSounds.PlayerSoundsType.Nitro);
		}
	}

	private void Awake()
	{
		this.playerSources = new AudioSource[3];
		for (int i = 0; i < this.playerSources.Length; i++)
		{
			this.playerSources[i] = base.gameObject.AddComponent<AudioSource>();
			this.playerSources[i].volume = 1f;
			this.playerSources[i].loop = false;
			this.playerSources[i].playOnAwake = false;
			this.playerSources[i].rolloffMode = AudioRolloffMode.Linear;
			this.playerSources[i].minDistance = 10f;
			this.playerSources[i].maxDistance = 150f;
			this.playerSources[i].velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
		}
		this.engineSource = base.gameObject.AddComponent<AudioSource>();
		this.engineSource.volume = 0f;
		this.engineSource.playOnAwake = false;
		this.engineSource.loop = true;
		this.engineSource.clip = Singleton<SoundManager>.Instance.engineLoop;
		this.engineSource.pitch = this.minEnginePitch;
		this.engineSource.clip = Singleton<SoundManager>.Instance.engineLoop;
		this.engineSource.rolloffMode = AudioRolloffMode.Linear;
		this.engineSource.minDistance = 1f;
		this.engineSource.maxDistance = 60f;
		this.engineSource.velocityUpdateMode = AudioVelocityUpdateMode.Fixed;
	}

	private void Start()
	{
		this.playerKinematics = base.gameObject.GetComponent<PlayerMovement>();
	}

	private void Update()
	{
		float deltaTime = Singleton<TimeManager>.Instance.MasterSource.DeltaTime;
		if (this.playerKinematics.Speed > 0f && !this.engineSource.isPlaying && !this.playerKinematics.PlayerIsDead && !Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			Singleton<SoundManager>.Instance.PlaySource(this.engineSource, false);
		}
		if (this.engineSource.isPlaying && !this.playerKinematics.PlayerIsDead)
		{
			this.engineSource.pitch = Mathf.Clamp(this.minEnginePitch + this.playerKinematics.Speed * 0.015f, this.minEnginePitch, 2.4f);
		}
		if (this.playerKinematics.PlayerIsDead)
		{
			this.engineSource.pitch = this.minEnginePitch;
		}
		if (this.engineSourceDelayTimer >= 0f && !Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			this.engineSourceDelayTimer -= deltaTime;
			if (this.engineSourceDelayTimer < 0f)
			{
				this.engineSourceDelayTimer = -1f;
				this.OnRestartPlayerEngine();
			}
		}
	}

	private AudioSource[] playerSources;

	private AudioSource engineSource;

	private float engineSourceDelayTimer = -1f;

	private PlayerMovement playerKinematics;

	private float minEnginePitch = 0.5f;

	public enum PlayerSoundsType
	{
		Hit,
		Nitro
	}
}
