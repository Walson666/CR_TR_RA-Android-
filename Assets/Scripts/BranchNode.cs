// dnSpy decompiler from Assembly-CSharp.dll class: BranchNode
using System;
using UnityEngine;

public class BranchNode : MonoBehaviour
{
	public TrackBranch Branch
	{
		get
		{
			return this.branch;
		}
	}

	public bool NoThrough
	{
		get
		{
			return this.noThrough;
		}
	}

	public bool Loop
	{
		get
		{
			return this.loop;
		}
	}

	public BranchNode[] PrevLinks
	{
		get
		{
			return this.prevLinks;
		}
	}

	public BranchNode[] NextLinks
	{
		get
		{
			return this.nextLinks;
		}
	}

	public void Initialize()
	{
		Mark mark = this.branch[0];
		Mark mark2 = this.branch[this.branch.Count - 1];
		this.prevLinks = new BranchNode[mark.prevLinks.Count];
		this.nextLinks = new BranchNode[mark2.nextLinks.Count];
		int num = 0;
		foreach (Mark mark3 in mark.prevLinks)
		{
			this.prevLinks[num] = this.track.GetNodeFromBranch(mark3.TrackBranch);
			num++;
		}
		num = 0;
		foreach (Mark mark4 in mark2.nextLinks)
		{
			this.nextLinks[num] = this.track.GetNodeFromBranch(mark4.TrackBranch);
			if (this.track.GetRootBranch() == mark4.TrackBranch)
			{
				num++;
			}
			else
			{
				num++;
			}
		}
	}

	private void Awake()
	{
		this.track = base.transform.parent.GetComponent<Track>();
		this.branch.startLength = 0f;
		this.branch.mask = this.mask;
		Mark mark = this.startToken;
		while (mark != this.endToken)
		{
			this.branch.AddToken(mark);
			mark = mark.nextLinks[0];
		}
		this.branch.AddToken(mark);
		Mark mark2 = this.branch[0];
		Mark mark3 = this.branch[this.branch.Count - 1];
		this.noThrough = (0 == mark3.nextLinks.Count);
		bool flag = mark2.prevLinks.IndexOf(mark3) >= 0;
		bool flag2 = mark3.nextLinks.IndexOf(mark2) >= 0;
		this.loop = (flag && flag2);
	}

	public int mask = -1;

	public Mark startToken;

	public Mark endToken;

	protected Track track;

	protected TrackBranch branch = new TrackBranch();

	protected bool noThrough;

	protected bool loop;

	protected BranchNode[] prevLinks;

	protected BranchNode[] nextLinks;
}
