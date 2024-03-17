// Ignore Spelling: Interactable

using UnityEngine;

public class InteractableObject:MonoBehaviour
	{
	public bool playerInRange;

	public string ItemName;

	public string GetItemName() => ItemName;

	private void Update()
		{
		// --- Add to inventory, only if click the mouse, on target, and the item we are picking up is the selected object --- //
		if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)

			{
			// ---  If the inventory is NOT full --- //
			if (InventorySystem.Instance.CheckSlotsAvailable(1))
				{
				InventorySystem.Instance.AddToInventory(ItemName);

				InventorySystem.Instance.itemsPickedup.Add(gameObject.name);

				Debug.Log("Item added to inventory");

				Destroy(gameObject);
				}
			else
				{
				Debug.Log("Inventory is full");
				}
			}
		}

	// --- Is player in range of object? --- //
	private void OnTriggerEnter(Collider other)
		{
		if (other.CompareTag("Player"))
			{
			playerInRange = true;
			}
		}

	private void OnTriggerExit(Collider other)
		{
		if (other.CompareTag("Player"))
			{
			playerInRange = false;
			}
		}
	}