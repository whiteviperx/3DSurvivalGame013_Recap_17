using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

public class SaveManager:MonoBehaviour
	{
	public static SaveManager Instance { get; set; }

	private void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			}
		else
			{
			Instance = this;
			}

		DontDestroyOnLoad(gameObject);
		}

	public bool isSavingJson;

	#region || --- General Section --- ||

	public void SaveGame()
		{
		AllGameData data = new AllGameData();

		data.playerData = GetPlayerData();

		SaveAllGameData(data);
		}

	private PlayerData GetPlayerData()
		{
		float [] playerStats = new float [3];
		playerStats [0] = PlayerState.Instance.currentHealth;
		playerStats [1] = PlayerState.Instance.currentFood;
		playerStats [2] = PlayerState.Instance.currentWaterPercent;

		float [] playerPosAndRot = new float [6];
		playerPosAndRot [0] = PlayerState.Instance.playerBody.transform.position.x;
		playerPosAndRot [1] = PlayerState.Instance.playerBody.transform.position.y;
		playerPosAndRot [2] = PlayerState.Instance.playerBody.transform.position.z;

		playerPosAndRot [3] = PlayerState.Instance.playerBody.transform.rotation.x;
		playerPosAndRot [4] = PlayerState.Instance.playerBody.transform.rotation.y;
		playerPosAndRot [5] = PlayerState.Instance.playerBody.transform.rotation.z;

		return new PlayerData(playerStats, playerPosAndRot);
		}

	public void SaveAllGameData(AllGameData gameData)
		{
		if (isSavingJson)
			{
			// SaveGameDataToJsonFile(gameData);
			}
		else
			{
			SaveGameDataToBinaryFile(gameData);
			}
		}

	#endregion || --- General Section --- ||

	#region || --- To Binary Section --- ||

	// Save Game Data To Binary File //
	public void SaveGameDataToBinaryFile(AllGameData gameData)
		{
		BinaryFormatter formatter = new();

		// --- Path to save to --- //
		string path = Application.persistentDataPath + "/save_game.bin";

		// FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		FileStream stream = new(path, FileMode.Create);

		// --- Serialize the data --- //
		formatter.Serialize(stream, gameData);
		stream.Close();

		// --- Print "Data saved to location" --- //
		print("Data saved to" + Application.persistentDataPath + "/save_game.bin");
		}

	public AllGameData LoadGameDataFromBinaryFile()
		{
		string path = Application.persistentDataPath + "/save_game.bin";

		if (File.Exists(path))
			{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new(path, FileMode.Open);

			AllGameData data = formatter.Deserialize(stream) as AllGameData;
			stream.Close();

			return data;
			}
		else
			{
			return null;
			}
		}

	#endregion || --- To Binary Section --- ||

	#region || --- Settings Section --- ||

	#region || --- Volume Section --- ||

	[System.Serializable]
	public class VolumeSettings
		{
		public float music;

		public float effects;

		public float master;
		}

	public void SaveVolumeSettings(float _music, float _effects, float _master)
		{
		VolumeSettings volumeSettings = new()
			{
			music = _music,
			effects = _effects,
			master = _master
			};

		PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
		PlayerPrefs.Save();

		print("Saved to Player Prefs");
		}

	public VolumeSettings LoadVolumeSettings()
		{
		return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
		}

	#endregion || --- Volume Section --- ||

	#endregion || --- Settings Section --- ||
	}