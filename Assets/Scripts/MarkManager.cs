// dnSpy decompiler from Assembly-CSharp.dll class: MarkManager
using System;
using System.Collections.Generic;
using UnityEngine;

public class MarkManager : Singleton<MarkManager>
{
	public Mark[] Tokens
	{
		get
		{
			return this.tokens;
		}
	}

	public bool GetToken(Vector3 worldPos, out MarkManager.TokenHit hitInfo)
	{
		return this.GetToken(worldPos, null, out hitInfo);
	}

	public bool GetToken(Vector3 worldPos, Mark startToken, out MarkManager.TokenHit hitInfo)
	{
		return this.GetToken(worldPos, startToken, null, out hitInfo);
	}

	public bool GetToken(Vector3 worldPos, Mark startToken, TrackBranch trackBranch, out MarkManager.TokenHit hitInfo)
	{
		List<MarkManager.TokenHit> list = this.GetTokens(worldPos, startToken, trackBranch, true, -1);
		if (list.Count > 0)
		{
			hitInfo = list[0];
			return true;
		}
		hitInfo = null;
		return false;
	}

	public List<MarkManager.TokenHit> GetTokens(Vector3 worldPos)
	{
		return this.GetTokens(worldPos, null);
	}

	public List<MarkManager.TokenHit> GetTokens(Vector3 worldPos, Mark startToken)
	{
		return this.GetTokens(worldPos, startToken, null, false, -1);
	}

	public List<MarkManager.TokenHit> GetTokens(Vector3 worldPos, Mark startToken, TrackBranch trackBranch, bool exitAtFirstHit, int branchMask)
	{
		if (null == startToken)
		{
			if (trackBranch == null)
			{
				LevelObject[] array = Singleton<LevelManager>.Instance.Query(new Bounds(worldPos, Vector3.zero), "tokens");
				if (array.Length > 0)
				{
					startToken = array[0].gameObject.GetComponent<Mark>();
				}
				else
				{
					array = Singleton<LevelManager>.Instance.Query("tokens");
					if (array.Length <= 0)
					{
						return new List<MarkManager.TokenHit>();
					}
					startToken = array[0].gameObject.GetComponent<Mark>();
				}
			}
			else
			{
				startToken = trackBranch[0];
			}
		}
		List<MarkManager.TokenHit> list = new List<MarkManager.TokenHit>();
		List<int> list2 = new List<int>();
		Queue<Mark> queue = new Queue<Mark>();
		queue.Enqueue(startToken);
		list2.Add(startToken.UniqueId);
		while (queue.Count > 0)
		{
			MarkManager.TokenHit tokenHit = new MarkManager.TokenHit();
			tokenHit.token = queue.Dequeue();
			tokenHit.token.WorldToToken(worldPos, out tokenHit.longitudinal, out tokenHit.trasversal);
			if (tokenHit.longitudinal >= 0f - this.longitudinalExt && tokenHit.longitudinal <= 1f + this.longitudinalExt && tokenHit.trasversal >= -1f - this.trasversalExt && tokenHit.trasversal <= 1f + this.trasversalExt && (tokenHit.token.TrackBranch == trackBranch || trackBranch == null) && (tokenHit.token.TrackBranch == null || (tokenHit.token.TrackBranch.mask & branchMask) != 0))
			{
				tokenHit.token.TokenToWorld(tokenHit.longitudinal, tokenHit.trasversal, out tokenHit.position, out tokenHit.tangent);
				list.Add(tokenHit);
				if (exitAtFirstHit)
				{
					break;
				}
			}
			foreach (Mark mark in tokenHit.token.nextLinks)
			{
				int num = list2.BinarySearch(mark.UniqueId);
				if (num < 0)
				{
					queue.Enqueue(mark);
					list2.Insert(~num, mark.UniqueId);
				}
			}
			foreach (Mark mark2 in tokenHit.token.prevLinks)
			{
				int num2 = list2.BinarySearch(mark2.UniqueId);
				if (num2 < 0)
				{
					queue.Enqueue(mark2);
					list2.Insert(~num2, mark2.UniqueId);
				}
			}
		}
		return list;
	}

	private void Awake()
	{
		this.tokens = (UnityEngine.Object.FindObjectsOfType(typeof(Mark)) as Mark[]);
	}

	public float longitudinalExt = MathC.Epsilon;

	public float trasversalExt = MathC.Epsilon;

	protected Mark[] tokens;

	public class TokenHit
	{
		public Mark token;

		public Vector3G position = default(Vector3G);

		public Vector3G tangent = default(Vector3G);

		public float longitudinal;

		public float trasversal;
	}
}
