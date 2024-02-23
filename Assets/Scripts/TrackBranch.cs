// dnSpy decompiler from Assembly-CSharp.dll class: TrackBranch
using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackBranch : IComparable<TrackBranch>
{
	public float Length
	{
		get
		{
			return this.length;
		}
	}

	public float LengthScale
	{
		get
		{
			return this.lenScale;
		}
		set
		{
			this.lenScale = value;
			this.invLenScale = 1f / this.lenScale;
		}
	}

	public int Count
	{
		get
		{
			return this.branchTokens.Count;
		}
	}

	public Mark this[int tokenIndex]
	{
		get
		{
			return this.branchTokens[tokenIndex].token;
		}
	}

	public void Clear()
	{
		this.length = 0f;
		this.branchTokens.Clear();
	}

	public void AddToken(Mark token)
	{
		token.LinkToTrack(this, this.branchTokens.Count);
		this.branchTokens.Add(new TrackBranch.BranchToken(token, this.length));
		this.length += token.Length;
	}

	public Mark EvaluateAt(float offset, float trasversal, out Vector3G pos, out Vector3G tang)
	{
		TrackBranch.BranchToken branchToken = new TrackBranch.BranchToken(null, (offset - this.startLength) * this.invLenScale);
		int num = this.branchTokens.BinarySearch(branchToken);
		num = ((num >= 0) ? num : (~num));
		TrackBranch.BranchToken branchToken2 = this.branchTokens[Mathf.Clamp(num - 1, 0, this.branchTokens.Count - 1)];
		float num2 = (branchToken.startLength - branchToken2.startLength) / branchToken2.token.Length;
		num2 = Mathf.Clamp01(num2);
		branchToken2.token.TokenToWorld(num2, trasversal, out pos, out tang);
		return branchToken2.token;
	}

	public float GetTrackPosition(Mark token, float longitudinal)
	{
		TrackBranch.BranchToken branchToken = this.branchTokens[token.TrackBranchIndex];
		return this.startLength + (branchToken.startLength + token.Length * longitudinal) * this.lenScale;
	}

	public int CompareTo(TrackBranch other)
	{
		return this.startLength.CompareTo(other.startLength);
	}

	public float startLength;

	public int mask = -1;

	protected float lenScale = 1f;

	protected float invLenScale = 1f;

	protected float length;

	protected List<TrackBranch.BranchToken> branchTokens = new List<TrackBranch.BranchToken>();

	protected class BranchToken : IComparable<TrackBranch.BranchToken>
	{
		private BranchToken()
		{
		}

		public BranchToken(Mark _token, float _startLength)
		{
			this.token = _token;
			this.startLength = _startLength;
		}

		public int CompareTo(TrackBranch.BranchToken other)
		{
			return this.startLength.CompareTo(other.startLength);
		}

		public Mark token;

		public float startLength;
	}
}
