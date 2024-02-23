// dnSpy decompiler from Assembly-CSharp.dll class: CarData
using System;

[Serializable]
public class CarData
{
	public CarID carID
	{
		get
		{
			return this.car.customizationData.carId;
		}
	}

	public int intID
	{
		get
		{
			return (int)this.car.customizationData.carId;
		}
	}

	public PlayerMovement car;

	public string _name;

	public int cost;
}
