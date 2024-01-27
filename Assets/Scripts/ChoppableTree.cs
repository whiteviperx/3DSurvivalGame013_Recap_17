// Ignore Spelling: Choppable

using System;
using System.Collections;

using UnityEngine;

[RequireComponent (typeof (BoxCollider))]
public class ChoppableTree:MonoBehaviour
	{
	public bool playerInRange;
	public bool canBeChopped;

	public float treeMaxHealth;
	public float treeHealth;

	public Animator animator;

	public void Start()
		{
		treeHealth = treeMaxHealth;
		animator = transform.parent.transform.parent.GetComponent<Animator> ();
		}

	// --- Is player in range of object? --- //
	public void OnTriggerEnter(Collider other)
		{
		if (other.CompareTag ("Player"))
			{
			playerInRange = true;
			}
		}

	public void OnTriggerExit(Collider other)
		{
		if (other.CompareTag ("Player"))
			{
			playerInRange = false;
			}
		}

	public void GetHit()
		{
		StartCoroutine (hit ());
		}

	public IEnumerator hit()
		{
		yield return new WaitForSeconds (0.6f);

		animator.SetTrigger ("shake");

		treeHealth -= 1;

		if (treeHealth <= 0)
			{

			TreeIsDead ();

			}

		}

	private void TreeIsDead()
		{

		Vector3 treePosition = transform.position;

		Destroy (transform.parent.transform.parent.gameObject);
		canBeChopped = false;
		SelectionManager.Instance.selectedTree = null;
		SelectionManager.Instance.chopHolder.gameObject.SetActive (false);

		// --- Change the (0, 0, 0) to change the rotation of logs --- //

		//GameObject brokenTree = Instantiate (Resources.Load<GameObject> ("ChoppedTree"), new Vector3 (treePosition.x, treePosition.y + 1, treePosition.z), Quaternion.Euler (0, 0, 0));
		GameObject brokenTree = Instantiate (Resources.Load<GameObject> ("ChoppedTree"), new Vector3 (treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler (0, 0, 0));
		}

	private void Update()
		{
		if (canBeChopped)
			{
			GlobalState.Instance.resourceHealth = treeHealth;
			GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
			}
		}
	}