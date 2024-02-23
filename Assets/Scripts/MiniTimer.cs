// dnSpy decompiler from Assembly-CSharp.dll class: MiniTimer
using System;
using UnityEngine;
using UnityEngine.UI;

public class MiniTimer : MonoBehaviour
{
	public float Seconds
	{
		get
		{
			return this._sec;
		}
		set
		{
			this._sec = value;
			this.timerText.text = string.Format("{0:0.00}", this._sec);
		}
	}

	private void Update()
	{
		this.timer += Time.deltaTime;
		this.Seconds = this.timer % 60f;
	}

	private void OnEnable()
	{
		this.timer = 0f;
	}

	private void OnDisable()
	{
		if (this.highSpeed)
		{
			Singleton<UIManager>.Instance.inGamePage.timeHighSpeed += this.Seconds;
		}
		else
		{
			Singleton<UIManager>.Instance.inGamePage.timeWrongLane += this.Seconds;
		}
	}

	public bool highSpeed;

	public Text timerText;

	private float timer;

	private float _sec;
}
