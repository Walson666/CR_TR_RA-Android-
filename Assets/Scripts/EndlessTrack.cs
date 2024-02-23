// dnSpy decompiler from Assembly-CSharp.dll class: EndlessTrack
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrackController))]
public class EndlessTrack : Singleton<EndlessTrack>
{
	private static LinkedList<int> Shuffle(LinkedList<int> c)
	{
		int[] array = new int[c.Count];
		c.CopyTo(array, 0);
		byte[] array2 = new byte[array.Length];
		EndlessTrack.rng.NextBytes(array2);
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

	protected GameObject CreateInstanceForCache(EndlessBlock block, EndlessTrack.BlockData data)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(block.gameObject);
		EndlessBlock component = gameObject.GetComponent<EndlessBlock>();
		component.used = false;
		component.id = block.id;
		component.toDelete = false;
		int lightmapserializedObjectffset = data.lightmapserializedObjectffset;
		Renderer[] componentsInChildren = component.GetComponentsInChildren<Renderer>(true);
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.lightmapIndex < 254)
			{
				renderer.lightmapIndex += lightmapserializedObjectffset;
			}
		}
		this.InitializeBlock(gameObject);
		gameObject.SetActive(false);
		return gameObject;
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

	protected GameObject GetFreeBlock(int id, bool updateFirstAmbientBlock = true)
	{
		bool flag = false;
		GameObject gameObject = null;
		if (this.ambientJustChanged)
		{
			flag = true;
			this.ambientJustChanged = false;
			gameObject = this.lastblock;
			this.spawnFirstAmbientBlock = true;
			this.firstblock = this.GetAmbientFirstBlock(id, false);
			if (this.firstblock != null)
			{
				this.firstblock.SetActive(false);
			}
			if (this.lastblock == null)
			{
				this.spawnFirstAmbientBlock = false;
				if (this.firstblock != null)
				{
					gameObject = this.firstblock;
				}
				else
				{
					flag = false;
				}
			}
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				gameObject.GetComponent<EndlessBlock>().used = true;
				this.lastblock = null;
			}
		}
		else if (this.spawnFirstAmbientBlock && this.firstblock != null)
		{
			flag = true;
			this.spawnFirstAmbientBlock = false;
			gameObject = this.firstblock;
			gameObject.SetActive(true);
			gameObject.GetComponent<EndlessBlock>().used = true;
			this.firstblock = null;
		}
		if (!flag)
		{
			if (updateFirstAmbientBlock)
			{
				this.spawnFirstAmbientBlock = false;
			}
			List<GameObject> list = this.blocksCache[id];
			foreach (GameObject gameObject2 in list)
			{
				EndlessBlock component = gameObject2.GetComponent<EndlessBlock>();
				if (!component.used)
				{
					component.used = true;
					gameObject2.SetActive(true);
					return gameObject2;
				}
			}
			gameObject = this.CreateInstanceForCache(this.blocks[id], this.blocksData[id]);
			gameObject.GetComponent<EndlessBlock>().used = true;
			gameObject.transform.position = Vector3.zero;
			gameObject.SetActive(true);
			list.Add(gameObject);
			return gameObject;
		}
		return gameObject;
	}

	protected GameObject GetAmbientLastBlock()
	{
		GameObject gameObject = null;
		if (this.lastAmbientBlocksCache[(int)this.worldBlockID] == null)
		{
			EndlessBlock endBlock = this.worldBlockList[(int)this.worldBlockID].endBlock;
			if (endBlock != null)
			{
				UnityEngine.Debug.LogErrorFormat("Last Block {0}", new object[]
				{
					endBlock.name
				});
				gameObject = this.CreateInstanceForCache(endBlock, this.lastAmbientBlocksData[(int)this.worldBlockID]);
				this.lastAmbientBlocksCache[(int)this.worldBlockID] = gameObject;
				gameObject.GetComponent<EndlessBlock>().used = true;
			}
		}
		else
		{
			GameObject gameObject2 = this.lastAmbientBlocksCache[(int)this.worldBlockID];
			if (!gameObject2.GetComponent<EndlessBlock>().used)
			{
				gameObject = gameObject2;
				gameObject.GetComponent<EndlessBlock>().used = true;
			}
			else
			{
				UnityEngine.Debug.Log("Loaded Last Block " + this.worldBlockID);
			}
		}
		return gameObject;
	}

	protected GameObject GetAmbientFirstBlock(int id, bool startGame = false)
	{
		GameObject gameObject = null;
		if (this.firstAmbientBlocksCache[(int)this.worldBlockID] == null)
		{
			EndlessBlock startBlock = this.worldBlockList[(int)this.worldBlockID].startBlock;
			if (startBlock != null)
			{
				gameObject = this.CreateInstanceForCache(startBlock, this.firstAmbientBlocksData[(int)this.worldBlockID]);
				gameObject.GetComponent<EndlessBlock>().used = true;
				this.firstAmbientBlocksCache[(int)this.worldBlockID] = gameObject;
			}
		}
		else
		{
			GameObject gameObject2 = this.firstAmbientBlocksCache[(int)this.worldBlockID];
			if (!gameObject2.GetComponent<EndlessBlock>().used)
			{
				gameObject = gameObject2;
				gameObject.GetComponent<EndlessBlock>().used = true;
			}
		}
		if (gameObject == null && startGame)
		{
			gameObject = this.GetFreeBlock(id, false);
		}
		else if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		return gameObject;
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
			EndlessTrackInternal.BlockNode blockNode = branch.head = (branch.tail = new EndlessTrackInternal.BlockNode(this.GetAmbientFirstBlock(randomBlockId, true)));
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
		EndlessTrackInternal.BlockNode blockNode2 = tail.next = new EndlessTrackInternal.BlockNode(this.GetFreeBlock(randomBlockId, true));
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
			if (linkedListNode.Value < this.ambientsBlockListIndexes[(int)this.worldBlockID].minIndex || linkedListNode.Value > this.ambientsBlockListIndexes[(int)this.worldBlockID].maxIndex)
			{
				flag2 = false;
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
		this.worldBlockID = base.GetComponent<TrackController>().CurrentAmbient;
		this.SetupAmbientIndex();
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
		for (int j = 0; j < this.firstAmbientBlocksCache.Count; j++)
		{
			foreach (GameObject gameObject2 in this.firstAmbientBlocksCache)
			{
				if (gameObject2 != null)
				{
					gameObject2.GetComponent<EndlessBlock>().used = false;
					gameObject2.BroadcastMessage("OnPopFromTrack", SendMessageOptions.DontRequireReceiver);
					gameObject2.SetActive(false);
				}
			}
		}
		for (int k = 0; k < this.lastAmbientBlocksCache.Count; k++)
		{
			foreach (GameObject gameObject3 in this.lastAmbientBlocksCache)
			{
				if (gameObject3 != null)
				{
					gameObject3.GetComponent<EndlessBlock>().used = false;
					gameObject3.BroadcastMessage("OnPopFromTrack", SendMessageOptions.DontRequireReceiver);
					gameObject3.SetActive(false);
				}
			}
		}
		this.nextRelaxTime = -1f;
		this.relaxRailsLength = -1f;
		this.voidRailsLength = -1f;
		this.worldBlockID = EndlessTrack.TrackID.WORLD_CHUNK_1;
		this.ambientJustChanged = false;
		this.gameStarted = false;
	}

	private void ChangeAmbient(EndlessTrack.TrackID nextAmbientIdx)
	{
		this.lastblock = this.GetAmbientLastBlock();
		this.ambientJustChanged = true;
		this.worldBlockID = nextAmbientIdx;
		this.SetupAmbientIndex();
	}

	private void SetupAmbientIndex()
	{
		if (this.worldBlockID > EndlessTrack.TrackID.WORLD_CHUNK_7)
		{
			this.worldBlockID -= 12;
		}
		if (this.worldBlockID > EndlessTrack.TrackID.WORLD_CHUNK_6)
		{
			this.worldBlockID -= 8;
		}
		else if (this.worldBlockID > EndlessTrack.TrackID.WORLD_CHUNK_4)
		{
			this.worldBlockID -= 4;
		}
	}

	public void DestroyAllFromScene()
	{
		for (int i = this.blocksCache.Length - 1; i >= 0; i--)
		{
			foreach (GameObject obj in this.blocksCache[i])
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.blocksCache[i].Clear();
			this.blocksCache[i] = null;
		}
		for (int j = 0; j < this.firstAmbientBlocksCache.Count; j++)
		{
			GameObject gameObject = this.firstAmbientBlocksCache[j];
			this.firstAmbientBlocksCache[j] = null;
			if (gameObject != null)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.firstAmbientBlocksCache.Clear();
		for (int k = 0; k < this.lastAmbientBlocksCache.Count; k++)
		{
			GameObject gameObject2 = this.lastAmbientBlocksCache[k];
			this.lastAmbientBlocksCache[k] = null;
			if (gameObject2 != null)
			{
				UnityEngine.Object.Destroy(gameObject2);
			}
		}
		this.lastAmbientBlocksCache.Clear();
	}

	private void Awake()
	{
		this.CopyValuesFromAssetFile();
		if (base.GetComponent<TrackController>() == null)
		{
			base.gameObject.AddComponent<TrackController>();
		}
		this.controller = GameObject.FindGameObjectWithTag("Player").GetComponent<EndlessPlayer>();
		this.blocks = new List<EndlessBlock>();
		this.blocksData = new List<EndlessTrack.BlockData>();
		this.ambientsBlockListIndexes = new List<EndlessTrack.MinMaxIndexes>();
		this.lastAmbientBlocksCache = new List<GameObject>();
		this.lastAmbientBlocksData = new List<EndlessTrack.BlockData>();
		this.firstAmbientBlocksCache = new List<GameObject>();
		this.firstAmbientBlocksData = new List<EndlessTrack.BlockData>();
		int lmOffset = 0;
		int num = 0;
		for (int i = 0; i < this.worldBlockList.Count; i++)
		{
			this.ambientsBlockListIndexes.Add(new EndlessTrack.MinMaxIndexes());
			this.lastAmbientBlocksCache.Add(null);
			this.firstAmbientBlocksCache.Add(null);
			EndlessTrack.TrackData trackData = this.worldBlockList[i];
			this.lastAmbientBlocksData.Add(new EndlessTrack.BlockData(trackData.trackStartID, lmOffset));
			this.firstAmbientBlocksData.Add(new EndlessTrack.BlockData(trackData.trackStartID, lmOffset));
			foreach (EndlessBlock item in trackData.blocks)
			{
				this.blocks.Add(item);
				this.blocksData.Add(new EndlessTrack.BlockData(trackData.trackStartID, lmOffset));
			}
			this.ambientsBlockListIndexes[i].minIndex = num;
			this.ambientsBlockListIndexes[i].maxIndex = num + trackData.blocks.Length - 1;
			num = this.ambientsBlockListIndexes[i].maxIndex + 1;
		}
		this.initCache();
		RenderSettings.skybox = Singleton<GameCore>.Instance.CurrentWorldInfo.skyboxMat;
		DynamicGI.UpdateEnvironment();
	}

	private void CopyValuesFromAssetFile()
	{
		Configurations.TrackData trackInfo = Singleton<GameCore>.Instance.CurrentWorldInfo.trackInfo;
		this.worldBlockList = trackInfo.trackDataList;
		this.worldBlockID = trackInfo.worldBlockID;
		if (trackInfo.trackDataList[0].blocks[0] == null)
		{
			trackInfo.trackDataList[0].blocks[0] = Singleton<GameCore>.Instance.defaultEndlessBlock;
		}
		this.randomSetSize = trackInfo.trackDataList.Count;
		this.minDistance = 250f;
		this.distForDeletion = 10f;
		this.relaxDeltaTimeMin = 50f;
		this.relaxDeltaTimeMax = 105f;
		this.relaxLengthRatio = 3f;
		this.noRelaxAfterMeters = 0f;
		this.stageConfiguration = trackInfo.stageConfiguration;
	}

	private void InitializeBlock(GameObject block)
	{
	}

	private void initCache()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < this.worldBlockList.Count; i++)
		{
			num4 += this.worldBlockList[i].blocks.Length;
		}
		this.blocksCache = new List<GameObject>[num4];
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
				list.Add(this.CreateInstanceForCache(endlessBlock, this.blocksData[endlessBlock.id]));
			}
		}
		this.blockIdsLRU = EndlessTrack.Shuffle(this.blockIdsLRU);
		for (int l = 0; l < this.worldBlockList.Count; l++)
		{
			EndlessBlock endBlock = this.worldBlockList[l].endBlock;
			if (endBlock != null)
			{
				GameObject value = this.CreateInstanceForCache(endBlock, this.lastAmbientBlocksData[l]);
				this.lastAmbientBlocksCache[l] = value;
			}
			EndlessBlock startBlock = this.worldBlockList[l].startBlock;
			if (startBlock != null)
			{
				GameObject value = this.CreateInstanceForCache(startBlock, this.firstAmbientBlocksData[l]);
				this.firstAmbientBlocksCache[l] = value;
			}
		}
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
		if (this.controller.SessionMeters < this.noRelaxAfterMeters && this.nextRelaxTime > 0f && Singleton<TimeManager>.Instance.MasterSource.TotalTime >= this.nextRelaxTime && this.voidRailsLength < 0f)
		{
			this.relaxRailsLength = 100f;
			this.nextRelaxTime = Singleton<TimeManager>.Instance.MasterSource.TotalTime + UnityEngine.Random.Range(this.relaxDeltaTimeMin, this.relaxDeltaTimeMax);
		}
	}

	private static System.Random rng = new System.Random();

	[HideInInspector]
	public List<EndlessTrack.TrackData> worldBlockList;

	[HideInInspector]
	public EndlessTrack.TrackID worldBlockID;

	[HideInInspector]
	public int randomSetSize = 5;

	[HideInInspector]
	public EndlessPlayer controller;

	[HideInInspector]
	public float minDistance = 25f;

	[HideInInspector]
	public float distForDeletion = 10f;

	[HideInInspector]
	public float relaxDeltaTimeMin = 35f;

	[HideInInspector]
	public float relaxDeltaTimeMax = 45f;

	[HideInInspector]
	public float relaxLengthRatio = 3f;

	[HideInInspector]
	public float noRelaxAfterMeters = 2000f;

	[HideInInspector]
	public List<TrackController.StageConfiguration> stageConfiguration;

	protected List<EndlessBlock> blocks;

	protected List<EndlessTrack.BlockData> blocksData;

	protected bool gameStarted;

	protected GameObject firstblock;

	protected GameObject lastblock;

	protected List<GameObject>[] blocksCache;

	protected List<GameObject> firstAmbientBlocksCache;

	protected List<EndlessTrack.BlockData> firstAmbientBlocksData;

	protected List<GameObject> lastAmbientBlocksCache;

	protected List<EndlessTrack.BlockData> lastAmbientBlocksData;

	protected List<EndlessTrackInternal.BlocksBranch> branches;

	protected bool ambientJustChanged;

	protected bool spawnFirstAmbientBlock;

	protected List<EndlessTrack.MinMaxIndexes> ambientsBlockListIndexes;

	protected List<EndlessTrackInternal.OpenBranch> openBranches;

	protected LinkedList<int> blockIdsLRU = new LinkedList<int>();

	protected float nextEnvChange;

	protected float nextRelaxTime = -1f;

	protected float relaxRailsLength = -1f;

	protected float startRunningTime = -1f;

	protected float voidRailsLength = -1f;

	public enum TrackID
	{
		WORLD_CHUNK_1,
		WORLD_CHUNK_2,
		WORLD_CHUNK_3,
		WORLD_CHUNK_4,
		WORLD_CHUNK_5,
		WORLD_CHUNK_6,
		WORLD_CHUNK_7
	}

	[Serializable]
	public class TrackData
	{
		public EndlessTrack.TrackID trackStartID;

		public EndlessBlock startBlock;

		public EndlessBlock endBlock;

		public EndlessBlock[] blocks;
	}

	protected class MinMaxIndexes
	{
		public int minIndex;

		public int maxIndex;
	}

	protected struct BlockData
	{
		public BlockData(EndlessTrack.TrackID _ambient, int lmOffset)
		{
			this.ambient = _ambient;
			this.lightmapserializedObjectffset = lmOffset;
		}

		public EndlessTrack.TrackID ambient;

		public int lightmapserializedObjectffset;
	}
}
