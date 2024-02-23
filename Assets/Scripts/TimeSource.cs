// dnSpy decompiler from Assembly-CSharp.dll class: TimeSource
using System;
using UnityEngine;

public class TimeSource
{
	public float TotalTime
	{
		get
		{
			return this.totalTime;
		}
	}

	public float DeltaTime
	{
		get
		{
			return (!this.IsPaused) ? this.deltaTime : 0f;
		}
	}

	public float TimeMultiplier
	{
		get
		{
			return (!this.IsPaused) ? this.timeMultiplier : 0f;
		}
		set
		{
			this.timeMultiplier = Mathf.Max(0f, value);
			if (this.IsMaster)
			{
				Time.timeScale = this.timeMultiplier;
			}
		}
	}

	public bool IsPaused
	{
		get
		{
			return this.pauseCounter > 0;
		}
	}

	public bool IsMaster
	{
		get
		{
			return this == Singleton<TimeManager>.Instance.MasterSource;
		}
	}

	protected virtual void OnPause()
	{
	}

	protected virtual void OnResume()
	{
	}

	public void Pause()
	{
		if (this.pauseCounter == 0 && !this.lockPause)
		{
			this.lockPause = true;
			if (this.IsMaster)
			{
				Time.timeScale = 0f;
				Singleton<TimeManager>.Instance.OnGamePaused();
			}
			this.OnPause();
			this.lockPause = false;
		}
		this.pauseCounter++;
	}

	public void Resume()
	{
		this.pauseCounter--;
		if (this.pauseCounter == 0)
		{
			if (this.IsMaster)
			{
				Time.timeScale = this.timeMultiplier;
				Singleton<TimeManager>.Instance.OnGameResumed();
			}
			this.OnResume();
		}
	}

	public void Reset()
	{
		this.realTime = Time.realtimeSinceStartup;
		this.totalTime = 0f;
		this.deltaTime = 0f;
		this.timeMultiplier = 1f;
	}

	public void Update()
	{
		if (this.IsPaused)
		{
			return;
		}
		float num = this.realTime;
		this.realTime = Time.realtimeSinceStartup;
		this.deltaTime = Mathf.Min(this.maxDeltaTime, this.realTime - num) * this.timeMultiplier;
		this.totalTime += this.deltaTime;
	}

	protected float realTime;

	protected float totalTime;

	protected float deltaTime;

	protected float timeMultiplier = 1f;

	protected int pauseCounter;

	protected float maxDeltaTime = 0.0833333358f;

	protected bool lockPause;
}
