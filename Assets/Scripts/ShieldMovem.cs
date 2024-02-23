// dnSpy decompiler from Assembly-CSharp.dll class: ShieldMovem
using System;
using UnityEngine;

public class ShieldMovem : MonoBehaviour
{
	private void Start()
	{
		this.rend = base.GetComponent<Renderer>();
	}

	private void Update()
	{
		this.offset = Time.time * 0.8f;
		this.rend.material.SetTextureOffset("_MainTex", new Vector2(this.offset, 0f));
	}

	private const float scrollSpeed = 0.8f;

	private Renderer rend;

	private float offset;
}
