// dnSpy decompiler from Assembly-CSharp.dll class: OffsetScroller
using System;
using UnityEngine;

public class OffsetScroller : MonoBehaviour
{
	private void Start()
	{
		this.savedOffset = base.GetComponent<Renderer>().sharedMaterial.GetTextureOffset(this.MainTex);
	}

	private void Update()
	{
		float y = Mathf.Repeat(Time.time * this.scrollSpeed, 1f);
		Vector2 value = new Vector2(this.savedOffset.x, y);
		base.GetComponent<Renderer>().sharedMaterial.SetTextureOffset(this.MainTex, value);
	}

	private void OnDisable()
	{
		base.GetComponent<Renderer>().sharedMaterial.SetTextureOffset(this.MainTex, this.savedOffset);
	}

	public float scrollSpeed;

	private Vector2 savedOffset;

	private string MainTex = "_MainTex";
}
