// dnSpy decompiler from Assembly-CSharp.dll class: RoadSheetBehaviour
using System;
using UnityEngine;

public class RoadSheetBehaviour : MonoBehaviour
{
	private void Update()
	{
		if (Singleton<GamePlay>.Instance.player.PlayerRigidbody.position.z - base.transform.position.z > 30f)
		{
			Singleton<ObjectsPool>.Instance.NotifyDestroyingParent(base.gameObject, ObjectsPool.ObjectType.RoadTimerSheet);
		}
	}

	private void ResetSheet(int lane)
	{
		base.gameObject.transform.position = new Vector3(0f, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
		base.gameObject.transform.position += new Vector3(9f * GameCore.lanes[lane], 0f, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Singleton<UIManager>.Instance.inGamePage.ShowHideOnTimerSheet(this.isPostive, true);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Singleton<UIManager>.Instance.inGamePage.ShowHideOnTimerSheet(this.isPostive, false);
		}
	}

	public bool isPostive;
}
