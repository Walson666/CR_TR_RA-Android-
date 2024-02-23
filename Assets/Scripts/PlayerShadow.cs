// dnSpy decompiler from Assembly-CSharp.dll class: PlayerShadow
using System;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
	private void Start()
	{
		this.baseOffset = base.gameObject.transform.localPosition;
	}

	private void Update()
	{
		float num = this.playerRef.transform.position.y - this.terrainHeight - this.shadowVerticalOffset;
		base.gameObject.transform.localPosition = new Vector3(this.baseOffset.x, -num, this.baseOffset.z);
		base.gameObject.transform.rotation = Quaternion.Euler(0f, this.playerRef.transform.rotation.eulerAngles.y, 0f);
	}

	private void ActivateShadow(bool active)
	{
		base.gameObject.SetActive(active);
	}

	private void OnEnable()
	{
		if (QualitySettings.GetQualityLevel() == 5)
		{
			base.gameObject.SetActive(false);
		}
	}

	public GameObject playerRef;

	public float shadowVerticalOffset = -0.5f;

	public float terrainHeight;

	public Vector3 baseOffset;

	public const string PLAYER_CAR = "PlayerCar";
}
