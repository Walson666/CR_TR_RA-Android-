// dnSpy decompiler from Assembly-CSharp.dll class: UILinkCarUpgradeItem
using System;
using UnityEngine;
using UnityEngine.UI;

public class UILinkCarUpgradeItem : MonoBehaviour
{
	private void Awake()
	{
		this.titleTxt.text = UICarUpgradeItem.playerData[base.transform.GetSiblingIndex()];
	}

	public Text titleTxt;

	public Image progressBar;
}
