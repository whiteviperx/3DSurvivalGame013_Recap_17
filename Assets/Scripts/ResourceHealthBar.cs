using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar:MonoBehaviour
	{
	private Slider slider;

	private float currentHealth, maxHealth;

	public GameObject globalState;

	private void Awake()
		{
		slider = GetComponent<Slider>();
		}

	private void Update()
		{
		// --- Set Current and Max Resource --- //
		currentHealth = globalState.GetComponent<GlobalState>().resourceHealth;
		maxHealth = globalState.GetComponent<GlobalState>().resourceMaxHealth;

		float fillValue = currentHealth / maxHealth;
		slider.value = fillValue;
		}
	}