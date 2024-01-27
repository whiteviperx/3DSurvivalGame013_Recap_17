using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem:MonoBehaviour
	{
	public GameObject craftingScreenUI;
	public GameObject toolsScreenUI;
	//public GameObject toolsScreenUI, survivalScreenUI, refineScreenUI, buildingScreenUI;

	// --- Get list from inventory --- //
	public List<string> inventoryItemList = new ();

	public static CraftingSystem Instance
		{
		get; set;
		}

	// --- Category Buttons --- //
	private Button toolsBTN;

	//private Button toolsBTN, survivalBTN, refineBTN, buildingBTN;

	// --- Craft Buttons --- //
	private Button craftAxeBTN;

	//private Button craftAxeBTN, craftPlankBTN, craftWallBTN, craftFoundationBTN, craftWindowBTN, craftDoorBTN;

	// --- Requirement Text --- //
	private Text AxeReq1, AxeReq2;

	//private TMP_Text AxeReq1, AxeReq2, PlankReq1, WallReq1, FoundationReq1, WindowReq1, DoorReq1;

	public bool isOpen;

	// --- All Blueprint --- //

	// (Itemname, req num, item1, item1 qty needed, item2, item2 qty needed)
	public Blueprint AxeBLP = new ("Axe", 2, "Stone", 3, "Stick", 3);

	//public ItemBluePrint AxeBLP;
	//public ItemBluePrint PlankBLP;
	//public ItemBluePrint WallBLP;
	//public ItemBluePrint FoundationBLP;
	//public ItemBluePrint WindowBLP;
	//public ItemBluePrint DoorBLP;

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

		// --- STONE AXE --- //
		//GameObject gO1 = new("AxeBLP");
		//AxeBLP = gO1.AddComponent<Blueprint>();
		//AxeBLP = gO1.AddComponent<ItemBluePrint>();
		//AxeBLP.itemName = "Axe";
		//AxeBLP.numberOfItemsToProduce = 1;
		//AxeBLP.numOfRequirements = 2;
		//AxeBLP.Req1 = "Stone";
		//AxeBLP.Req1amount = 3;
		//AxeBLP.Req2 = "Stick";
		//AxeBLP.Req2amount = 3;
		}

	private void Start()
		{
		isOpen = false;

		toolsBTN = craftingScreenUI.transform.Find ("ToolsButton").GetComponent<Button> ();

		toolsBTN.onClick.AddListener (delegate
			{
				OpenToolsCategory ();
				});

		//  --- PickAxe --- //

		AxeReq1 = toolsScreenUI.transform.Find ("Axe").transform.Find ("req1").GetComponent<Text> ();
		AxeReq2 = toolsScreenUI.transform.Find ("Axe").transform.Find ("req2").GetComponent<Text> ();

		craftAxeBTN = toolsScreenUI.transform.Find ("Axe").transform.Find ("CraftButton").GetComponent<Button> ();
		craftAxeBTN.onClick.AddListener (delegate
			{ CraftAnyItem (AxeBLP); });
		}

	// --- Tool Window --- //
	private void OpenToolsCategory()
		{
		craftingScreenUI.SetActive (false);
		toolsScreenUI.SetActive (true);
		}

	// --- Crafting instructions --- //
	private void CraftAnyItem(Blueprint blueprintToCraft)
		{
		// --- Add item into inventory --- //
		InventorySystem.Instance.AddToInventory (blueprintToCraft.itemName);

		// --- Remove resources from inventory --- //
		if (blueprintToCraft.numOfRequirements == 1)
			{
			InventorySystem.Instance.RemoveItem (blueprintToCraft.Req1, blueprintToCraft.Req1amount);
			}
		else if (blueprintToCraft.numOfRequirements == 2)
			{
			InventorySystem.Instance.RemoveItem (blueprintToCraft.Req1, blueprintToCraft.Req1amount);
			InventorySystem.Instance.RemoveItem (blueprintToCraft.Req2, blueprintToCraft.Req2amount);
			}

		// --- Refresh list --- //
		// InventorySystem.Instance.ReCalculateList ()

		// else if (blueprintToCraft.numOfRequirements == 3)
		//	{
		//	InventorySystem.Instance.RemoveItem (blueprintToCraft.Req1, blueprintToCraft.Req1amount);
		//	InventorySystem.Instance.RemoveItem (blueprintToCraft.Req2, blueprintToCraft.Req2amount);
		// InventorySystem.Instance.RemoveItem (blueprintToCraft.Req3, blueprintToCraft.Req2amount);
		// }

		// --- Refresh list --- //
		StartCoroutine (Calculate ());

		RefreshNeededItems ();
		}

	public IEnumerator Calculate()
		{
		// yield return new WaitForSeconds(1f);
		yield return 0; // --- So there is no delay --- //

		InventorySystem.Instance.ReCalculateList ();

		RefreshNeededItems ();
		}

	// --- Update is called once per frame --- //
	private void Update()
		{
		// RefreshNeededItems();

		// --- Lock and unlock cursor if crafting window open or closed --- //

		// --- Pressing "C" opens & closes crafting screen --- //
		if (Input.GetKeyDown (KeyCode.C) && !isOpen)
			{
			Debug.Log ("C is pressed");

			craftingScreenUI.SetActive (true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection ();
			SelectionManager.Instance.GetComponent<SelectionManager> ().enabled = false;

			isOpen = true;
			}
		else if (Input.GetKeyDown (KeyCode.C) && isOpen)
			{
			craftingScreenUI.SetActive (false);
			toolsScreenUI.SetActive (false);

			if (!InventorySystem.Instance.isOpen)
				{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				SelectionManager.Instance.EnableSelection ();
				SelectionManager.Instance.GetComponent<SelectionManager> ().enabled = true;
				}

			isOpen = false;
			}
		}

	// --- Items in stock for crafting --- //
	public void RefreshNeededItems()
		{
		var stone_count = 0;
		var stick_count = 0;

		inventoryItemList = InventorySystem.Instance.itemList;

		foreach (string itemName in inventoryItemList)
			{
			switch (itemName)
				{
				case "Stone":
					stone_count += 1;
					break;

				case "Stick":
					stick_count += 1;
					break;
				}
			}

		// --- AXE --- //

		AxeReq1.text = "3 Stone [" + stone_count + "]";
		AxeReq2.text = "3 Stick [" + stick_count + "]";

		if (stone_count >= 3 && stick_count >= 3)
			{
			craftAxeBTN.gameObject.SetActive (true);
			}
		else
			{
			craftAxeBTN.gameObject.SetActive (false);
			}
		}
	}