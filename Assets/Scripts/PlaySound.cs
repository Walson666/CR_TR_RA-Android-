// dnSpy decompiler from Assembly-CSharp.dll class: PlaySound
using System;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
	private void OnEnable()
	{
		if (Singleton<SoundManager>.Instance.soundsOn)
		{
			base.GetComponent<AudioSource>().Play();
		}
	}
}
