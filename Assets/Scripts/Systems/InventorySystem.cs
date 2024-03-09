using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class InventorySystem:MonoBehaviour
	{
	[Header("Item Info Panel")]
	public GameObject ItemInfoUI;

	public static InventorySystem Instance { get; set; }

	public GameObject inventoryScreenUI;

	private GameObject itemToAdd;

	private GameObject whatSlotToEquip;

	// --- Slot list that contains the slots themselves --- //
	public List<GameObject> slotList = new List<GameObject>();

	// --- Here we actually store the names of the actual items --- //
	public List<string> itemList = new();

	// --- Is inventory screen open or closed --- //
	public bool isOpen;

	// --- Checks to see if inventory is full --- //
	//public bool isFull;

	// --- Pickup Popup --- //
	[Header("Pickup Alert")]
	public GameObject pickupAlert;

	public Text pickupName;

	public Image pickupImage;

	public List<string> itemsPickedup;

	private void Awake()
		{
		if (Instance != null && Instance != this)
			{
			Destroy(gameObject);
			}
		else
			{
			Instance = this;
			}
		}

	private void Start()
		{
		isOpen = false;

		//isFull = false;

		// --- Adding the slots to the inventory list (Calling it) --- //
		PopulateSlotList();

		// --- Make cursor invisible when starting game --- //
		Cursor.visible = false;
		}

	// --- Adding the slots to the inventory list (Creating it) --- //
	private void PopulateSlotList()
		{
		// --- Searches for all the slot children in the inventory screen UI --- //
		foreach (Transform child in inventoryScreenUI.transform)
			if (child.CompareTag("Slot"))
				{
				slotList.Add(child.gameObject);
				}
		}

	// --- Open and close Inventory Screen --- //
	private void Update()
		{
		// --- Pressing "I" opens & closes inventory screen --- //
		if (Input.GetKeyDown(KeyCode.I) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
			{
			Debug.Log("i is pressed");

			inventoryScreenUI.SetActive(true);

			inventoryScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection();
			SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

			isOpen = true;
			}
		else if (Input.GetKeyDown(KeyCode.I) && isOpen)
			{
			inventoryScreenUI.SetActive(false);

			if (!CraftingSystem.Instance.isOpen)
				{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				SelectionManager.Instance.EnableSelection();
				SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
				}

			isOpen = false;
			}
		}

	// --- Adding items to inventory when we pick up an item --- //
	public void AddToInventory(string itemName)
		{
		whatSlotToEquip = FindNextEmptySlot();

		itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);

		itemToAdd.transform.SetParent(whatSlotToEquip.transform);

		itemList.Add(itemName);

		Debug.Log("added to inventory from inentorysystem.cs");

		SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
		TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

		// -- For debugging we dont want want to use the save manager -- //

		//  if (SaveManager.Instance.isLoading == false)
		//	{
		//		SoundManager.Instance.PlaySound(SoundManager.Instance.pickupItemSound);
		//		TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);
		//	}

		ReCalculateList();
		CraftingSystem.Instance.RefreshNeededItems();

		QuestManager.Instance.RefreshTrackerList();
		}

	// --- Trigger the popup when an item is picked up --- //
	private void TriggerPickupPopUp(string itemName, Sprite itemSprite)
		{
		// --- Pickup alert to appear --- //
		pickupAlert.SetActive(true);

		// --- Change the text and image --- //
		pickupName.text = itemName;
		pickupImage.sprite = itemSprite;
		}

	// --- Find next empty slot in inventory --- //
	private GameObject FindNextEmptySlot()
		{
		foreach (GameObject slot in slotList)
			if (slot.transform.childCount == 0)
				return slot;

		return new();
		}

	// --- Check if slots available --- //
	public bool CheckSlotsAvailable(int emptyNeeded)
		{
		var emptySlot = 0;

		foreach (GameObject slot in slotList)
			if (slot.transform.childCount <= 0)
				emptySlot += 1;

		if (emptySlot >= emptyNeeded)
			return true;
		else
			return false;
		}

	// --- Remove used item from inventory --- //
	public void RemoveItem(string nameToRemove, int amountToRemove)
		{
		var counter = amountToRemove;

		for (var i = slotList.Count - 1; i >= 0; i--)
			{
			if (slotList [i].transform.childCount > 0)
				{
				if (slotList [i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
					{
					DestroyImmediate(slotList [i].transform.GetChild(0).gameObject);

					counter -= 1;
					}
				}
			}

		ReCalculateList();
		CraftingSystem.Instance.RefreshNeededItems();
		QuestManager.Instance.RefreshTrackerList();
		}

	// --- Recalculate remaining items in inventory --- //
	public void ReCalculateList()
		{
		itemList.Clear();

		foreach (GameObject slot in slotList)
			{
			if (slot.transform.childCount > 0)
				{
				var name = slot.transform.GetChild(0).name; // Item (Clone)

				var str2 = "(Clone)";

				var result = name.Replace(str2, string.Empty);

				itemList.Add(result);
				}
			}
		}

	public int CheckItemAmount(string name)
		{
		int itemCounter = 0;
		foreach (string item in itemList)
			{
			if (item == name)
				{
				itemCounter++;
				}
			}
		return itemCounter;
		}
	}