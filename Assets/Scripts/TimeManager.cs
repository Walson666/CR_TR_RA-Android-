// dnSpy decompiler from Assembly-CSharp.dll class: TimeManager
using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	public TimeSource MasterSource
	{
		get
		{
			return this.masterSource;
		}
		set
		{
			if (this.masterSource != null && value != null)
			{
				if (this.masterSource.IsPaused && !value.IsPaused)
				{
					this.OnGamePaused();
				}
				else if (!this.masterSource.IsPaused && value.IsPaused)
				{
					this.OnGameResumed();
				}
			}
			this.masterSource = value;
		}
	}

	private void Awake()
	{
		this.appIsPaused = false;
		this.appIsFocused = true;
		this.masterSource = new TimeSource();
		this.sources = new List<TimeSource>();
		this.sources.Add(this.masterSource);
	}

	private void OnApplicationPause(bool pause)
	{
		bool flag = this.appIsPaused;
		this.appIsPaused = pause;
		if (!flag && pause && this.masterSource != null)
		{
			this.masterSource.Pause();
		}
		if (flag && !pause && this.masterSource != null)
		{
			this.masterSource.Resume();
		}
	}

	private void OnApplicationFocus(bool focus)
	{
		if (Application.runInBackground || !this.manageFocus || Application.platform == RuntimePlatform.Android)
		{
			return;
		}
		bool flag = this.appIsFocused;
		this.appIsFocused = focus;
		if (flag && !focus && this.masterSource != null)
		{
			this.masterSource.Pause();
		}
		if (!flag && focus && this.masterSource != null)
		{
			this.masterSource.Resume();
		}
	}

	private void Update()
	{
		foreach (TimeSource timeSource in this.sources)
		{
			timeSource.Update();
		}
	}

	public void AddSource(TimeSource source)
	{
		this.sources.Add(source);
	}

	public void RemoveSource(TimeSource source)
	{
		this.sources.Remove(source);
	}

	public void OnGamePaused()
	{
		if (Singleton<LevelManager>.Instance != null)
		{
			Singleton<LevelManager>.Instance.BroadcastMessage("OnPause", SendMessageOptions.DontRequireReceiver);
		}
	}

	public void OnGameResumed()
	{
		if (Singleton<LevelManager>.Instance != null)
		{
			Singleton<LevelManager>.Instance.BroadcastMessage("OnResume", SendMessageOptions.DontRequireReceiver);
		}
	}

	[HideInInspector]
	public bool manageFocus = true;

	protected bool appIsPaused;

	protected bool appIsFocused;

	protected TimeSource masterSource;

	protected List<TimeSource> sources;
}
