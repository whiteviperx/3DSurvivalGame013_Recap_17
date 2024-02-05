using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.SceneManagement;

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

	// Json Project Save Path
	private string jsonPathProject;

	// Json External/Real Save Path
	private string jsonPathPersistant;

	// Binary Save Path
	private string binaryPath;

	public bool isSavingToJson;

	private void Start()
		{
		jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
		jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveGame.json";
		binaryPath = Application.persistentDataPath + "/save_game.bin";
		}

	#region || --- General Section --- ||

	#region || --- Saving Section --- ||

	public void SaveGame()
		{
		var data = gameObject.AddComponent<AllGameData>();

		SavingTypeSwitch(data);
		}

	private PlayerData GetPlayerData()
		{
		var playerStats = new float [3];
		playerStats [0] = PlayerState.Instance.currentHealth;
		playerStats [1] = PlayerState.Instance.currentFood;
		playerStats [2] = PlayerState.Instance.currentWaterPercent;

		var playerPosAndRot = new float [6];
		playerPosAndRot [0] = PlayerState.Instance.playerBody.transform.position.x;
		playerPosAndRot [1] = PlayerState.Instance.playerBody.transform.position.y;
		playerPosAndRot [2] = PlayerState.Instance.playerBody.transform.position.z;

		playerPosAndRot [3] = PlayerState.Instance.playerBody.transform.rotation.x;
		playerPosAndRot [4] = PlayerState.Instance.playerBody.transform.rotation.y;
		playerPosAndRot [5] = PlayerState.Instance.playerBody.transform.rotation.z;

		return new(playerStats, playerPosAndRot);
		}

	public void SavingTypeSwitch(AllGameData gameData)
		{
		if (isSavingToJson)
			{
			SaveGameDataToJsonFile(gameData);
			}
		else
			{
			SaveGameDataToBinaryFile(gameData);
			}
		}

	#endregion || --- Saving Section --- ||

	#region || --- Loading Section --- ||

	// --- Loading Section --- //
	public AllGameData LoadingTypeSwitch()
		{
		if (isSavingToJson)
			{
			AllGameData gameData = LoadGameDataFromJsonFile();
			//AllGameData gameData = LoadGameDataFromBinaryFile();
			return gameData;
			}
		else
			{
			AllGameData gameData = LoadGameDataFromBinaryFile();
			return gameData;
			}
		}

	public void LoadGame()
		{
		// --- Player Data --- //
		SetPlayerData(LoadingTypeSwitch().playerData);

		// --- Enviroment Data --- //
		// setEnviroment();
		}

	private void SetPlayerData(PlayerData playerData)
		{
		// --- Setting Player Stats --- //
		PlayerState.Instance.currentHealth = playerData.playerStats [0];
		PlayerState.Instance.currentFood = playerData.playerStats [1];
		PlayerState.Instance.currentWaterPercent = playerData.playerStats [2];

		// --- Setting Player Position --- //
		Vector3 loadedPosition;
		loadedPosition.x = playerData.playerPositionAndRotation [0];
		loadedPosition.y = playerData.playerPositionAndRotation [1];
		loadedPosition.z = playerData.playerPositionAndRotation [2];

		PlayerState.Instance.playerBody.transform.position = loadedPosition;

		// --- Setting Player Rotation --- //
		Vector3 loadedRotation;
		loadedRotation.x = playerData.playerPositionAndRotation [3];
		loadedRotation.y = playerData.playerPositionAndRotation [4];
		loadedRotation.z = playerData.playerPositionAndRotation [5];

		PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);
		}

	public void StartLoadedGame()
		{
		SceneManager.LoadScene("GameScene");

		StartCoroutine(DelayedLoading());
		}

	private IEnumerator DelayedLoading()
		{
		yield return new WaitForSeconds(1f);

		LoadGame();

		print("Game Loaded line 144");
		}

	#endregion || --- Loading Section --- ||

	#endregion || --- General Section --- ||

	#region || --- To Binary Section --- ||

	// Save Game Data To Binary File //
	public void SaveGameDataToBinaryFile(AllGameData gameData)
		{
		BinaryFormatter formatter = new();

		// --- Path to save to --- //
		string path = binaryPath;

		// FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		FileStream stream = new FileStream(binaryPath, FileMode.Create);

		// --- Serialize the data --- //
		formatter.Serialize(stream, gameData);
		stream.Close();

		// --- Print "Data saved to location" --- //
		print("Data saved to" + binaryPath);
		}

	public AllGameData LoadGameDataFromBinaryFile()
		{
		// string path = Application.persistentDataPath + "/save_game.bin";

		if (File.Exists(binaryPath))
			{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(binaryPath, FileMode.Open);

			AllGameData data = formatter.Deserialize(stream) as AllGameData;
			stream.Close();

			print("Data Loaded from" + binaryPath);

			return data;
			}
		else
			{
			return null;
			}
		}

	#endregion || --- To Binary Section --- ||

	#region || --- Json Section --- ||

	// Save Game Data To Binary File //
	public void SaveGameDataToJsonFile(AllGameData gameData)
		{
		string json = JsonUtility.ToJson(gameData);

		using (StreamWriter writer = new StreamWriter(jsonPathProject))
			{
			writer.Write(json);
			print("Saved Game to Json file at :" + jsonPathProject);
			};
		}

	public AllGameData LoadGameDataFromJsonFile()
		{
		using (StreamReader reader = new StreamReader(jsonPathProject))
			{
			string json = reader.ReadToEnd();

			AllGameData data = JsonUtility.FromJson<AllGameData>(json);
			return data;
			};
		}

	#endregion || --- Json Section --- ||

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