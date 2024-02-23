// dnSpy decompiler from Assembly-CSharp.dll class: ChangeSortingOrder
using System;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeSortingOrder : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Renderer>().sortingOrder = this.sortingOrder;
	}

	private void OnEnable()
	{
		this.Start();
	}

	public int sortingOrder = 8;
}
