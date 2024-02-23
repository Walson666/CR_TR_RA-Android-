// dnSpy decompiler from Assembly-CSharp.dll class: ChangeSortingOrderParticles
using System;
using UnityEngine;

public class ChangeSortingOrderParticles : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = this.NewSortingOrder;
	}

	public int NewSortingOrder;
}
