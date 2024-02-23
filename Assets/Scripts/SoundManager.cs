// dnSpy decompiler from Assembly-CSharp.dll class: SoundManager
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
	public float MusicVolume
	{
		get
		{
			return this.musicVol;
		}
		set
		{
			this.musicVol = value;
			this.UpdateMusicOnOff();
		}
	}

	public void PlayInGameMusic()
	{
		this.StopMusicSource();
		this.musicClip = null;
		this.StopAllSources();
		AudioClip audioClip = this.ingameMusic;
		if (null == audioClip)
		{
			audioClip = this.ingameMusic;
		}
		this.bgSource.clip = audioClip;
		this.bgSource.loop = true;
		this.bgSource.mute = false;
		this.musicClip = this.ingameMusic;
		this.PlayMusicSource(this.bgSource);
	}

	public bool IsInGameMusicPlaying()
	{
		return this.bgSource.isPlaying;
	}

	public void ResetIngameMusic()
	{
		this.changeBgVolume = false;
		this.bgSource.volume = 1f;
		this.StopMusicSource();
	}

	public void CheckForMusicDownVolume(AudioSource source)
	{
		if (this.isMusicDownActive)
		{
			source.volume = 0.5f;
		}
		else
		{
			source.volume = 1f;
		}
	}

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

	public void PlayGeneralGameSound(SoundManager.GeneralGameSounds soundType)
	{
		AudioSource audioSource = this.FreeAudioSource(this.gameSoundsSources);
		if (audioSource != null)
		{
			if (soundType != SoundManager.GeneralGameSounds.BackFromBoost)
			{
				if (soundType == SoundManager.GeneralGameSounds.MissionCompleted)
				{
					audioSource.clip = this.missionCompleted;
				}
			}
			else
			{
				audioSource.clip = this.backToNormal;
			}
			this.CheckForMusicDownVolume(audioSource);
			this.PlaySource(audioSource, false);
		}
	}

	public void PlayBonusCollected(ObjectsPool.ObjectType bonusType)
	{
		AudioSource audioSource = this.FreeAudioSource(this.gameSoundsSources);
		if (audioSource != null)
		{
			if (bonusType == ObjectsPool.ObjectType.BonusCoin)
			{
				audioSource.clip = this.pickupCoin;
			}
			this.CheckForMusicDownVolume(audioSource);
			this.PlaySource(audioSource, false);
		}
	}

	public void PlayMetersOk()
	{
		this.metersSource.clip = this.metersGood;
		this.PlaySource(this.metersSource, false);
	}

	public void bgVolumeControl(bool down)
	{
		this.isMusicDownActive = down;
		this.fadeStartTime = Time.time;
		this.changeBgVolume = true;
		this.startVolume = this.bgSource.volume;
		if (down)
		{
			this.finalVolume = 0.1f;
		}
		else
		{
			this.finalVolume = 1f;
		}
	}

	private void Awake()
	{
		this.musicOn = (PlayerPrefs.GetInt("sm_mu", 1) == 1);
		this.soundsOn = (PlayerPrefs.GetInt("sm_sf", 1) == 1);
		this.musicTgl.Enable = this.musicOn;
		this.soundTgl.Enable = this.soundsOn;
		this.InitFxSources();
		this.InitInterfaceSounds();
		this.bgSource = base.gameObject.AddComponent<AudioSource>();
		this.bgSource.volume = 1f;
		this.bonusSource = base.gameObject.AddComponent<AudioSource>();
		this.bonusSource.volume = 1f;
		this.metersSource = base.gameObject.AddComponent<AudioSource>();
		this.metersSource.volume = 1f;
		this.startVolume = this.bgSource.volume;
		this.finalVolume = 0.1f;
		this.gameSoundsSources = new AudioSource[this.gameSourcesNumber];
		for (int i = 0; i < this.gameSoundsSources.Length; i++)
		{
			this.gameSoundsSources[i] = base.gameObject.AddComponent<AudioSource>();
			this.gameSoundsSources[i].playOnAwake = false;
			this.gameSoundsSources[i].volume = 1f;
			this.gameSoundsSources[i].loop = false;
		}
	}

	private void Start()
	{
		this.startVolume = this.bgSource.volume;
		this.PlayOffGameMusic();
	}

	private void Update()
	{
		float time = Time.time;
		if (this.changeBgVolume && !Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			if (this.playerEngineSound == null)
			{
				this.playerEngineSound = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSounds>();
			}
			float num;
			if (this.startVolume > this.finalVolume)
			{
				num = time - this.fadeStartTime;
				if (this.bgSource.volume <= this.finalVolume)
				{
					this.playerEngineSound.UpdatePlayerEngineVolume(this.finalVolume);
					this.bgSource.volume = this.finalVolume;
					this.changeBgVolume = false;
				}
			}
			else
			{
				num = this.fadeStartTime - time;
				if (this.bgSource.volume >= this.finalVolume)
				{
					this.playerEngineSound.UpdatePlayerEngineVolume(this.finalVolume);
					this.bgSource.volume = this.finalVolume;
					this.changeBgVolume = false;
				}
			}
			this.bgSource.volume = this.startVolume - num * 0.6f;
			this.playerEngineSound.UpdatePlayerEngineVolume(this.bgSource.volume);
		}
	}

	private void InitFxSources()
	{
		this.fxSources = new SoundManager.FxSoundSource[this.fxSourcesNum];
		for (int i = 0; i < this.fxSourcesNum; i++)
		{
			this.fxSources[i] = new SoundManager.FxSoundSource();
			this.fxSources[i].go.name = "FxSoundSource_" + i.ToString();
		}
	}

	public SoundManager.FxSoundSource FreeAudioSource(SoundManager.FxSoundSource[] sourcesGO)
	{
		for (int i = 0; i < sourcesGO.Length; i++)
		{
			if (!sourcesGO[i].s.isPlaying)
			{
				return sourcesGO[i];
			}
		}
		return null;
	}

	private void StopAllSources()
	{
		foreach (SoundManager.FxSoundSource fxSoundSource in this.fxSources)
		{
			fxSoundSource.s.Stop();
		}
		this.jingleSource.Stop();
	}

	public AudioSource FreeAudioSourceInt(AudioSource[] sources)
	{
		if (sources == null)
		{
			return null;
		}
		for (int i = 0; i < sources.Length; i++)
		{
			if (!sources[i].isPlaying)
			{
				return sources[i];
			}
		}
		return null;
	}

	public void PlayOffGameMusic()
	{
		AudioClip clip = this.musicMainMenu;
		this.StopMusicSource();
		this.musicClip = null;
		this.musicSource.clip = clip;
		this.musicClip = clip;
		this.PlayMusicSource(this.musicSource);
		this.MusicVolume = Singleton<SoundManager>.Instance.offGameBaseVolume;
	}

	private void StopOffGameMusic()
	{
		this.StopMusicSource();
	}

	private void PlayerIsDead()
	{
	}

	public void PlayCheckpointSound()
	{
		AudioSource audioSource = this.FreeAudioSourceInt(this.interfaceSources);
		audioSource.clip = this.reachedMileStone;
		this.PlaySource(audioSource, false);
	}

	public void Click()
	{
		AudioSource audioSource = this.FreeAudioSourceInt(this.interfaceSources);
		if (audioSource != null)
		{
			audioSource.clip = this.click;
			this.PlaySource(audioSource, true);
		}
	}

    public void Nitro()
    {
        nitroSource = this.FreeAudioSourceInt(this.interfaceSources);
        if (nitroSource != null)
        {
            nitroSource.clip = this.nitro;
            this.PlaySource(nitroSource, false);
        }
    }

	public void StopNitro()
	{
        if(nitroSource != null)
			nitroSource.Stop();
    }

    public void PlayEndingTimeSound()
	{
		this.endingTimeSource.time = 0f;
		this.PlaySource(this.endingTimeSource, false);
	}

	public void StopEndingTimeSound()
	{
		this.StopSource(this.endingTimeSource);
	}

	public void PauseEndingTimeSound(bool inPause)
	{
		if (inPause)
		{
			this.endingTimeTime = this.endingTimeSource.time;
			this.StopSource(this.endingTimeSource);
		}
		else if (this.endingTimeTime != 0f)
		{
			this.endingTimeSource.time = this.endingTimeTime;
			this.PlaySource(this.endingTimeSource, false);
			this.endingTimeTime = 0f;
		}
	}

	private void InitInterfaceSounds()
	{
		this.interfaceSources = new AudioSource[3]; //20 objects
		for (int i = 0; i < this.interfaceSources.Length; i++)
		{
			this.interfaceSources[i] = base.gameObject.AddComponent<AudioSource>();
			this.interfaceSources[i].volume = 1f;
			this.interfaceSources[i].loop = false;
			this.interfaceSources[i].playOnAwake = false;
		}
		this.musicSource = base.gameObject.AddComponent<AudioSource>();
		this.musicSource.volume = 1f;
		this.musicSource.playOnAwake = false;
		this.musicSource.loop = true;
		this.jingleSource = base.gameObject.AddComponent<AudioSource>();
		this.jingleSource.volume = 1f;
		this.jingleSource.playOnAwake = false;
		this.jingleSource.loop = false;
		this.endingTimeSource = base.gameObject.AddComponent<AudioSource>();
		this.endingTimeSource.volume = 1f;
		this.endingTimeSource.playOnAwake = false;
		this.endingTimeSource.loop = false;
		this.endingTimeSource.clip = this.timeOutClip;
	}

	public void ToggleMusicOnOff()
	{
		this.musicOn = !this.musicOn;
		PlayerPrefs.SetInt("sm_mu", (!this.musicOn) ? 0 : 1);
		this.musicTgl.Enable = this.musicOn;
		this.UpdateMusicOnOff();
	}

	protected void UpdateMusicOnOff()
	{
		if (null == this.musicClip)
		{
			return;
		}
		IEnumerable<AudioSource> enumerable;
		if (Singleton<LevelManager>.Instance != null && this.useLevelRootToFindSources)
		{
			enumerable = Singleton<LevelManager>.Instance.gameObject.GetComponentsInChildren<AudioSource>(true);
		}
		else
		{
			Transform[] array = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));
			List<AudioSource> list = new List<AudioSource>();
			enumerable = list;
			foreach (Transform transform in array)
			{
				if (null == transform.parent)
				{
					AudioSource[] componentsInChildren = transform.gameObject.GetComponentsInChildren<AudioSource>(true);
					foreach (AudioSource item in componentsInChildren)
					{
						list.Add(item);
					}
				}
			}
		}
		foreach (AudioSource audioSource in enumerable)
		{
			if (audioSource.clip == this.musicClip)
			{
				audioSource.volume = this.musicVol;
				audioSource.mute = !this.musicOn;
			}
		}
	}

	public void ToggleSoundsOnOff()
	{
		this.soundsOn = !this.soundsOn;
		PlayerPrefs.SetInt("sm_sf", (!this.soundsOn) ? 0 : 1);
		this.soundTgl.Enable = this.soundsOn;
		IEnumerable<AudioSource> enumerable;
		if (Singleton<LevelManager>.Instance != null && this.useLevelRootToFindSources)
		{
			enumerable = Singleton<LevelManager>.Instance.gameObject.GetComponentsInChildren<AudioSource>(true);
		}
		else
		{
			Transform[] array = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));
			List<AudioSource> list = new List<AudioSource>();
			enumerable = list;
			foreach (Transform transform in array)
			{
				if (null == transform.parent)
				{
					AudioSource[] componentsInChildren = transform.gameObject.GetComponentsInChildren<AudioSource>(true);
					foreach (AudioSource item in componentsInChildren)
					{
						list.Add(item);
					}
				}
			}
		}
		foreach (AudioSource audioSource in enumerable)
		{
			if (null == audioSource.clip || audioSource.clip != this.musicClip)
			{
				audioSource.mute = !this.soundsOn;
			}
		}
	}

	public void PlayMusicSource(AudioSource source)
	{
		if (this.oldMusic != null)
		{
			this.oldMusic.Stop();
		}
		source.time = 0f;
		source.Play();
		this.oldMusic = source;
		this.lastMusicTime = 0f;
		if (Singleton<TimeManager>.Instance.MasterSource.IsPaused && this.pauseMusic)
		{
			source.Pause();
		}
		source.mute = !this.musicOn;
	}

	public void PauseMusicForced()
	{
		this.oldMusic.Pause();
		this.lastMusicTime = this.oldMusic.time;
		AudioSource[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip == this.musicClip)
			{
				if (this.pauseMusic)
				{
					audioSource.Pause();
				}
				this.lastMusicTime = audioSource.time;
			}
			else
			{
				if (this.wasPlaying.ContainsKey(audioSource))
				{
					this.timePlayed[audioSource] = audioSource.time;
					this.wasPlaying[audioSource] = audioSource.isPlaying;
				}
				else
				{
					this.timePlayed.Add(audioSource, audioSource.time);
					this.wasPlaying.Add(audioSource, audioSource.isPlaying);
				}
				audioSource.Pause();
			}
		}
	}

	public void ResumeForced()
	{
		this.OnResume();
	}

	public void StopMusicSource()
	{
		if (this.oldMusic != null)
		{
			this.lastMusicTime = this.oldMusic.time;
		}
		if (null == this.musicClip)
		{
			return;
		}
		IEnumerable<AudioSource> enumerable;
		if (Singleton<LevelManager>.Instance != null && this.useLevelRootToFindSources)
		{
			enumerable = Singleton<LevelManager>.Instance.gameObject.GetComponentsInChildren<AudioSource>(true);
		}
		else
		{
			Transform[] array = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));
			List<AudioSource> list = new List<AudioSource>();
			enumerable = list;
			foreach (Transform transform in array)
			{
				if (null == transform.parent)
				{
					AudioSource[] componentsInChildren = transform.gameObject.GetComponentsInChildren<AudioSource>(true);
					foreach (AudioSource item in componentsInChildren)
					{
						list.Add(item);
					}
				}
			}
		}
		foreach (AudioSource audioSource in enumerable)
		{
			if (audioSource.clip == this.musicClip)
			{
				audioSource.Stop();
			}
		}
		this.musicClip = null;
	}

	public void PlaySource(AudioSource source, bool bPlayInPause = false)
	{
		if (!Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			source.time = 0f;
			source.Play();
		}
		else if (bPlayInPause && this.soundsOn)
		{
			float timeScale = Time.timeScale;
			Time.timeScale = 1f;
			AudioSource.PlayClipAtPoint(source.clip, Camera.main.transform.position, 1f);
			Time.timeScale = timeScale;
		}
		if (!this.soundsOn)
		{
			source.mute = true;
		}
	}

	public void StopSource(AudioSource source)
	{
		if (this.wasPlaying.ContainsKey(source))
		{
			this.timePlayed[source] = source.time;
			this.wasPlaying[source] = source.isPlaying;
		}
		else
		{
			this.timePlayed.Add(source, source.time);
			this.wasPlaying.Add(source, source.isPlaying);
		}
	}

	public void UpdateDeviceMusicPlaying()
	{
		this.UpdateMusicOnOff();
	}

	private void OnPause()
	{
		if (this.oldMusic != null)
		{
			this.lastMusicTime = this.oldMusic.time;
		}
		AudioSource[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip == this.musicClip)
			{
				if (this.pauseMusic)
				{
					audioSource.Pause();
				}
			}
			else
			{
				if (this.wasPlaying.ContainsKey(audioSource))
				{
					this.timePlayed[audioSource] = audioSource.time;
					this.wasPlaying[audioSource] = audioSource.isPlaying;
				}
				else
				{
					this.timePlayed.Add(audioSource, audioSource.time);
					this.wasPlaying.Add(audioSource, audioSource.isPlaying);
				}
				audioSource.Pause();
			}
		}
	}

	private void OnResume()
	{
		AudioSource[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip == this.musicClip)
			{
				if (!audioSource.isPlaying)
				{
					audioSource.time = this.lastMusicTime;
					if (this.pauseMusic)
					{
						audioSource.Play();
					}
				}
			}
			else
			{
				bool flag = true;
				this.wasPlaying.TryGetValue(audioSource, out flag);
				if (flag)
				{
					audioSource.Play();
				}
			}
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused || Singleton<TimeManager>.Instance.MasterSource.IsPaused)
		{
			AudioSource[] array = UnityEngine.Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
			foreach (AudioSource audioSource in array)
			{
				if (audioSource.clip == this.musicClip)
				{
					if (this.pauseMusic)
					{
						audioSource.Pause();
					}
					this.lastMusicTime = audioSource.time;
				}
				else
				{
					audioSource.Pause();
				}
			}
		}
	}

	[Range(0f, 1f)]
	public float offGameBaseVolume = 1f;

	public AudioClip reachedMileStone;

	public AudioClip missionCompleted;

	public AudioClip pickupBackwash;

	public AudioClip pickupCoin;

	public AudioClip nitroStartClip;

	public AudioClip metersGood;

	public AudioClip backToNormal;

	public AudioClip musicMainMenu;

	public AudioClip click;

	public AudioClip timeOutClip;

	public AudioClip engineLoop;

	public AudioClip bang;

	public AudioClip nitro;

	internal AudioClip ingameMusic;

	private SoundManager.FxSoundSource[] fxSources;

	private PlayerSounds playerEngineSound;

	private AudioSource[] gameSoundsSources;

	private AudioSource[] interfaceSources;

	private int gameSourcesNumber = 2;//5

	private int fxSourcesNum = 2;

	private bool changeBgVolume;

	private bool isMusicDownActive;

	public bool musicOn = true;

	public bool soundsOn = true;

	private float fadeStartTime = -1f;

	private float startVolume;

	private float finalVolume;

	private float lastMusicTime;

	private float endingTimeTime;

	private float musicVol = 1f;

	private AudioSource musicSource;

	private AudioSource jingleSource;

	private AudioSource endingTimeSource;

	private AudioSource bgSource;

	private AudioSource bonusSource;

	private AudioSource metersSource;

	private AudioSource oldMusic;
	private AudioSource nitroSource;

	private Dictionary<AudioSource, bool> wasPlaying = new Dictionary<AudioSource, bool>();

	private Dictionary<AudioSource, float> timePlayed = new Dictionary<AudioSource, float>();

	[NonSerialized]
	public AudioClip musicClip;

	private bool pauseMusic = true;

	private bool useLevelRootToFindSources = true;

	public CustomToggle musicTgl;

	public CustomToggle soundTgl;

	public enum GeneralGameSounds
	{
		BackFromBoost,
		MissionCompleted
	}

	public class FxSoundSource
	{
		public FxSoundSource()
		{
			this.go = new GameObject();
			this.s = this.go.AddComponent<AudioSource>();
			this.s.loop = false;
			this.s.volume = 10f;
			this.s.minDistance = 25f;
			this.s.maxDistance = 150f;
			this.s.playOnAwake = false;
		}

		public GameObject go;

		public AudioSource s;
	}
}
