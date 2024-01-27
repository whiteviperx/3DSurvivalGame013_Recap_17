using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot:MonoBehaviour, IDropHandler
	{
	// --- Check if this item slot is already occupied --- //
	public GameObject Item
		{
		get
			{
			if (transform.childCount > 0)
				{
				return transform.GetChild (0).gameObject;
				}

			return null;
			}
		}

	public void OnDrop(PointerEventData eventData)
		{
		Debug.Log ("OnDrop");

		// --- If there is not item already then set our item --- //
		if (!Item)
			{
			DragDrop.itemBeingDragged.transform.SetParent (transform);
			DragDrop.itemBeingDragged.transform.localPosition = new Vector2 (0, 0);

			if (transform.CompareTag ("QuickSlot") == false)
				{
				DragDrop.itemBeingDragged.GetComponent<InventoryItem> ().isInsideQuickSlot = false;
				InventorySystem.Instance.ReCalculateList ();
				}

			if (transform.CompareTag ("QuickSlot"))
				{
				DragDrop.itemBeingDragged.GetComponent<InventoryItem> ().isInsideQuickSlot = true;
				InventorySystem.Instance.ReCalculateList ();
				}
			}
		}
	}