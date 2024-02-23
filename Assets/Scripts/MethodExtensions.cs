// dnSpy decompiler from Assembly-CSharp.dll class: MethodExtensions
using System;
using UnityEngine;

public static class MethodExtensions
{
	public static void SetActiveRecursivelyL(this GameObject go, bool active)
	{
		go.SetActive(active);
	}

	public static T GetOrAddComponent<T>(this Component child) where T : Component
	{
		T t = child.GetComponent<T>();
		if (t == null)
		{
			t = child.gameObject.AddComponent<T>();
		}
		return t;
	}
}
