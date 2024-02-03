using UnityEngine;
using UnityEngine.UI;

public class WaterBar:MonoBehaviour
	{
	private Slider slider;

	public Text WaterCounter;

	public GameObject playerState;

	private float currentWater, maxWater;

	private void Awake() => slider = GetComponent<Slider> ();

	private void Update()
		{
		/// --- Set current and max Water ---///
		currentWater = playerState.GetComponent<PlayerState> ().currentWaterPercent;
		maxWater = playerState.GetComponent<PlayerState> ().maxWaterPercent;

		var fillValue = currentWater / maxWater; // 0 - 1
		slider.value = fillValue;

		//WaterCounter.text = currentWater + "/" + maxWater; // 100/100
		WaterCounter.text = currentWater + "%";
		}
	}