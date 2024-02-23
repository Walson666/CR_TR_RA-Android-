// dnSpy decompiler from Assembly-CSharp.dll class: SearchPlayer
using System;
using UnityEngine;
using UnityEngine.UI;

public class SearchPlayer : MonoBehaviour
{
	private void Awake()
	{
		this.dummyAvatar = Resources.LoadAll<Sprite>("dummyAvatar");
	}

	public Sprite RandomDummyAvatar
	{
		get
		{
			return this.dummyAvatar[UnityEngine.Random.Range(0, this.dummyAvatar.Length)];
		}
	}

	private void OnDisable()
	{
		this.globe.SetActive(false);
	}

	public void ChangeAvatar()
	{
		for (int i = 0; i < this.dummyImg.Length; i++)
		{
			this.dummyImg[i].sprite = this.dummyAvatar[UnityEngine.Random.Range(0, this.dummyAvatar.Length)];
		}
	}

	internal void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
		this.globe.SetActive(v);
	}

	public Image[] dummyImg;

	public GameObject globe;

	private Sprite[] dummyAvatar;
}
