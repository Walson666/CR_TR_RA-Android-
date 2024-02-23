// dnSpy decompiler from Assembly-CSharp.dll class: EndlessBlock
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessBlock : MonoBehaviour
{
	public EndlessBlock.Layer[] Layers
	{
		get
		{
			return this.layers;
		}
	}

	protected void FillBranch(Mark startToken, TrackBranch branch, ref List<TrackBranch> newBranches)
	{
		Mark mark = startToken;
		while (mark != null && mark.nextLinks.Count > 0)
		{
			branch.AddToken(mark);
			int count = mark.nextLinks.Count;
			if (count == 1)
			{
				mark = mark.nextLinks[0];
			}
			else
			{
				for (int i = 0; i < count; i++)
				{
					TrackBranch trackBranch = new TrackBranch();
					trackBranch.startLength = branch.startLength + branch.Length;
					if (newBranches == null)
					{
						newBranches = new List<TrackBranch>();
					}
					newBranches.Add(trackBranch);
					this.FillBranch(mark.nextLinks[i], trackBranch, ref newBranches);
				}
				mark = null;
			}
		}
		if (mark != null)
		{
			branch.AddToken(mark);
		}
	}

	public virtual List<TrackBranch> AddToBranch(TrackBranch startBranch)
	{
		List<TrackBranch> list = null;
		this.FillBranch(this.firstToken, startBranch, ref list);
		if (list == null)
		{
			this.endTrackPositions[0] = startBranch.startLength + startBranch.Length;
		}
		else
		{
			for (int i = 0; i < this.endTrackPositions.Length; i++)
			{
				this.endTrackPositions[i] = startBranch.startLength + startBranch.Length + list[i].Length;
			}
		}
		return list;
	}

	public float GetEndTrackPosition(int nextLinkIndex)
	{
		return this.endTrackPositions[nextLinkIndex];
	}

	public float GetMaxEndTrackPosition()
	{
		float num = 0f;
		foreach (float b in this.endTrackPositions)
		{
			num = Mathf.Max(num, b);
		}
		return num;
	}

	public int GetDifficultyMin()
	{
		if (this.layers.Length == 0)
		{
			return 0;
		}
		int num = 100;
		foreach (EndlessBlock.Layer layer in this.layers)
		{
			num = Mathf.Min(num, layer.diffMin);
		}
		return num;
	}

	public int GetDifficultyMax()
	{
		if (this.layers.Length == 0)
		{
			return 100;
		}
		int num = 0;
		foreach (EndlessBlock.Layer layer in this.layers)
		{
			num = Mathf.Max(num, layer.diffMax);
		}
		return num;
	}

	public bool IsDifficultyInRange(int index, int difficulty)
	{
		return this.layers[index].diffMin <= difficulty && difficulty <= this.layers[index].diffMax;
	}

	public bool IsDifficultyInRange(int difficulty)
	{
		for (int i = 0; i < this.layers.Length; i++)
		{
			if (!this.layers[i].name.Contains("Relax"))
			{
				if (this.IsDifficultyInRange(i, difficulty))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		this.endTrackPositions = new float[this.links.Length - 1];
	}

	private void OnMove()
	{
		this.toDelete = false;
	}

	public EndlessBlockLink[] links;

	public Mark firstToken;

	public EndlessBlock.Frequency frequency;

	public EndlessBlock.CacheSize cacheSize;

	public bool excludeFromRelax;

	[NonSerialized]
	public bool used;

	[NonSerialized]
	public int id = -1;

	[NonSerialized]
	public bool toDelete;

	[NonSerialized]
	public int activeLayer = -1;

	protected float[] endTrackPositions;

	[SerializeField]
	protected EndlessBlock.Layer[] layers;

	public enum Frequency
	{
		Lowest,
		Low,
		Medium,
		High,
		Highest
	}

	public enum CacheSize
	{
		Small,
		Medium,
		Big
	}

	[Serializable]
	public class Layer
	{
		public string name = "Layer";

		public float perc;

		public int diffMin = 1;

		public int diffMax = 100;
	}
}
