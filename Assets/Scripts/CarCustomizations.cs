// dnSpy decompiler from Assembly-CSharp.dll class: CarCustomizations
using System;
using UnityEngine;

public class CarCustomizations : MonoBehaviour
{
	internal void ApplyCustomizations(CarID carId)
	{
		foreach(var p in paint)
		{
            p.material.SetColor("_Color", UIManager.GaragePage.carCustomize.carPreviews[(int)carId].carClrs[PlayerDataPersistant.Instance.GetPlayerData(carId).currClr]);
        }
		//this.paint.material.SetColor("_Color", UIManager.GaragePage.carCustomize.carPreviews[(int)carId].carClrs[PlayerDataPersistant.Instance.GetPlayerData(carId).currClr]);
		UnityEngine.Object.Instantiate<Item>(UIManager.GaragePage.carCustomize.carPreviews[(int)carId].rims[PlayerDataPersistant.Instance.GetPlayerData(carId).currRim], base.transform).AddPlayerRot();
		UnityEngine.Object.Instantiate<StickerSet>(UIManager.GaragePage.carCustomize.carPreviews[(int)carId].stickerSet[PlayerDataPersistant.Instance.GetPlayerData(carId).currSticker], base.transform);
		if (Singleton<GameCore>.Instance.gameMode == GameMode.Multi)
		{
			if (!base.GetComponentInParent<TimeTrackMulti>())
			{
				base.transform.parent.gameObject.AddComponent<TimeTrackMulti>();
			}
		}
		else
		{
			UnityEngine.Object.Destroy(base.GetComponentInParent<TimeTrackMulti>());
		}
	}

	internal void SetActive(bool v)
	{
		base.gameObject.SetActive(v);
	}

	public MeshRenderer[] paint;
}
