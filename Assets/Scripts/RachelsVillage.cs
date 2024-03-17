using UnityEngine;

public class RachelsVillage:MonoBehaviour
	{
	public Checkpoint reachVillage_Rachel;

	private void OnTriggerEnter(Collider other)
		{
		if (other.CompareTag("Player"))
			{
			reachVillage_Rachel.isCompleted = true;
			}
		}
	}