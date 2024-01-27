using UnityEngine;

public class DEMO_SoundManager:MonoBehaviour
	{
	// Use this for initialization
	private void Start()
		{
		}

	// Update is called once per frame
	private void Update()
		{
		}

	public void PlayClickSound()
		{
		this.GetComponent<AudioSource> ().Play ();
		}
	}