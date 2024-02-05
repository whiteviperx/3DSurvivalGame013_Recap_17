using UnityEngine;
using UnityEngine.UI;

public class FoodBar:MonoBehaviour
	{
	private Slider slider;

	public Text FoodCounter;

	public GameObject playerState;

	private float currentFood, maxFood;

	private void Awake() => slider = GetComponent<Slider>();

	private void Update()
		{
		// --- Set current and max Food --- //
		currentFood = playerState.GetComponent<PlayerState>().currentFood;
		maxFood = playerState.GetComponent<PlayerState>().maxFood;

		var fillValue = currentFood / maxFood; // 0 - 1
		slider.value = fillValue;

		FoodCounter.text = currentFood + "/" + maxFood; // 100/100
		}
	}