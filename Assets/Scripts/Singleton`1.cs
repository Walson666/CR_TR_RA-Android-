// dnSpy decompiler from Assembly-CSharp.dll class: Singleton`1
using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static T Instance
	{
		get
		{
			if (Singleton<T>.applicationIsQuitting)
			{
				UnityEngine.Debug.LogWarningFormat("[Singleton] Instance '{0}' already destroyed on application quit.Won't create again- returning null.", new object[]
				{
					typeof(T)
				});
				return (T)((object)null);
			}
			object @lock = Singleton<T>._lock;
			T instance;
			lock (@lock)
			{
				if (Singleton<T>._instance == null)
				{
					Singleton<T>._instance = (T)((object)UnityEngine.Object.FindObjectOfType(typeof(T)));
					if (UnityEngine.Object.FindObjectsOfType(typeof(T)).Length > 1)
					{
						return Singleton<T>._instance;
					}
					if (Singleton<T>._instance == null)
					{
						GameObject gameObject = new GameObject();
						Singleton<T>._instance = gameObject.AddComponent<T>();
						gameObject.name = "(singleton) " + typeof(T).ToString();
					}
				}
				instance = Singleton<T>._instance;
			}
			return instance;
		}
	}

	public void OnDestroy()
	{
		Singleton<T>.applicationIsQuitting = true;
		Singleton<T>._instance = (T)((object)null);
	}

	private static T _instance;

	private static object _lock = new object();

	private static bool applicationIsQuitting = false;
}
