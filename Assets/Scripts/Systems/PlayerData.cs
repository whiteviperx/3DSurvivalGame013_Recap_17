[System.Serializable]
public class PlayerData
	{
	public float [] playerStats;

	//public float playerHealth;
	//public float playerWater;
	//public float playerFood;

	public float [] playerPositionAndRotation;

	// public string [] inventoryContent;

	public PlayerData(float [] _playerStats, float [] _playerPosAndRot)
		{
		playerStats = _playerStats;
		playerPositionAndRotation = _playerPosAndRot;
		}
	}