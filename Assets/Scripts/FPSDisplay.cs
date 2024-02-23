// dnSpy decompiler from Assembly-CSharp.dll class: FPSDisplay
using System;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
	private void Update()
	{
		this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
		float num = this.deltaTime * 1000f;
		float num2 = 1f / this.deltaTime;
		this.fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", num, num2);
	}

	private float deltaTime;

	public Text fpsText;
}
