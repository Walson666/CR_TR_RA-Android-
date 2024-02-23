// dnSpy decompiler from Assembly-CSharp.dll class: EndlessBlockLink
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBlockLink : MonoBehaviour
{
	private void OnPopFromTrack()
	{
		this.branch = null;
	}

	public List<EndlessBlock> blackList;

	public List<EndlessBlock> whiteList;

	public Mark firstBranchToken;

	[NonSerialized]
	public EndlessTrackInternal.BlocksBranch branch;
}
