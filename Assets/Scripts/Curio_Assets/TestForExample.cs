// dnSpy decompiler from Assembly-CSharp.dll class: Curio_Assets.TestForExample
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Curio_Assets
{
	public class TestForExample : MonoBehaviour
	{
		private void Start()
		{
			this.btns[0].interactable = true;
			for (int i = 1; i < this.btns.Length; i++)
			{
				this.btns[i].interactable = false;
			}
		}

		private void Update()
		{
			if (this.done)
			{
				return;
			}
			if (PlayGameCenterManager.Instance.Authenticated)
			{
				this.EnableButton();
			}
		}

		private void EnableButton()
		{
			this.btns[0].interactable = false;
			for (int i = 1; i < this.btns.Length; i++)
			{
				this.btns[i].interactable = true;
			}
			this.done = true;
		}

		public void Authenticate()
		{
			PlayGameCenterManager.Instance.Authenticate();
		}

		public void ShowLeaderboard()
		{
			PlayGameCenterManager.Instance.ShowLeaderboardUI();
		}

		public void ShowAchievementsUI()
		{
			PlayGameCenterManager.Instance.ShowAchievementsUI();
		}

		public void ResetAch()
		{
			PlayGameCenterManager.Instance.ResetAchievements();
		}

		public Button[] btns;

		private bool done;
	}
}
