// dnSpy decompiler from Assembly-CSharp.dll class: PlayGameCenterManager
using System;
using UnityEngine;

public class PlayGameCenterManager
{
	private PlayGameCenterManager()
	{
		this.Authenticate();
	}

	public static PlayGameCenterManager Instance
	{
		get
		{
			return PlayGameCenterManager._instance;
		}
	}

	public void UnlockAchievement(string achId, double progress = 100.0)
	{
		if (this.Authenticated)
		{
			Social.ReportProgress(achId, progress, delegate(bool success)
			{
			});
		}
	}

	public void Authenticate()
	{
		if (this.Authenticated || this.Authenticating)
		{
			UnityEngine.Debug.LogWarning("Ignoring repeated call to Authenticate().");
			return;
		}
		this.Authenticating = true;
	}

	public bool Authenticated
	{
		get
		{
			return Social.Active.localUser.authenticated;
		}
	}

	public void ShowLeaderboardUI()
	{
		if (!this.Authenticated)
		{
			this.Authenticate();
		}
	}

	public void ShowAchievementsUI()
	{
		if (this.Authenticated)
		{
			Social.ShowAchievementsUI();
		}
		else
		{
			this.Authenticate();
		}
	}

	public void ResetAchievements()
	{
	}

	public void PostToLeaderboard(long score, string leaderboardID)
	{
		if (this.Authenticated)
		{
			Social.ReportScore(score, leaderboardID, delegate(bool success)
			{
				UnityEngine.Debug.Log("Posted ? " + success);
			});
		}
	}

	private static PlayGameCenterManager _instance = new PlayGameCenterManager();

	internal bool Authenticating;

	private string authproMessage = "Signing In.";
}
