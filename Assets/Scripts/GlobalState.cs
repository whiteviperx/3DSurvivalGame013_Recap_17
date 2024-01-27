using UnityEngine;

public class GlobalState:MonoBehaviour
	{
	public static GlobalState Instance { get; set; }

	public float resourceHealth;
	public float resourceMaxHealth;

	public void Awake()
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
	}