// dnSpy decompiler from Assembly-CSharp.dll class: PlayerDataPersistant
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerDataPersistant
{
	PlayerData PlayerData;
	public PlayerDataPersistant()
	{
		PlayerDataPersistant.instance = this;
		this.playerDataList = new Dictionary<CarID, PlayerCustomizeData>();
	}

	public static PlayerDataPersistant Instance
	{
		get
		{
			if (PlayerDataPersistant.instance == null)
			{
				PlayerDataPersistant.instance = new PlayerDataPersistant();
			}
			return PlayerDataPersistant.instance;
		}
	}

	/*public int TicketSpin
	{
		get
		{
			return this._spinTicket;
		}
		set
		{
			this._spinTicket = value;
			Singleton<UIManager>.Instance.spinUIMan.spinsText.text = this._spinTicket.ToString();
		}
	}*/

	public int Coins
	{
		get
		{
			return this._coins;
		}
		set
		{
			this._coins = value;
			Singleton<UIManager>.Instance.UpdateCoinsCount();
		}
	}

	public float LevelProgress
	{
		get
		{
			return this._progressNextLevel;
		}
		set
		{
			this._progressNextLevel = value;
			if (this._progressNextLevel >= 250f)
			{
				this._progressNextLevel = 0f;
				this.Level++;
			}
			Singleton<UIManager>.Instance.levelProgressBar.fillAmount = this._progressNextLevel / 250f;
		}
	}

	public int Level
	{
		get
		{
			return this._level;
		}
		set
		{
			this._level = value;
			Singleton<UIManager>.Instance.playerLevelText.text = string.Format("Level {0}", GameUtils.GetValueFormated(this._level));
		}
	}

	public int CurentCar
	{
		get
		{
			if(PlayerData != null)
			return PlayerData.CurentCar;
			else return 0;
		}
		set
		{
			if(PlayerData != null)
				PlayerData.CurentCar = value;
		}
	}

	public Dictionary<CarID, PlayerCustomizeData> CarList
	{
		get
		{
			return this.playerDataList;
		}
	}

	public void UpdateValues(PlayerCustomizeData pData)
	{
		this.CarList[pData.carId].acceleration = pData.acceleration;
		this.CarList[pData.carId].speed = pData.speed;
		this.CarList[pData.carId].resistance = pData.resistance;
		this.CarList[pData.carId].nitroSpeed = pData.nitroSpeed;
		this.GetPlayerData(pData.carId).BuyCar();
	}

	public void InitPlayerData(CarID carId)
	{
		this.CarList[carId].acceleration = Mathf.Max(this.CarList[carId].acceleration, 1);
		this.CarList[carId].speed = Mathf.Max(this.CarList[carId].speed, 1);
		this.CarList[carId].resistance = Mathf.Max(this.CarList[carId].resistance, 1);
		this.CarList[carId].nitroSpeed = Mathf.Max(this.CarList[carId].nitroSpeed, 1);
	}

	public bool IsWorldLocked(int index)
	{
		return this.worldLockStatus[index];
	}

	public bool IsWorldLockedByEnv(int env)
	{
		return this.worldLockStatus[env];
	}

	public void UnlockWorld(int index)
	{
		this.worldLockStatus[index] = false;
	}

	public PlayerCustomizeData GetPlayerData(CarID carId)
	{
		return this.CarList[carId];
	}

	public bool CanAfford(float cost)
	{
		return (float)this.Coins >= cost;
	}

	public void BuyItem(int cost)
	{
		this.Coins -= cost;
	}

	public void IncreaseCoins(int ammount)
	{
		this.Coins += ammount;
	}



    public void Load()
	{
		string saveJson;

        /*if (GP_Player.IsLoggedIn())
		{
            saveJson = GP_Player.GetString("PLAYERDATA");
        }*/

		
            saveJson = PlayerPrefs.GetString("PLAYERDATA");
        
             

		bool isEmpty = string.IsNullOrEmpty(saveJson);
        //Debug.LogError("ISEMPTY:" + isEmpty);
        if (isEmpty)
		{
            PlayerData = new PlayerData();
			PlayerData.CarData = new PlayerCarData[Singleton<GamePlay>.Instance.playerCars.Length];
			for (int i = 0; i < Singleton<GamePlay>.Instance.playerCars.Length; i++)
			{
                PlayerCustomizeData customizationData = Singleton<GamePlay>.Instance.playerCars[i].car.customizationData;
				PlayerData.CarData[i] = new PlayerCarData(customizationData);
            }
        }
            
		else
            PlayerData = JsonUtility.FromJson<PlayerData>(saveJson);

        for (int i = 0; i < Singleton<GamePlay>.Instance.playerCars.Length; i++)
        {
            PlayerCustomizeData customizationData = Singleton<GamePlay>.Instance.playerCars[i].car.customizationData;
            int @int = isEmpty? customizationData.acceleration: PlayerData.CarData[i].Acceleration;
            int int2 = isEmpty ? customizationData.speed : PlayerData.CarData[i].Speed;
            int int3 = isEmpty ? customizationData.resistance : PlayerData.CarData[i].Resistance;
            int int4 = isEmpty ? customizationData.nitroSpeed : PlayerData.CarData[i].NitroSpeed;
            int int5 = isEmpty ? customizationData.grip : PlayerData.CarData[i].Grip;
            bool owned = isEmpty ? customizationData.owned || Singleton<GamePlay>.Instance.playerCars[i].cost == 0 : PlayerData.CarData[i].Owned || Singleton<GamePlay>.Instance.playerCars[i].cost == 0;
            bool[] boolArray = isEmpty ? new bool[15] : PlayerData.CarData[i].Clrs;
            bool[] boolArray2 = isEmpty ? new bool[8] : PlayerData.CarData[i].Sticker;
            bool[] boolArray3 = isEmpty ? new bool[4] : PlayerData.CarData[i].Rims;
            int int6 = isEmpty ? 0 : PlayerData.CarData[i].CurrRim;
            int int7 = isEmpty ? 0 : PlayerData.CarData[i].CurrClr;
            int int8 = isEmpty ? 0 : PlayerData.CarData[i].CurrSticker;

            if (!this.CarList.ContainsKey(Singleton<GamePlay>.Instance.playerCars[i].carID))
            {
                this.CarList.Add(Singleton<GamePlay>.Instance.playerCars[i].carID, new PlayerCustomizeData(Singleton<GamePlay>.Instance.playerCars[i].carID, @int, int2, int3, int4, int5, owned, customizationData.maxAcceleration, customizationData.maxSpeed, customizationData.maxResistance, customizationData.maxNitroSpeed, customizationData.maxGrip, boolArray, boolArray2, boolArray3, int7, int8, int6));
            }
        }

        this.worldLockStatus = PlayerData.WorldLockStatus;
        this.gameModeScores = PlayerData.GameModeScores;


        for (int j = 0; j < this.worldLockStatus.Length; j++)
		{
			this.worldLockStatus[j] = (Configurations.Instance.worldInfo[j].coinsNeededToUnlock > 0);
		}
        this.Coins = PlayerData.Coins;
        //this.TicketSpin = PlayerData.TicketSpin;
        this.LevelProgress = PlayerData.LevelProgress;
        this.Level = PlayerData.Level;
    }

	public int GetCarUpgradeValue(CarID carID, int upgradeID)
	{
		return this.GetPlayerData(carID).GetStatAmountByIndex(upgradeID);
	}

	public int GetMaxValue(CarID carID, int upgradeID)
	{
		return this.GetPlayerData(carID).GetMaxAmountByIndex(upgradeID);
	}

	public bool GetCustomizationStatus(CarID carID, CustomiztionType t, int arrInd)
	{
		return this.GetPlayerData(carID).GetCustomizationValueByIndex(t, arrInd);
	}

	public void SetCarCustomizationValue(CarID carID, UICarCustomizeItem customizationId)
	{
		string str = carID.ToString();
		switch (customizationId.customiztionType)
		{
		default:
			this.CarList[carID].clrs[customizationId.id] = customizationId.Owned;
			//PlayerPrefz.SetBoolArray(str + "CAR_COLOR", this.CarList[carID].clrs);
			break;
		case CustomiztionType.Sticker:
			this.CarList[carID].sticker[customizationId.id] = customizationId.Owned;
			//PlayerPrefz.SetBoolArray(str + "STICKER", this.CarList[carID].sticker);
			break;
		case CustomiztionType.Rim:
			this.CarList[carID].rims[customizationId.id] = customizationId.Owned;
			//PlayerPrefz.SetBoolArray(str + "CAR_RIM_", this.CarList[carID].rims);
			break;
		}
		//PlayerPrefs.Save();
		Debug.LogError("SetCarCustomizationValue");
        SaveGameData(true);
    }

	public void SetCarCurrentCustomizationValue(CarID carID, UICarCustomizeItem customizeItem)
	{
		string str = carID.ToString();
		switch (customizeItem.customiztionType)
		{
		default:
			this.CarList[carID].currClr = customizeItem.id;
			//PlayerPrefs.SetInt(str + "CURRENT_CLR", this.CarList[carID].currClr);
			break;
		case CustomiztionType.Sticker:
			this.CarList[carID].currSticker = customizeItem.id;
			//PlayerPrefs.SetInt(str + "CURRENT_STICKER", this.CarList[carID].currSticker);
			break;
		case CustomiztionType.Rim:
			this.CarList[carID].currRim = customizeItem.id;
			//PlayerPrefs.SetInt(str + "CURRENT_RIM", this.CarList[carID].currRim);
			break;
		}
        //PlayerPrefs.Save();
        Debug.LogError("SetCarCurrentCustomizationValue");
        SaveGameData(false);
    }

	public void SetCarUpgradeValue(CarID carID, int upgradeID, int val)
	{
		string str = carID.ToString();
		switch (upgradeID)
		{
		case 0:
			this.CarList[carID].acceleration = val;
			//PlayerPrefs.SetInt(str + "_P_ACC", val);
			break;
		case 1:
			this.CarList[carID].speed = val;
			//PlayerPrefs.SetInt(str + "_MAX_SPEED", val);
			break;
		case 2:
			this.CarList[carID].resistance = val;
			//PlayerPrefs.SetInt(str + "_P_RESISTANCE", val);
			break;
		case 3:
			this.CarList[carID].nitroSpeed = val;
			//PlayerPrefs.SetInt(str + "NITRO_SPEED", val);
			break;
		case 4:
			this.CarList[carID].grip = val;
			//PlayerPrefs.SetInt(str + "DAGRIP", val);
			break;
		}
		SaveGameData(true);
        //Debug.LogError("SetCarCurrentCustomizationValue");
    }

	public int BestMeters
	{
		get
		{
			return this._bestMeters;
		}
		set
		{
			this._bestMeters = value;
		}
	}

	public void SaveGameData(bool sync)
	{
        for (int i = 0; i < CarList.Count; i++)
        {
            CarID id = Singleton<GamePlay>.Instance.playerCars[i].car.carId;
            PlayerData.CarData[i] = new PlayerCarData(CarList[id]);
        }

        /*for (int i = 1; i < Singleton<GamePlay>.Instance.playerCars.Length; i++)
		{
			string str = Singleton<GamePlay>.Instance.playerCars[i].carID.ToString();
			PlayerCustomizeData playerCustomizeData = this.CarList[Singleton<GamePlay>.Instance.playerCars[i].carID];
			PlayerPrefs.SetInt(str + "_P_ACC", playerCustomizeData.acceleration);
			PlayerPrefs.SetInt(str + "_MAX_SPEED", playerCustomizeData.speed);
			PlayerPrefs.SetInt(str + "_P_RESISTANCE", playerCustomizeData.resistance);
			PlayerPrefs.SetInt(str + "NITRO_SPEED", playerCustomizeData.nitroSpeed);
			PlayerPrefs.SetInt(str + "DAGRIP", playerCustomizeData.grip);
			PlayerPrefz.SetBool(str + "CAR_OWNED", playerCustomizeData.owned);
			PlayerPrefz.SetBoolArray(str + "CAR_COLOR", playerCustomizeData.clrs);
			PlayerPrefz.SetBoolArray(str + "STICKER", playerCustomizeData.sticker);
			PlayerPrefz.SetBoolArray(str + "CAR_RIM_", playerCustomizeData.rims);
		}*/
		PlayerData.WorldLockStatus = this.worldLockStatus;
		PlayerData.GameModeScores = this.gameModeScores;
		PlayerData.LevelProgress = this.LevelProgress;
		PlayerData.Level = this.Level;
		PlayerData.Coins = this.Coins;
		//PlayerData.TicketSpin = this.TicketSpin;

		//PlayerPrefz.SetBoolArray("WORLD_LOCKED_", this.worldLockStatus);
		//PlayerPrefz.SetIntArray("GAME_MODE_SCORES", this.gameModeScores);
		//PlayerPrefs.SetFloat("P4PROGRESS", this.LevelProgress);
		//PlayerPrefs.SetInt("P4LEVEL", this.Level);
		//PlayerPrefs.SetInt("_COINS_", this.Coins);
		//PlayerPrefs.SetInt("SPIN_TICKETS", this.TicketSpin);
		//PlayerPrefs.Save();
		string data = JsonUtility.ToJson(PlayerData);
		//Debug.Log("Saving value:" + data);
		/*if(GP_Player.IsLoggedIn())
		{
            GP_Player.Set("PLAYERDATA", data);
            if (sync)
            {
                GP_Player.Sync();
            }
        }
		else
		{*/
			PlayerPrefs.SetString("PLAYERDATA", data);
			if(sync)
			{
				PlayerPrefs.Save();
			}
		//}
		
		
		
		//PlayerPrefs.SetString("PLAYERDATA", data);
		//PlayerPrefs.Save();
        //GP_Player.Set("PLAYERDATA", JsonUtility.ToJson(PlayerData));
	}

	private static PlayerDataPersistant instance;

	private Dictionary<CarID, PlayerCustomizeData> playerDataList;

	private int _coins = 1500;

	private bool[] worldLockStatus;

	public const int RIM_COUNT = 4;

	public const int CLR_COUNT = 15;

	public const int STICKER_COUNT = 8;

	private int _spinTicket;

	private const int NEXT_LEVEL = 250;

	private float _progressNextLevel;

	private int _level;

	public const string P_LEVEL = "P4LEVEL";

	public const string P_PROGRESS = "P4PROGRESS";

	public const string _COINS = "_COINS_";

	public const string PACC = "_P_ACC";

	public const string PMS = "_MAX_SPEED";

	public const string PRES = "_P_RESISTANCE";

	public const string NITRO_SPEED = "NITRO_SPEED";

	public const string GRIP = "DAGRIP";

	public const string OWNED = "CAR_OWNED";

	public const string WORLD_LOCKED = "WORLD_LOCKED_";

	public const string CAR_STICKER = "STICKER";

	public const string CAR_RIM = "CAR_RIM_";

	public const string CAR_COLOR = "CAR_COLOR";

	public const string CURRENT_RIM = "CURRENT_RIM";

	public const string CURRENT_CLR = "CURRENT_CLR";

	public const string CURRENT_STICKER = "CURRENT_STICKER";

	public const string SPIN_TICKETS = "SPIN_TICKETS";

	public const string GAME_MODE_SCORES = "GAME_MODE_SCORES";

	public int[] gameModeScores;

	private int _bestMeters;
}

