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

	public Material redMaterial;
	public Material greenMaterial;
	public Material defaultMaterial;

	public List<GameObject> ghostList = new ();

	public BoxCollider solidCollider; // We need to drag this collider manually into the inspector

	private void Start()
		{
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

		if (other.CompareTag ("Tree") || other.CompareTag ("pickable") && gameObject.CompareTag ("activeConstructable"))
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

		if (other.CompareTag ("Tree") || other.CompareTag ("pickable") && gameObject.CompareTag ("activeConstructable"))
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
			//  item.gameObject.GetComponent<GhostItem>().solidCollider.enabled = false;
			item.GetComponent<GhostItem> ().isPlaced = true;
			}
		}
	}