using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public static SettingsManager Instance { get; set; }

	public Button backBTN;

	public Slider masterSlider;
	public GameObject masterValue;

	public Slider musicSlider;
	public GameObject musicValue;

	public Slider effectsSlider;
	public GameObject effectsValue;

	private void Start()
		{

		backBTN.onClick.AddListener (() => {

			MainMenuSaveManager.Instance.SaveVolumeSettings (musicSlider.value, effectsSlider.value, masterSlider.value);
			print ("Saved to prefs");
		
		});
		 

		}


	private void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy (gameObject);
			}
		else
			{
			Instance = this;
			}
		}

	private void Update()
		{

		masterValue.GetComponent<TextMeshProUGUI> ().text = "" + masterSlider.value + "";
		musicValue.GetComponent<TextMeshProUGUI> ().text = "" + musicSlider.value + "";
		effectsValue.GetComponent<TextMeshProUGUI> ().text = "" + effectsSlider.value + "";
		}


	}
