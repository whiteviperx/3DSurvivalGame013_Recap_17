using System.Collections;

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

	private Animator animator;
	public bool isDead;

	[SerializeField] ParticleSystem bloodSplashParticles;
	[SerializeField] GameObject bloodPuddle;


	private enum AnimalType
	{
		Rabbit,

		Lion,

		Snake
	}

	[SerializeField] AnimalType thisAnimalType;

	private void Start()
	{
		currentHealth = maxHealth;

		animator = GetComponent<Animator>();
	}

	public void TakeDamage(int damage)
	{
		if(isDead == false)
		{
			currentHealth -= damage;

			bloodSplashParticles.Play();


			if(currentHealth <= 0)
			{
				PlayDyingSound();

				animator.SetTrigger("Die");
				GetComponent<AI_Movement>().enabled = false;

				StartCoroutine(PuddleDelay());

				isDead = true;
			}
			else
			{
				PlayHitSound();
			} 
		}
	}

	IEnumerator PuddleDelay()
	{
		yield return new WaitForSeconds(1f);
		bloodPuddle.SetActive(true);
	}

	private void PlayDyingSound()
	{
		switch(thisAnimalType)
		{
			case AnimalType.Rabbit:
				soundChannel.PlayOneShot(rabbitHitAndDie);
				break;
			case AnimalType.Lion:

				// soundChannel.PlayOneShot(); // Lion Sound Clip
				break;
			default:
				break;

				// soundChannel.PlayOneShot(rabbitHitAndDie);
		}
	}

	private void PlayHitSound()
	{
		switch(thisAnimalType)
		{
			case AnimalType.Rabbit:
				soundChannel.PlayOneShot(rabbitHitAndScream);
				break;
			case AnimalType.Lion:

				// soundChannel.PlayOneShot(); // Lion Scream Sound Clip
				break;
			default:
				break;

				// soundChannel.PlayOneShot(rabbitHitAndDie);
		}
	}

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