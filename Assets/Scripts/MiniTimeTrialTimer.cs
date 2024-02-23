// dnSpy decompiler from Assembly-CSharp.dll class: MiniTimeTrialTimer
using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniTimeTrialTimer : MonoBehaviour
{
	public float timer
	{
		get
		{
			return this._sec;
		}
		set
		{
			this._sec = value;
			if (Singleton<UIManager>.Instance.inGamePage.isPositiveTimer)
			{
				Singleton<GamePlay>.Instance.GamePlayTime += this._sec;
			}
			else
			{
				Singleton<GamePlay>.Instance.GamePlayTime -= this._sec;
			}
		}
	}

	public void OnEnable()
	{
		this.timer = 0f;
		this.img.color = ((!Singleton<UIManager>.Instance.inGamePage.isPositiveTimer) ? this.clrNeg : this.clrPos);
	}

	public void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
	}

	public Image img;

	private float _sec;

	public Color clrPos;

	public Color clrNeg;
}
