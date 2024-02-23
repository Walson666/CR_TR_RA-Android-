// dnSpy decompiler from Assembly-CSharp.dll class: Configurations
using System;
using System.Collections.Generic;
using UnityEngine;

public class Configurations : ScriptableObject
{
	public int WorldCount
	{
		get
		{
			if (this.worldInfo.Count == 0)
			{
				this.worldInfo.Add(new Configurations.WorldInfo());
				this.iapItems = new IAPItem[5];
				Configurations.DirtyEditor();
			}
			return this.worldInfo.Count;
		}
		set
		{
			if (value > this.worldInfo.Count && value <= 10)
			{
				for (int i = this.worldInfo.Count; i < value; i++)
				{
					this.worldInfo.Add(new Configurations.WorldInfo());
				}
			}
			Configurations.DirtyEditor();
		}
	}

	public static void FillDefaultValues(out Configurations.TrackData data)
	{
		data = new Configurations.TrackData();
		data.worldBlockID = EndlessTrack.TrackID.WORLD_CHUNK_1;
		data.trackDataList = new List<EndlessTrack.TrackData>();
		EndlessTrack.TrackData trackData = new EndlessTrack.TrackData();
		trackData.trackStartID = EndlessTrack.TrackID.WORLD_CHUNK_1;
		trackData.blocks = new EndlessBlock[1];
		data.trackDataList.Add(trackData);
		data.stageConfiguration = new List<TrackController.StageConfiguration>();
		TrackController.StageConfiguration stageConfiguration = new TrackController.StageConfiguration();
		stageConfiguration.ambientIdList = new List<EndlessTrack.TrackID>();
		stageConfiguration.ambientIdList.Add(EndlessTrack.TrackID.WORLD_CHUNK_1);
		stageConfiguration.minDurationInMeters = 50000f;
		stageConfiguration.maxDurationInMeters = 60000f;
		stageConfiguration.minSingleAmbientDurationInMeters = 2000f;
		stageConfiguration.maxSingleAmbientDurationInMeters = 3000f;
		data.stageConfiguration.Add(stageConfiguration);
	}

	public void RemoveAtIndex(int i)
	{
		this.worldInfo.RemoveAt(i);
	}

	public static Configurations Instance
	{
		get
		{
			if (Configurations.instance == null)
			{
				Configurations.instance = (Resources.Load("GameConfigs") as Configurations);
				if (Configurations.instance == null)
				{
					Configurations.instance = ScriptableObject.CreateInstance<Configurations>();
				}
			}
			return Configurations.instance;
		}
	}

	public static void DirtyEditor()
	{
	}

	public WorldBetInfo[] bets;

	public List<Configurations.WorldInfo> worldInfo = new List<Configurations.WorldInfo>();

	public IAPItem[] iapItems;

	public int[] dailyRewardPrices = new int[5];

	public const int MAX_WORLDS = 10;

	public bool useAds = true;

	public bool useIAP;

	public bool useGPGC;

	public bool useFb;

	public const string ENABLE_ADS = "ENABLE_ADS";

	public const string ENABLE_IAP = "ENABLE_IAP";

	public const string ENABLE_GPGC = "ENABLE_GPGC";

	private const string assetDataPath = "Assets/CrazyTrafficRacing/Prefabs/Resources/";

	private const string assetName = "GameConfigs";

	private const string assetExt = ".asset";

	private static Configurations instance;

	[Serializable]
	public class TrackData
	{
		public EndlessTrack.TrackID worldBlockID;

		public List<EndlessTrack.TrackData> trackDataList;

		public List<TrackController.StageConfiguration> stageConfiguration;
	}

	[Serializable]
	public class WorldInfo
	{
		public WorldInfo()
		{
			this.worldName = "New World ";
			this.coinsNeededToUnlock = 1000;
			Configurations.FillDefaultValues(out this.trackInfo);
		}

		public string worldName;

		public int coinsNeededToUnlock;

		public Sprite worldIcon;

		public Material skyboxMat;

		public AudioClip inGameMusic;

		public Configurations.TrackData trackInfo;
	}
}
