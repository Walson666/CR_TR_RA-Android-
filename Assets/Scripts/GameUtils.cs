// dnSpy decompiler from Assembly-CSharp.dll class: GameUtils
using System;
using UnityEngine;

public static class GameUtils
{
	public static string GetValueFormated(int val)
	{
		return string.Format("{0:n0}", val);
	}

	public static string GetValueFormated(string val)
	{
		return string.Format("{0:n0}", val);
	}

	public static string MMSSFF(float time)
	{
		time = Mathf.Max(time, 0f);
		return string.Format("{0:00}:{1:00}:{2:00}", Mathf.FloorToInt(time / 60f), Mathf.FloorToInt(time % 60f), time * 10f % 10f);
	}

	public const float ToMetersPerSecs = 0.2777778f;
}
