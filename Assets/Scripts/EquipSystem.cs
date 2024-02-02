using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EquipSystem:MonoBehaviour
	{
	public static EquipSystem Instance { get; set; }

	// -- UI -- //
	public GameObject quickSlotsPanel;

	// -- Quickslot Lists -- //
	public List<GameObject> quickSlotsList = new ();

	public List<string> itemList = new ();

	public GameObject numbersHolder;

	public int selectedNumber = -1;

	public GameObject selectedItem;

	public GameObject toolHolder;

	public GameObject selectedItemModel;

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

	private void Start() => PopulateSlotList ();

	private void Update()
		{
		if (Input.GetKeyDown (KeyCode.Alpha1))
			{
			SelectQuickSlot (1);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha2))
			{
			SelectQuickSlot (2);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha3))
			{
			SelectQuickSlot (3);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha4))
			{
			SelectQuickSlot (4);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha5))
			{
			SelectQuickSlot (5);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha6))
			{
			SelectQuickSlot (6);
			}
		else if (Input.GetKeyDown (KeyCode.Alpha7))
			{
			SelectQuickSlot (7);
			}
		}

	private void SelectQuickSlot(int number)
		{
		if (checkIfSlotIsFull (number) == true)
			{
			if (selectedNumber != number)
				{
				selectedNumber = number;

				// --- Unselect previously selected item --- //
				if (selectedItem != null)
					{
					selectedItem.GetComponent<InventoryItem> ().isSelected = false;
					}

				selectedItem = getSelectedItem (number);
				selectedItem.GetComponent<InventoryItem> ().isSelected = true;

				SetEquippedModel (selectedItem);

				// --- Changing the color --- //
				foreach (Transform child in numbersHolder.transform)
					{
					// --- Adjust color as needed --- //
					child.transform.Find ("Text").GetComponent<Text> ().color = Color.gray;
					}

				Text toBeChanged = numbersHolder.transform.transform.Find ("number" + number).transform.Find ("Text").GetComponent<Text> ();
				toBeChanged.color = Color.white;
				}
			else // --- We are trying to select the same slot --- //
				{
				selectedNumber = -1; // null

				// --- Unselect previously selected item --- //
				if (selectedItem != null)
					{
					selectedItem.gameObject.GetComponent<InventoryItem> ().isSelected = false;
					selectedItem = null;
					}

				if (selectedItemModel != null)
					{
					DestroyImmediate (selectedItemModel);
					selectedItemModel = null;
					}

				// --- Changing the color --- //
				foreach (Transform child in numbersHolder.transform)
					{
					// --- Adjust color as needed --- //
					child.transform.Find ("Text").GetComponent<Text> ().color = Color.gray;
					}
				}
			}
		}

	// --- Equip the new Item --- //
	private void SetEquippedModel(GameObject selectedItem)
		{
		if (selectedItemModel != null)
			{
			DestroyImmediate (selectedItemModel.gameObject);
			selectedItemModel = null;
			}

		// --- Position of the new Item --- //
		string selectedItemName = selectedItem.name.Replace ("(Clone)", "");

		// --- Position (0.25f, 0.8f, 0.39f) --- Rotation (0, -100f, -20f) --- //
		GameObject SelectedItemModel = Instantiate (Resources.Load<GameObject> (selectedItemName + "_Model"), new Vector3 (0.25f, 0.8f, 0.39f), Quaternion.Euler (0, -100f, -20f));
		SelectedItemModel.transform.SetParent (toolHolder.transform, false);
		}

	private GameObject getSelectedItem(int slotNumber)
		{
		return quickSlotsList [slotNumber - 1].transform.GetChild (0).gameObject;
		}

	private bool checkIfSlotIsFull(int slotNumber)
		{
		if (quickSlotsList [slotNumber - 1].transform.childCount > 0)
			{
			return true;
			}
		else
			{
			return false;
			}
		}

	private void PopulateSlotList()
		{
		foreach (Transform child in quickSlotsPanel.transform)
			{
			if (child.CompareTag ("QuickSlot"))
				{
				quickSlotsList.Add (child.gameObject);
				}
			}
		}

	public void AddToQuickSlots(GameObject itemToEquip)
		{
		// --- Find next free slot --- //
		var availableSlot = FindNextEmptySlot ();

		// --- Set transform of our object --- //
		itemToEquip.transform.SetParent (availableSlot.transform, false);

		InventorySystem.Instance.ReCalculateList ();
		}

	private GameObject FindNextEmptySlot()
		{
		foreach (GameObject slot in quickSlotsList)
			if (slot.transform.childCount == 0)
				return slot;

		return new ();
		}

	public bool CheckIfFull()
		{
		var counter = 0;

		foreach (GameObject slot in quickSlotsList)
			if (slot.transform.childCount > 0)
				counter += 1;

		if (counter == 7)
			return true;
		else
			return false;
		}
	}