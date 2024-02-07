using UnityEngine;
using UnityEngine.UI;

public class HealthBar:MonoBehaviour
	{
	private Slider slider;

	public Text healthCounter;

	public GameObject playerState;

	private float currentHealth, maxHealth;

	private void Awake() => slider = GetComponent<Slider>();

	private void Update()
		{
		// --- Set current and max health --- //
		currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
		maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

		var fillValue = currentHealth / maxHealth; // 0 - 1
		slider.value = fillValue;

		healthCounter.text = currentHealth + "/" + maxHealth; // 100/100
		}
	}