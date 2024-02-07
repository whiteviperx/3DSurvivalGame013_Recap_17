using System.Collections;

using UnityEngine;

public class PlayerState:MonoBehaviour
	{
	public static PlayerState Instance { get; set; }

	// --- Player Health --- //
	public float currentHealth;

	public float maxHealth, maxFood, maxWaterPercent;

	// --- Player Food --- //
	public float currentFood;

	// --- Player Travel for Food --- //
	private float distanceTraveled = 0;

	private Vector3 lastPosition;

	public GameObject playerBody;

	// --- Player Water --- //
	public float currentWaterPercent;

	public bool isWaterActive;

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
		}

	private void Start()
		{
		currentHealth = maxHealth;
		currentFood = maxFood;
		currentWaterPercent = maxWaterPercent;

		StartCoroutine(DecreaseWater());
		}

	private IEnumerator DecreaseWater()
		{
		while (true)
			{
			currentWaterPercent -= 1;
			yield return new WaitForSeconds(60);
			}
		}

	// --- Update is called once per frame --- //
	private void Update()
		{
		distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
		lastPosition = playerBody.transform.position;

		if (distanceTraveled >= 20)
			{
			distanceTraveled = 0;
			currentFood -= 1;
			}
		}

	// --- Set new value if buffed --- //
	public void SetHealth(float newHealth) => currentHealth = newHealth;

	public void SetFood(float newFood) => currentFood = newFood;

	public void SetWater(float newWater) => currentWaterPercent = newWater;
	}