// dnSpy decompiler from Assembly-CSharp.dll class: TrackController
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackController : MonoBehaviour
{
	public EndlessTrack Track
	{
		get
		{
			if (this._track == null)
			{
				this._track = base.GetComponent<EndlessTrack>();
			}
			return this._track;
		}
	}

	public EndlessTrack.TrackID CurrentAmbient
	{
		get
		{
			return this.currentConfiguration.ambientIdList[this.currentAmbientIndex];
		}
	}

	private void Awake()
	{
		this.playerKinematics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		this.currentConfigurationIndex = 0;
		this.currentAmbientIndex = 0;
		this.currentConfiguration = this.Track.stageConfiguration[this.currentConfigurationIndex];
		this.currentAmbientLength = UnityEngine.Random.Range(this.currentConfiguration.minDurationInMeters, this.currentConfiguration.maxDurationInMeters);
		if (this.currentConfiguration.minSingleAmbientDurationInMeters != -1f && this.currentConfiguration.maxSingleAmbientDurationInMeters != -1f)
		{
			this.nextSingleAmbientLength = UnityEngine.Random.Range(this.currentConfiguration.minSingleAmbientDurationInMeters, this.currentConfiguration.maxSingleAmbientDurationInMeters);
		}
		Singleton<SoundManager>.Instance.ingameMusic = Singleton<GameCore>.Instance.CurrentWorldInfo.inGameMusic;
	}

	private void Update()
	{
		if (!Singleton<UIManager>.Instance.inGamePage.isActiveAndEnabled)
		{
			return;
		}
		if (this.playerKinematics == null)
		{
			this.playerKinematics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
		}
		if (this.currentAmbientLength > 0f && this.playerKinematics.Distance > this.currentAmbientLength)
		{
			this.UpdateAmbient();
		}
		else if (this.nextSingleAmbientLength > 0f && this.playerKinematics.Distance > this.nextSingleAmbientLength)
		{
			this.switchAmbient();
		}
	}

	private void switchAmbient()
	{
		this.currentAmbientIndex = UnityEngine.Random.Range(0, this.Track.stageConfiguration[this.currentConfigurationIndex].ambientIdList.Count);
		this.currentConfiguration = this.Track.stageConfiguration[this.currentConfigurationIndex];
		this.nextSingleAmbientLength += UnityEngine.Random.Range(this.currentConfiguration.minSingleAmbientDurationInMeters, this.currentConfiguration.maxSingleAmbientDurationInMeters);
		base.SendMessage("ChangeAmbient", this.CurrentAmbient);
	}

	private void UpdateAmbient()
	{
		if (this.currentAmbientLength > 0f)
		{
			this.currentConfigurationIndex = (this.currentConfigurationIndex + 1) % this.Track.stageConfiguration.Count;
			this.currentAmbientIndex = UnityEngine.Random.Range(0, this.Track.stageConfiguration[this.currentConfigurationIndex].ambientIdList.Count);
			this.currentConfiguration = this.Track.stageConfiguration[this.currentConfigurationIndex];
			if (this.currentConfiguration.minSingleAmbientDurationInMeters != -1f && this.currentConfiguration.maxSingleAmbientDurationInMeters != -1f)
			{
				this.nextSingleAmbientLength += UnityEngine.Random.Range(this.currentConfiguration.minSingleAmbientDurationInMeters, this.currentConfiguration.maxSingleAmbientDurationInMeters);
			}
			else
			{
				this.nextSingleAmbientLength = -1f;
			}
			float num = UnityEngine.Random.Range(this.currentConfiguration.minDurationInMeters, this.currentConfiguration.maxDurationInMeters);
			if (num > 0f)
			{
				this.currentAmbientLength += num;
			}
			else
			{
				this.currentAmbientLength = -1f;
			}
			base.SendMessage("ChangeAmbient", this.CurrentAmbient);
		}
	}

	private void OnStartGame()
	{
		this.currentConfigurationIndex = 0;
		this.currentAmbientIndex = 0;
		this.currentConfiguration = this.Track.stageConfiguration[this.currentConfigurationIndex];
		this.currentAmbientLength = UnityEngine.Random.Range(this.currentConfiguration.minDurationInMeters, this.currentConfiguration.maxDurationInMeters);
		this.nextSingleAmbientLength = -1f;
	}

	private void OnReset()
	{
		this.OnStartGame();
	}

	private void OnGameover()
	{
		this.OnStartGame();
	}

	private void OnChangePlayerCar()
	{
		this.playerKinematics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
	}

	protected PlayerMovement playerKinematics;

	protected TrackController.StageConfiguration currentConfiguration;

	protected GameObject trackGO;

	protected int currentConfigurationIndex;

	protected int currentAmbientIndex;

	protected float currentAmbientLength;

	protected float nextSingleAmbientLength;

	private EndlessTrack _track;

	[Serializable]
	public class StageConfiguration
	{
		public List<EndlessTrack.TrackID> ambientIdList;

		public float minDurationInMeters = 700f;

		public float maxDurationInMeters = 750f;

		public float minSingleAmbientDurationInMeters = -1f;

		public float maxSingleAmbientDurationInMeters = -1f;
	}
}
