// dnSpy decompiler from Assembly-CSharp.dll class: EndlessTrackInternal
using System;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTrackInternal : Singleton<EndlessTrackInternal>
{
	private static LinkedList<int> Shuffle(LinkedList<int> c)
	{
		int[] array = new int[c.Count];
		c.CopyTo(array, 0);
		byte[] array2 = new byte[array.Length];
		EndlessTrackInternal.rng.NextBytes(array2);
		Array.Sort<byte, int>(array2, array);
		return new LinkedList<int>(array);
	}

	public EndlessTrackInternal.BlocksBranch FirstBranch
	{
		get
		{
			return this.branches[0];
		}
	}

	protected int FindOpenBranchIndex(EndlessTrackInternal.BlocksBranch branch)
	{
		int count = this.openBranches.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.openBranches[i].branch == branch)
			{
				return i;
			}
		}
		return -1;
	}

	protected GameObject GetFreeBlock(int id)
	{
		List<GameObject> list = this.blocksCache[id];
		foreach (GameObject gameObject in list)
		{
			EndlessBlock component = gameObject.GetComponent<EndlessBlock>();
			if (!component.used)
			{
				component.used = true;
				gameObject.SetActive(true);
				return gameObject;
			}
		}
		UnityEngine.Debug.Log("New block #" + id);
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.blocks[id].gameObject);
		gameObject2.GetComponent<EndlessBlock>().used = true;
		list.Add(gameObject2);
		return gameObject2;
	}

	protected virtual List<TrackBranch> PlaceBlock(EndlessTrackInternal.OpenBranch openBranch, TrackBranch activeBranch, GameObject prevBlock, GameObject newBlock, int nextLinkIndex)
	{
		EndlessBlock component = newBlock.GetComponent<EndlessBlock>();
		EndlessBlock endlessBlock = (!(null == prevBlock)) ? prevBlock.GetComponent<EndlessBlock>() : null;
		float num = (!(null == endlessBlock)) ? endlessBlock.GetEndTrackPosition(nextLinkIndex) : 0f;
		EndlessBlockLink endlessBlockLink = (!(null == prevBlock)) ? endlessBlock.links[nextLinkIndex + 1] : null;
		if (endlessBlockLink != null)
		{
			Matrix4x4G m = endlessBlockLink.transform.localToWorldMatrix;
			Matrix4x4G matrix4x4G = Matrix4x4G.TRS(component.links[0].transform.localPosition, component.links[0].transform.localRotation, Vector3G.one);
			matrix4x4G.InvertFast();
			matrix4x4G.Prepend(m);
			newBlock.transform.position = matrix4x4G.position;
			newBlock.transform.rotation = Quaternion.identity;
		}
		else
		{
			newBlock.transform.position = Vector3.zero;
			newBlock.transform.rotation = Quaternion.identity;
		}
		bool flag = false;
		bool flag2 = false;
		if (this.relaxRailsLength > 0f || this.voidRailsLength > 0f)
		{
			flag = true;
			flag2 = (this.voidRailsLength > 0f);
		}
		if (component.Layers.Length > 0 && !flag)
		{
			int num2 = component.Layers.Length;
			int num3 = UnityEngine.Random.Range(0, num2);
			int difficultyValue = this.controller.GetDifficultyValue();
			int num4 = 0;
			while ((!component.IsDifficultyInRange((num3 + num4) % num2, difficultyValue) || component.Layers[(num3 + num4) % num2].name.Contains("Relax")) && num4 < num2)
			{
				num4++;
			}
			component.activeLayer = (num3 + num4) % num2;
		}
		else if (flag && !flag2)
		{
			int i = 0;
			int num5 = component.Layers.Length;
			while (i < num5)
			{
				if (component.Layers[i].name.Contains("Relax"))
				{
					break;
				}
				i++;
			}
			component.activeLayer = ((i >= num5) ? -1 : i);
		}
		else
		{
			component.activeLayer = -1;
		}
		newBlock.BroadcastMessage("OnMove", SendMessageOptions.DontRequireReceiver);
		List<TrackBranch> result = component.AddToBranch(activeBranch);
		float num6 = 0f;
		for (int j = component.links.Length - 1; j >= 1; j--)
		{
			float endTrackPosition = component.GetEndTrackPosition(j - 1);
			if (endTrackPosition > num6)
			{
				num6 = endTrackPosition;
			}
		}
		float num7 = num6 - num;
		if (flag)
		{
			if (flag2)
			{
				this.voidRailsLength -= num7;
				if (this.voidRailsLength < 0f)
				{
					this.voidRailsLength = -1f;
				}
			}
			else
			{
				this.relaxRailsLength -= num7;
				if (this.relaxRailsLength < 0f)
				{
					this.relaxRailsLength = -1f;
				}
			}
		}
		return result;
	}

	protected List<TrackBranch> PlaceBlock(EndlessTrackInternal.OpenBranch openBranch, TrackBranch activeBranch, GameObject prevBlock, GameObject newBlock)
	{
		return this.PlaceBlock(openBranch, activeBranch, prevBlock, newBlock, 0);
	}

	protected void PushBlock(int openBranchIndex, int nextLinkIndex)
	{
		int randomBlockId = this.GetRandomBlockId(openBranchIndex, nextLinkIndex);
		EndlessTrackInternal.OpenBranch openBranch = this.openBranches[openBranchIndex];
		EndlessTrackInternal.BlocksBranch branch = openBranch.branch;
		if (branch.head == null)
		{
			EndlessTrackInternal.BlockNode blockNode = branch.head = (branch.tail = new EndlessTrackInternal.BlockNode(this.GetFreeBlock(randomBlockId)));
			if (branch.prev == null)
			{
				this.PlaceBlock(openBranch, branch.trackBranch, null, blockNode.block);
			}
			else
			{
				EndlessTrackInternal.BlocksBranch prev = branch.prev;
				int i = 0;
				int num = prev.links.Length;
				while (i < num)
				{
					if (branch == prev.links[i])
					{
						break;
					}
					i++;
				}
				this.PlaceBlock(openBranch, branch.trackBranch, prev.tail.block, blockNode.block, i);
			}
			this.openBranches[openBranchIndex].lastBlock = branch.head.component;
			this.openBranches[openBranchIndex].lastBlockLinkIndex = 0;
			return;
		}
		EndlessTrackInternal.BlockNode tail = branch.tail;
		EndlessTrackInternal.BlockNode blockNode2 = tail.next = new EndlessTrackInternal.BlockNode(this.GetFreeBlock(randomBlockId));
		blockNode2.prev = tail;
		branch.tail = blockNode2;
		List<TrackBranch> list = this.PlaceBlock(openBranch, branch.trackBranch, tail.block, blockNode2.block);
		this.openBranches[openBranchIndex].lastBlock = blockNode2.component;
		this.openBranches[openBranchIndex].lastBlockLinkIndex = 0;
		if (this.blocks[randomBlockId].links.Length > 2)
		{
			int num2 = this.blocks[randomBlockId].links.Length - 1;
			EndlessBlock lastBlock = this.openBranches[openBranchIndex].lastBlock;
			this.openBranches.RemoveAt(openBranchIndex);
			branch.links = new EndlessTrackInternal.BlocksBranch[num2];
			for (int j = 0; j < num2; j++)
			{
				int k;
				for (k = 0; k < num2; k++)
				{
					if (lastBlock.links[k + 1].firstBranchToken == list[j][0])
					{
						lastBlock.links[k + 1].branch = (branch.links[k] = new EndlessTrackInternal.BlocksBranch());
						UnityEngine.Debug.Log(string.Concat(new object[]
						{
							"Link #",
							k,
							" got branch #",
							j
						}));
						break;
					}
				}
				branch.links[k].prev = branch;
				branch.links[k].trackBranch = list[j];
				this.openBranches.Add(new EndlessTrackInternal.OpenBranch(branch.links[k], lastBlock, k));
				this.branches.Add(branch.links[k]);
			}
		}
	}

	protected bool PopBlock(EndlessTrackInternal.BlocksBranch branch)
	{
		GameObject block = branch.head.block;
		block.GetComponent<EndlessBlock>().used = false;
		block.BroadcastMessage("OnPopFromTrack", SendMessageOptions.DontRequireReceiver);
		block.SetActive(false);
		branch.head.block = null;
		if (branch.head == branch.tail)
		{
			if (branch.links != null)
			{
				foreach (EndlessTrackInternal.BlocksBranch blocksBranch in branch.links)
				{
					blocksBranch.prev = null;
				}
			}
			this.branches.Remove(branch);
			int num = this.FindOpenBranchIndex(branch);
			if (num >= 0)
			{
				this.openBranches.RemoveAt(num);
			}
			return true;
		}
		branch.head = branch.head.next;
		branch.head.prev = null;
		return false;
	}

	protected void ClearUnlinkedBranches()
	{
		for (int i = this.branches.Count - 1; i >= 0; i--)
		{
			EndlessTrackInternal.BlocksBranch blocksBranch = this.branches[i];
			if (blocksBranch.head != null && this.controller.ActiveBranch != blocksBranch && this.controller.ActiveBlock != blocksBranch.tail)
			{
				if (blocksBranch.prev == null)
				{
					UnityEngine.Debug.Log("Clearing branch #" + i);
					while (!this.PopBlock(blocksBranch))
					{
					}
				}
			}
		}
	}

	protected virtual int GetRandomBlockId(int openBranchIndex, int nextLinkIndex)
	{
		int difficultyValue = this.controller.GetDifficultyValue();
		EndlessBlock lastBlock = this.openBranches[openBranchIndex].lastBlock;
		EndlessBlockLink endlessBlockLink = (!(null == lastBlock)) ? lastBlock.links[nextLinkIndex + 1] : null;
		LinkedListNode<int>[] array = new LinkedListNode<int>[this.randomSetSize];
		int num = 0;
		LinkedListNode<int> linkedListNode = this.blockIdsLRU.First;
		while (linkedListNode != null)
		{
			EndlessBlock endlessBlock = this.blocks[linkedListNode.Value];
			bool flag = this.relaxRailsLength > 0f || this.voidRailsLength > 0f;
			bool flag2 = flag || endlessBlock.IsDifficultyInRange(difficultyValue);
			if (flag2 && flag)
			{
				flag2 = !endlessBlock.excludeFromRelax;
			}
			if (flag2 && flag2 && lastBlock != null)
			{
				int count = endlessBlockLink.blackList.Count;
				for (int i = 0; i < count; i++)
				{
					if (linkedListNode.Value == endlessBlockLink.blackList[i].id)
					{
						flag2 = false;
						break;
					}
				}
				count = endlessBlockLink.whiteList.Count;
				if (count > 0)
				{
					flag2 = false;
					for (int i = 0; i < count; i++)
					{
						if (linkedListNode.Value == endlessBlockLink.whiteList[i].id)
						{
							flag2 = true;
							break;
						}
					}
				}
			}
			LinkedListNode<int> linkedListNode2 = linkedListNode;
			linkedListNode = linkedListNode.Next;
			if (flag2)
			{
				array[num++] = linkedListNode2;
				if (this.randomSetSize == num)
				{
					break;
				}
			}
		}
		LinkedListNode<int> linkedListNode3 = null;
		int num2 = UnityEngine.Random.Range(0, this.randomSetSize);
		int num3 = 0;
		while (linkedListNode3 == null && num3 < this.randomSetSize)
		{
			int num4 = (num2 + num3++) % this.randomSetSize;
			linkedListNode3 = array[num4];
		}
		this.blockIdsLRU.Remove(linkedListNode3);
		this.blockIdsLRU.AddLast(linkedListNode3);
		return linkedListNode3.Value;
	}

	private void OnStartGame()
	{
		this.gameStarted = true;
		this.branches = new List<EndlessTrackInternal.BlocksBranch>();
		this.branches.Add(new EndlessTrackInternal.BlocksBranch());
		this.branches[0].trackBranch = new TrackBranch();
		this.openBranches = new List<EndlessTrackInternal.OpenBranch>();
		this.openBranches.Add(new EndlessTrackInternal.OpenBranch(this.branches[0]));
		for (int i = 0; i < 2; i++)
		{
			this.PushBlock(0, 0);
		}
		this.nextEnvChange = (float)UnityEngine.Random.Range(29, 51) * 10f;
		this.startRunningTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		this.nextRelaxTime = this.startRunningTime + UnityEngine.Random.Range(this.relaxDeltaTimeMin, this.relaxDeltaTimeMax);
		this.controller.StartRunning(this.branches[0]);
	}

	private void OnTutorialEnd()
	{
		this.nextEnvChange = this.controller.TrackPos + (float)UnityEngine.Random.Range(29, 51) * 10f;
		this.startRunningTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime;
		this.nextRelaxTime = this.startRunningTime + UnityEngine.Random.Range(this.relaxDeltaTimeMin, this.relaxDeltaTimeMax);
	}

	private void OnGameover()
	{
		this.controller.EndRunning();
		for (int i = this.blocksCache.Length - 1; i >= 0; i--)
		{
			foreach (GameObject gameObject in this.blocksCache[i])
			{
				gameObject.GetComponent<EndlessBlock>().used = false;
				gameObject.BroadcastMessage("OnPopFromTrack", SendMessageOptions.DontRequireReceiver);
				gameObject.SetActive(false);
			}
		}
		this.nextRelaxTime = -1f;
		this.relaxRailsLength = -1f;
		this.voidRailsLength = -1f;
		this.gameStarted = false;
	}

	private void Start()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		this.blocksCache = new List<GameObject>[this.blocks.Length];
		foreach (EndlessBlock endlessBlock in this.blocks)
		{
			endlessBlock.id = num;
			switch (endlessBlock.frequency)
			{
			case EndlessBlock.Frequency.Lowest:
				num3 = 1;
				break;
			case EndlessBlock.Frequency.Low:
				num3 = 3;
				break;
			case EndlessBlock.Frequency.Medium:
				num3 = 5;
				break;
			case EndlessBlock.Frequency.High:
				num3 = 7;
				break;
			case EndlessBlock.Frequency.Highest:
				num3 = 9;
				break;
			}
			EndlessBlock.CacheSize cacheSize = endlessBlock.cacheSize;
			if (cacheSize != EndlessBlock.CacheSize.Small)
			{
				if (cacheSize != EndlessBlock.CacheSize.Medium)
				{
					if (cacheSize == EndlessBlock.CacheSize.Big)
					{
						num2 = 3;
					}
				}
				else
				{
					num2 = 2;
				}
			}
			else
			{
				num2 = 1;
			}
			for (int j = 0; j < num3; j++)
			{
				this.blockIdsLRU.AddLast(num);
			}
			List<GameObject> list = this.blocksCache[num++] = new List<GameObject>();
			for (int k = 0; k < num2; k++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(endlessBlock.gameObject);
				EndlessBlock component = gameObject.GetComponent<EndlessBlock>();
				component.used = false;
				component.id = endlessBlock.id;
				component.toDelete = false;
				list.Add(gameObject);
			}
			foreach (GameObject gameObject2 in list)
			{
				gameObject2.SetActive(false);
			}
		}
		this.blockIdsLRU = EndlessTrackInternal.Shuffle(this.blockIdsLRU);
	}

	private void Update()
	{
		if (!this.gameStarted)
		{
			return;
		}
		EndlessBlock component = this.FirstBranch.head.component;
		if (!component.toDelete && this.controller.TrackPos > component.GetMaxEndTrackPosition() + this.distForDeletion)
		{
			component.toDelete = true;
		}
		EndlessBlock component2 = this.FirstBranch.head.component;
		if (component2.toDelete)
		{
			this.PopBlock(this.FirstBranch);
		}
		this.ClearUnlinkedBranches();
		for (int i = this.openBranches.Count - 1; i >= 0; i--)
		{
			EndlessTrackInternal.OpenBranch openBranch = this.openBranches[i];
			EndlessBlock lastBlock = openBranch.lastBlock;
			if (lastBlock != null && this.controller.TrackPos > lastBlock.GetMaxEndTrackPosition() - this.minDistance)
			{
				this.PushBlock(i, openBranch.lastBlockLinkIndex);
			}
		}
	}

	private static System.Random rng = new System.Random();

	public EndlessBlock[] blocks;

	public int randomSetSize = 5;

	public EndlessPlayer controller;

	public float minDistance = 25f;

	public float distForDeletion = 10f;

	public float relaxDeltaTimeMin = 35f;

	public float relaxDeltaTimeMax = 45f;

	public float relaxLengthRatio = 3f;

	public float noRelaxAfterMeters = 2000f;

	protected bool gameStarted;

	protected List<GameObject>[] blocksCache;

	protected List<EndlessTrackInternal.BlocksBranch> branches;

	protected List<EndlessTrackInternal.OpenBranch> openBranches;

	protected LinkedList<int> blockIdsLRU = new LinkedList<int>();

	protected float nextEnvChange;

	protected float nextRelaxTime = -1f;

	protected float relaxRailsLength = -1f;

	protected float startRunningTime = -1f;

	protected float voidRailsLength = -1f;

	public class BlockNode
	{
		public BlockNode(GameObject _block)
		{
			this.block = _block;
			this.transform = _block.transform;
			this.component = _block.GetComponent<EndlessBlock>();
			this.objects = _block.GetComponentsInChildren<EndlessObject>();
		}

		public GameObject block;

		public EndlessTrackInternal.BlockNode prev;

		public EndlessTrackInternal.BlockNode next;

		public Transform transform;

		public EndlessBlock component;

		public EndlessObject[] objects;
	}

	public class BlocksBranch
	{
		public EndlessTrackInternal.BlockNode head;

		public EndlessTrackInternal.BlockNode tail;

		public EndlessTrackInternal.BlocksBranch prev;

		public EndlessTrackInternal.BlocksBranch[] links;

		public TrackBranch trackBranch;
	}

	public class OpenBranch
	{
		public OpenBranch(EndlessTrackInternal.BlocksBranch _branch)
		{
			this.branch = _branch;
		}

		public OpenBranch(EndlessTrackInternal.BlocksBranch _branch, EndlessBlock _lastBlock, int linkIndex)
		{
			this.branch = _branch;
			this.lastBlock = _lastBlock;
			this.lastBlockLinkIndex = linkIndex;
		}

		public EndlessTrackInternal.BlocksBranch branch;

		public EndlessBlock lastBlock;

		public int lastBlockLinkIndex;
	}
}
