// dnSpy decompiler from Assembly-CSharp.dll class: EndlessObject
using System;
using UnityEngine;

public class EndlessObject : MonoBehaviour
{
	private void OnMove()
	{
		base.gameObject.SendMessage("ObjectOnMoveBefore", SendMessageOptions.DontRequireReceiver);
		int activeLayer = base.transform.parent.GetComponent<EndlessBlock>().activeLayer;
		int num = 0;
		if (activeLayer >= 0)
		{
			num = 1 << activeLayer;
		}
		bool enabled = (this.layersMask & num) != 0;
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().enabled = enabled;
		}
		if (base.GetComponent<Collider>() != null)
		{
			base.GetComponent<Collider>().enabled = enabled;
		}
		foreach (Renderer renderer in base.gameObject.GetComponentsInChildren<Renderer>())
		{
			renderer.enabled = enabled;
		}
		foreach (Collider collider in base.gameObject.GetComponentsInChildren<Collider>())
		{
			collider.enabled = enabled;
		}
		base.gameObject.SendMessage("ObjectOnMoveAfter", SendMessageOptions.DontRequireReceiver);
	}

	public int layersMask;
}
