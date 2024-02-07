using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager:MonoBehaviour
	{
	public static SaveManager Instance { get; set; }

	#region || --- ????? Section --- ||
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

	// --- Json Project Save Path Use for Testing --- //
	private string jsonPathProject;

	// --- External/Real Save Path Use for Real Game --- //
	private string jsonPathPersistent;

	// --- Binary Save Path --- //
	private string binaryPath;
	private string fileName = "SaveGame";

	public bool isSavingToJson;

	private void Start()
		{
		// --- Testing Save Path--- //
		jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;

		// --- Real Save Path --- //
		jsonPathPersistent = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

		// --- Binary Save Path --- //
		binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
		}
	#endregion || --- ????? Section --- ||
	#region || --- General Section --- ||

	#region || --- Saving Section --- ||

	public void SaveGame(int slotNumber)
		{
		AllGameData data = new()
			{
			playerData = GetPlayerData()
			};

		SavingTypeSwitch(data, slotNumber);
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

		return new(playerStats, playerPosAndRot);
		}

	public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
		{
		if (isSavingToJson)
			{
			SaveGameDataToJsonFile(gameData, slotNumber);
			}
		else
			{
			SaveGameDataToBinaryFile(gameData, slotNumber);
			}
		}

	#endregion || --- Saving Section --- ||

	#region || --- Loading Section --- ||

	// --- Loading Section --- //
	public AllGameData LoadingTypeSwitch(int slotNumber)
		{
		if (isSavingToJson)
			{
			AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);

			// AllGameData gameData = LoadGameDataFromBinaryFile();
			return gameData;
			}
		else
			{
			AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
			return gameData;
			}
		}

	public void LoadGame(int slotNumber)
		{
		// --- Player Data --- //
		SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);

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

	public void StartLoadedGame(int slotNumber)
		{
		SceneManager.LoadScene("GameScene");

		StartCoroutine(DelayedLoading(slotNumber));
		}

	private IEnumerator DelayedLoading(int slotNumber)
		{
		yield return new WaitForSeconds(1f);

		LoadGame(slotNumber);

		Debug.Log("Game Loaded line 170");
		}

	#endregion || --- Loading Section --- ||

	#endregion || --- General Section --- ||

	#region || --- To Binary Section --- ||

	// Save Game Data To Binary File //
	public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
		{
		BinaryFormatter formatter = new();

		// --- Path to save to --- //
		string path = binaryPath;

		// FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
		FileStream stream = new(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

		// --- Serialize the data --- //
		formatter.Serialize(stream, gameData + fileName + slotNumber + ".bin");
		stream.Close();

		// --- Print "Data saved to location" --- //
		Debug.Log("Line 190 Data saved to" + binaryPath + fileName + slotNumber + ".bin");
		}

	public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
		{
		// string path = Application.persistentDataPath + "/save_game.bin";

		if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
			{
			BinaryFormatter formatter = new();
			FileStream stream = new(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

			AllGameData data = formatter.Deserialize(stream) as AllGameData;
			stream.Close();

			Debug.Log("Line 205 Data Loaded from" + binaryPath + fileName + slotNumber + ".bin");

			return data;
			}
		else
			{
			return null;
			}
		}

	#endregion || --- To Binary Section --- ||

	#region || --- Json Section --- ||

	// --- Save Game Data To Json File --- //
	public void SaveGameDataToJsonFile(AllGameData gameData)
		{
		string json = JsonUtility.ToJson(gameData);

		string encrypted = EncryptionDecryption(json);

		using (StreamWriter writer = new StreamWriter(jsonPathProject))
			{
			writer.Write(encrypted);
			Debug.Log("Line 229 Saved Game to Json file at :" + jsonPathProject);
			};
		}

	public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
		{
		//string json = JsonUtility.ToJson(gameData + fileName + slotNumber + ".bin");
		string json = JsonUtility.ToJson(gameData + fileName + slotNumber + ".json");

		//string encrypted = EncryptionDecryption(json + fileName + slotNumber + ".bin");
		string encrypted = EncryptionDecryption(json + fileName + slotNumber + ".json");

		using (StreamWriter writer = new(jsonPathProject + fileName + slotNumber + ".json"))
			{
			//writer.Write(encrypted + fileName + slotNumber + ".bin");
			writer.Write(encrypted + fileName + slotNumber + ".json");
			Debug.Log("Line 245 Saved Game to Json file at :" + jsonPathProject + fileName + slotNumber + ".json");
			}
;
		}

	public AllGameData LoadGameDataFromJsonFile(int slotNumber)
		{
		using (StreamReader reader = new(jsonPathProject + fileName + slotNumber + ".json"))
			{
			string json = reader.ReadToEnd();

			string decrypted = EncryptionDecryption(json + fileName + slotNumber + ".bin");

			AllGameData data = JsonUtility.FromJson<AllGameData>(decrypted + fileName + slotNumber + ".bin");
			return data;
			}
;
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

		Debug.Log("Line 292 Saved to Player Prefs");
		}

	public VolumeSettings LoadVolumeSettings()
		{
		return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
		}

	#endregion || --- Volume Section --- ||

	#endregion || --- Settings Section --- ||

	#region || --- Encryption Section --- ||

	public string EncryptionDecryption(string jsonString)
		{
		string keyword = "1234567";

		string result = "";

		for (int i = 0; i < jsonString.Length; i++)
			{
			result += (char) (jsonString [i] ^ keyword [i % keyword.Length]);
			}
		return result;

		}

	#endregion || --- Encryption Section --- ||

	#region || --- Utility Section --- ||

	public bool DoesFileExist(int slotNumber)
		{
		if (isSavingToJson)
			{
			if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
				{
				return true;
				}
			else
				{
				return false;
				}
			}
		else
			{
			if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
				{
				return false;
				}
			else
				{
				return true;
				}
			}
		}

	public bool IsSlotEmpty(int slotNumber)
		{
		if (DoesFileExist(slotNumber)) return false;
		else return true;
		}

	public void DeselectButton()
		{
		GameObject myEventSystem = GameObject.Find("EventSystem");
		myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
		}

	#endregion || --- Utility Section --- ||
	}