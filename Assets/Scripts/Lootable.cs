using System.Collections.Generic;

using NUnit.Framework;

using UnityEngine;

public class Lootable:MonoBehaviour
{
	public List<LootPossibility> possibleLoot;

	public List<LootReceived> finalLoot;

	public bool wasLootCalculated;

	[System.Serializable]
	public class LootPossibility
	{
		public GameObject item;

		public int amountMin;

		public int amountMax;
	}

	[System.Serializable]
	public class LootReceived
	{
		public GameObject item;

		public int amount;

	}
}