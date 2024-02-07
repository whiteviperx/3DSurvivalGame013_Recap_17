[System.Serializable]
public class PlayerData
	{
	public float [] playerStats; // --- [0] - Health, [1] - Food, [2] - Water --- //

	//public float playerHealth;
	//public float playerFood;
	//public float playerWater;

	public float [] playerPositionAndRotation; // --- Position [0]x,[1]y,[2]z and Rotation [3]x,[4]y,[5]z --- //

	// public string [] inventoryContent;

	public PlayerData(float [] _playerStats, float [] _playerPosAndRot)
		{
		playerStats = _playerStats;
		playerPositionAndRotation = _playerPosAndRot;
		}
	}