// dnSpy decompiler from Assembly-CSharp-firstpass.dll class: UnityEngine.Purchasing.DemoInventory
using System;

namespace UnityEngine.Purchasing
{
	[AddComponentMenu("")]
	public class DemoInventory : MonoBehaviour
	{
		public void Fulfill(string productId)
		{
			if (productId != null)
			{
				if (productId == "100.gold.coins")
				{
					UnityEngine.Debug.Log("You Got Money!");
					return;
				}
			}
			UnityEngine.Debug.Log(string.Format("Unrecognized productId \"{0}\"", productId));
		}
	}
}
