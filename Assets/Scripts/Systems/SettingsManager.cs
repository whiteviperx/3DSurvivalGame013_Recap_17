using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveManager;

public class SettingsManager:MonoBehaviour
	{
	public static SettingsManager Instance { get; set; }

	public Button backBTN;

	public Slider masterSlider, musicSlider, effectsSlider;

	public GameObject masterValue, musicValue, effectsValue;

	void Start()
		{
		backBTN.onClick
		    .AddListener(() =>
		{
			SaveManager.Instance.SaveVolumeSettings(musicSlider.value, effectsSlider.value, masterSlider.value);
		});

		StartCoroutine(LoadAndApplySettings());
		}

	IEnumerator LoadAndApplySettings()
		{
		LoadAndSetVolume();

		// Load Graphics Settings

		// Load Chopped Trees

		// Load Key Bindings

		// Load ????
		yield return new WaitForSeconds(0.1f);
		}

	void LoadAndSetVolume()
		{
		VolumeSettings volumeSettings = SaveManager.Instance.LoadVolumeSettings();

		masterSlider.value = volumeSettings.master;
		musicSlider.value = volumeSettings.music;
		effectsSlider.value = volumeSettings.effects;

		Debug.Log("Volume Settings are Loaded");
		}

	void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			}
		else
			{
			Instance = this;
			}
		}

	void Update()
		{
		masterValue.GetComponent<TextMeshProUGUI>().text = "" + masterSlider.value + "";
		musicValue.GetComponent<TextMeshProUGUI>().text = "" + musicSlider.value + "";
		effectsValue.GetComponent<TextMeshProUGUI>().text = "" + effectsSlider.value + "";
		}
	}