[System.Serializable]
public class PlayerData
	{
	public float [] playerStats; // --- [0] - Health, [1] - Food, [2] - Water --- //

	public float [] playerPositionAndRotation; // --- Position [0]x,[1]y,[2]z and Rotation [3]x,[4]y,[5]z --- //

	public string [] inventoryContent;

	public string [] quickSlotsContent;

	public PlayerData(float [] _playerStats, float [] _playerPosAndRot, string [] _inventoryContent, string [] quickSlotsContent)
		{
		playerStats = _playerStats;
		playerPositionAndRotation = _playerPosAndRot;
		inventoryContent = _inventoryContent;
		//quickSlotsContent = _quickSlotsContent;
		}
	}