[System.Serializable]
public class PlayerData
{
	public int CurentCar;
	public int Coins = 0;
	public int TicketSpin = 0;
	public float LevelProgress = 0;
	public int Level = 30;
	public int[] GameModeScores;
	public bool[] WorldLockStatus;
	public PlayerCarData[] CarData;

	public PlayerData()
	{
		Coins = 1500;
		TicketSpin = 2;
		LevelProgress = 0;
		Level = 1;
		GameModeScores = new int[3]
		{
			0,0,0
		};
		WorldLockStatus = new bool[3]
		{
			false, false, false
		};
	}
}

[System.Serializable]
public class PlayerCarData
{
    public int Acceleration;
    public int Speed;
    public int Resistance;
    public int NitroSpeed;
    public int Grip;
    public bool Owned;
    public bool[] Sticker;
    public bool[] Clrs;
    public bool[] Rims;
    public int CurrRim = 0;
    public int CurrClr = 0;
    public int CurrSticker = 0;
    public PlayerCarData(PlayerCustomizeData playerCustomizeData)
	{
		Acceleration = playerCustomizeData.acceleration;
		Speed = playerCustomizeData.speed;
		Resistance = playerCustomizeData.resistance;
		NitroSpeed = playerCustomizeData.nitroSpeed;
		Grip = playerCustomizeData.grip;
		Owned = playerCustomizeData.owned;
		Clrs = playerCustomizeData.clrs;
		Sticker = playerCustomizeData.sticker;
		Rims = playerCustomizeData.rims;
        CurrRim = playerCustomizeData.currRim;
        CurrClr = playerCustomizeData.currClr;
        CurrSticker = playerCustomizeData.currSticker;
    }
}