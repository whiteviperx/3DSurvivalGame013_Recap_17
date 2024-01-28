using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	// --- Singleton --- //
	public static SoundManager Instance	{get; set;}

	// --- Sound FX --- //
	[Header ("Item Sounds")]
	public AudioSource dropItemSound;
	public AudioSource pickupItemSound;
	
	[Header ("Tool Sounds")]
	public AudioSource toolSwingSound;
	public AudioSource chopSound;
	public AudioSource craftingSound;

	[Header ("Movement Sounds")]
	public AudioSource grassWalkSound;

	[Header ("Death Sounds")]
	public AudioSource killAnimalSound;


	// --- Music --- //
	[Header ("Music")]
	public AudioSource startingZoneBGMusic;




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

	public void PlaySound(AudioSource soundToPlay)
		{
			if (!soundToPlay.isPlaying)
			{
			soundToPlay.Play ();
			}
		}
	}
