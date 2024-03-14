using UnityEngine;

public class Animal:MonoBehaviour
{
	public string animalName;

	public bool playerInRange;

	[SerializeField] private int currentHealth;

	[SerializeField] private int maxHealth;

	[Header("Sounds")]
	[SerializeField] private AudioSource soundChannel;

	[SerializeField] private AudioClip rabbitHitAndScream;

	[SerializeField] private AudioClip rabbitHitAndDie;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		if(currentHealth <= 0)
		{
			soundChannel.PlayOneShot(rabbitHitAndDie);
			Destroy(gameObject);
		}
		else
		{
			soundChannel.PlayOneShot(rabbitHitAndScream);
		}
	}

	//private void PlayDyingSound()
	//{

	//}

	//private void PlayHitSound()
	//{

	//}


	// --- Is player in range of animal? --- //
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			playerInRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			playerInRange = false;
		}
	}
}