// Ignore Spelling: Constructable

using System.Collections.Generic;

using UnityEngine;

public class Constructable:MonoBehaviour
	{
	// Validation
	public bool isGrounded;

	public bool isOverlappingItems;

	public bool isValidToBeBuilt;

	public bool detectedGhostMember;

	// Material related
	private Renderer mRenderer;

	// --- Is it a valid place to be built? --- //
	public Material redMaterial;

	public Material greenMaterial;

	public Material defaultMaterial;

	// --- List of Ghosts of the specific item --- //
	public List<GameObject> ghostList = new ();

	public BoxCollider solidCollider;

	private void Start()
		{
		// --- Renderer to change materials --- //
		mRenderer = GetComponent<Renderer> ();

		mRenderer.material = defaultMaterial;
		foreach (Transform child in transform)
			{
			ghostList.Add (child.gameObject);
			}
		}

	private void Update()
		{
		if (isGrounded && isOverlappingItems == false)
			{
			isValidToBeBuilt = true;
			}
		else
			{
			isValidToBeBuilt = false;
			}
		}

	private void OnTriggerEnter(Collider other)
		{
		if (other.CompareTag ("Ground") && gameObject.CompareTag ("activeConstructable"))
			{
			isGrounded = true;
			}

		if (other.CompareTag ("Tree") || other.CompareTag ("Pickable") && gameObject.CompareTag ("activeConstructable"))
			{
			isOverlappingItems = true;
			}

		if (other.gameObject.CompareTag ("ghost") && gameObject.CompareTag ("activeConstructable"))
			{
			detectedGhostMember = true;
			}
		}

	private void OnTriggerExit(Collider other)
		{
		if (other.CompareTag ("Ground") && gameObject.CompareTag ("activeConstructable"))
			{
			isGrounded = false;
			}

		if (other.CompareTag ("Tree") || other.CompareTag ("Pickable") && gameObject.CompareTag ("activeConstructable"))
			{
			isOverlappingItems = false;
			}

		if (other.gameObject.CompareTag ("ghost") && gameObject.CompareTag ("activeConstructable"))
			{
			detectedGhostMember = false;
			}
		}

	public void SetInvalidColor()
		{
		if (mRenderer != null)
			{
			mRenderer.material = redMaterial;
			}
		}

	public void SetValidColor()
		{
		mRenderer.material = greenMaterial;
		}

	public void SetDefaultColor()
		{
		mRenderer.material = defaultMaterial;
		}

	public void ExtractGhostMembers()
		{
		foreach (GameObject item in ghostList)
			{
			item.transform.SetParent (transform.parent, true);

			// sets solid false when finished collide so player doesn't hit them
			item.GetComponent<GhostItem> ().solidCollider.enabled = false;
			item.GetComponent<GhostItem> ().isPlaced = true;
			}
		}
	}