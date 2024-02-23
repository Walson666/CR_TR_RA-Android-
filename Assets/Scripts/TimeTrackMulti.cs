// dnSpy decompiler from Assembly-CSharp.dll class: TimeTrackMulti
using System;
using UnityEngine;

public class TimeTrackMulti : MonoBehaviour
{
	private void Update()
	{
		this.timer += Time.deltaTime;
		this.Seconds = this.timer % 60f;
	}

	private void OnEnable()
	{
		this.timer = 0f;
	}

	private float timer;

	internal float Seconds;
}
