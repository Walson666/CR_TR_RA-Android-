// dnSpy decompiler from Assembly-CSharp.dll class: Track
using System;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
	public float Length
	{
		get
		{
			return this.maxLength;
		}
	}

	public float GetStartOffset(int index)
	{
		return this.startOffsets[index];
	}

	public float GetStartOffset()
	{
		return this.startOffsets[0];
	}

	public TrackBranch GetRootBranch(int index)
	{
		return this.rootBranches[index];
	}

	public TrackBranch GetRootBranch()
	{
		return this.rootBranches[0];
	}

	public BranchNode GetNodeFromBranch(TrackBranch branch)
	{
		return this.branchNodes[branch];
	}

	public Mark EvaluateAt(TrackBranch firstBranch, float trackPos, float trasversal, int mask, out Vector3G pos, out Vector3G tang)
	{
		trackPos += this.startOffsets[0];
		trackPos = ((trackPos >= 0f) ? ((trackPos <= this.maxLength) ? trackPos : (trackPos - this.maxLength)) : (trackPos + this.maxLength));
		Stack<TrackBranch> stack = new Stack<TrackBranch>();
		List<TrackBranch> list = new List<TrackBranch>();
		stack.Push(firstBranch);
		while (stack.Count > 0)
		{
			TrackBranch trackBranch = stack.Pop();
			list.Add(trackBranch);
			if (trackPos >= trackBranch.startLength && trackPos <= trackBranch.startLength + trackBranch.Length * trackBranch.LengthScale)
			{
				return trackBranch.EvaluateAt(trackPos, trasversal, out pos, out tang);
			}
			BranchNode branchNode = this.branchNodes[trackBranch];
			foreach (BranchNode branchNode2 in branchNode.PrevLinks)
			{
				if ((branchNode2.Branch.mask & mask) != 0 && !stack.Contains(branchNode2.Branch) && !list.Contains(branchNode2.Branch))
				{
					stack.Push(branchNode2.Branch);
				}
			}
			foreach (BranchNode branchNode3 in branchNode.NextLinks)
			{
				if ((branchNode3.Branch.mask & mask) != 0 && !stack.Contains(branchNode3.Branch) && !list.Contains(branchNode3.Branch))
				{
					stack.Push(branchNode3.Branch);
				}
			}
		}
		pos = Vector3G.zero;
		tang = Vector3G.zero;
		return null;
	}

	public void Build()
	{
		int num = this.startTransforms.Length;
		this.startTokens = new Mark[num];
		this.startOffsets = new float[num];
		this.rootBranches = new TrackBranch[num];
		MarkManager.TokenHit[] array = new MarkManager.TokenHit[num];
		for (int i = 0; i < num; i++)
		{
			Vector3G v = this.startTransforms[i].position;
			array[i] = new MarkManager.TokenHit();
			Singleton<MarkManager>.Instance.GetToken(v, out array[i]);
			TrackBranch trackBranch = array[i].token.TrackBranch;
			this.startTokens[i] = array[i].token;
			this.rootBranches[i] = trackBranch;
		}
		foreach (KeyValuePair<TrackBranch, BranchNode> keyValuePair in this.branchNodes)
		{
			keyValuePair.Value.Initialize();
		}
		Stack<TrackBranch> stack = new Stack<TrackBranch>();
		List<TrackBranch> list = new List<TrackBranch>();
		stack.Push(this.rootBranches[0]);
		list.Add(this.rootBranches[0]);
		this.rootBranches[0].startLength = 0f;
		while (stack.Count > 0)
		{
			TrackBranch trackBranch2 = stack.Pop();
			BranchNode branchNode = this.branchNodes[trackBranch2];
			float num2 = 0f;
			foreach (BranchNode branchNode2 in branchNode.NextLinks)
			{
				num2 = Mathf.Max(num2, branchNode2.Branch.Length);
			}
			foreach (BranchNode branchNode3 in branchNode.NextLinks)
			{
				if (!stack.Contains(branchNode3.Branch) && !list.Contains(branchNode3.Branch))
				{
					branchNode3.Branch.LengthScale = num2 / branchNode3.Branch.Length;
					branchNode3.Branch.startLength = Mathf.Max(branchNode3.Branch.startLength, trackBranch2.startLength + trackBranch2.Length);
					list.Add(trackBranch2);
					stack.Push(branchNode3.Branch);
				}
			}
		}
		for (int l = 0; l < num; l++)
		{
			TrackBranch trackBranch3 = array[l].token.TrackBranch;
			this.startOffsets[l] = (trackBranch3.startLength + array[l].longitudinal * array[l].token.Length) * trackBranch3.LengthScale;
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"startOffsets[",
				l,
				"] = ",
				this.startOffsets[l]
			}));
		}
		this.maxLength = 0f;
		foreach (KeyValuePair<TrackBranch, BranchNode> keyValuePair2 in this.branchNodes)
		{
			this.maxLength = Mathf.Max(this.maxLength, keyValuePair2.Key.startLength + keyValuePair2.Key.Length);
		}
		UnityEngine.Debug.Log("Track length: " + this.maxLength);
	}

	private void Awake()
	{
		BranchNode[] componentsInChildren = base.gameObject.GetComponentsInChildren<BranchNode>();
		foreach (BranchNode branchNode in componentsInChildren)
		{
			this.branchNodes.Add(branchNode.Branch, branchNode);
		}
	}

	public Transform[] startTransforms;

	protected Mark[] startTokens;

	protected float[] startOffsets;

	protected TrackBranch[] rootBranches;

	protected float maxLength;

	protected Dictionary<TrackBranch, BranchNode> branchNodes = new Dictionary<TrackBranch, BranchNode>();
}
