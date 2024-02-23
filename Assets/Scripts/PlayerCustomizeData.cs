// dnSpy decompiler from Assembly-CSharp.dll class: PlayerCustomizeData
using System;
using UnityEngine;

[Serializable]
public class PlayerCustomizeData
{
	public PlayerCustomizeData(CarID _carId, int _acceleration, int _speed, int _resistance, int _nitroSpeed, int _grip, bool _owned, int maxAcc, int maxS, int maxResist, int maxNitro, int max_Grip, bool[] c, bool[] s, bool[] r, int sC, int sS, int sR)
	{
		this.carId = _carId;
		this.acceleration = _acceleration;
		this.speed = _speed;
		this.resistance = _resistance;
		this.nitroSpeed = _nitroSpeed;
		this.grip = _grip;
		this.owned = _owned;
		this.maxAcceleration = maxAcc;
		this.maxSpeed = maxS;
		this.maxResistance = maxResist;
		this.maxNitroSpeed = maxNitro;
		this.maxGrip = max_Grip;
		this.rims = r;
		this.clrs = c;
		this.sticker = s;
		this.currRim = sR;
		this.currClr = sC;
		this.currSticker = sS;
	}

	public void BuyCar()
	{
		this.owned = true;
		PlayerDataPersistant.Instance.SaveGameData(true);
	}

	public int GetStatAmountByIndex(int iIndex)
	{
		switch (iIndex)
		{
		default:
			return this.acceleration;
		case 1:
			return this.speed;
		case 2:
			return this.resistance;
		case 3:
			return this.nitroSpeed;
		case 4:
			return this.grip;
		}
	}

	public int GetMaxAmountByIndex(int iIndex)
	{
		switch (iIndex)
		{
		default:
			return this.maxAcceleration;
		case 1:
			return this.maxSpeed;
		case 2:
			return this.maxResistance;
		case 3:
			return this.maxNitroSpeed;
		case 4:
			return this.maxGrip;
		}
	}

	public bool GetCustomizationValueByIndex(CustomiztionType t, int arrayIndex)
	{
		switch (t)
		{
		default:
			return this.clrs[arrayIndex];
		case CustomiztionType.Sticker:
			return this.sticker[arrayIndex];
		case CustomiztionType.Rim:
			return this.rims[arrayIndex];
		}
	}

	public int GetCurrentItemByIndex(CustomiztionType t)
	{
		switch (t)
		{
		default:
			return this.currClr;
		case CustomiztionType.Sticker:
			return this.currSticker;
		case CustomiztionType.Rim:
			return this.currRim;
		}
	}

	public CarID carId;

	[Space(5f)]
	[Range(1f, 10f)]
	public int acceleration;

	[Space(5f)]
	[Range(1f, 10f)]
	public int speed;

	[Space(5f)]
	[Range(1f, 10f)]
	public int resistance;

	[Space(5f)]
	[Range(1f, 10f)]
	public int nitroSpeed;

	[Space(5f)]
	[Range(1f, 10f)]
	public int grip;

	[Space(5f)]
	[Range(1f, 10f)]
	public int maxAcceleration;

	[Space(5f)]
	[Range(1f, 10f)]
	public int maxSpeed;

	[Space(5f)]
	[Range(1f, 10f)]
	public int maxResistance;

	[Space(5f)]
	[Range(1f, 10f)]
	public int maxNitroSpeed;

	[Space(5f)]
	[Range(1f, 10f)]
	public int maxGrip;

	public bool owned;

    public bool[] sticker;

    public bool[] clrs;

    public bool[] rims;

    public int currRim;

    public int currClr;

    public int currSticker;
}
