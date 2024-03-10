using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem:MonoBehaviour
	{
	public GameObject craftingScreenUI, toolsScreenUI, survivalScreenUI, refineScreenUI, constructionScreenUI;

	// --- Get list from inventory --- //
	public List<string> inventoryItemList = new();

	// --- Category Buttons --- //

	private Button toolsBTN, survivalBTN, refineBTN, constructionBTN;

	// --- Craft Buttons --- //
	private Button craftAxeBTN;

	private Button craftPlankBTN;

	private Button craftWallBTN;

	private Button craftFoundationBTN;

	// craftWindowBTN, craftDoorBTN;

	// --- Requirement Text --- //
	private TMPro.TMP_Text AxeReq1, AxeReq2, PlankReq1, WallReq1, FoundationReq1;

	public bool isOpen;

	// --- All Blueprint --- //

	// (Itemname, items produced, req num, item1, item1 qty needed, item2, item2 qty needed)
	public Blueprint AxeBLP = new("Axe", 1, 2, "Stone", 3, "Stick", 3);

	public Blueprint PlankBLP = new("Plank", 2, 1, "Log", 1, "", 0);

	public Blueprint WallBLP = new("Wall", 1, 1, "Plank", 2, "", 0);

	public Blueprint FoundationBLP = new("Foundation", 1, 1, "Plank", 4, "", 0);

	//public ItemBluePrint WindowBLP;
	//public ItemBluePrint DoorBLP;

	public static CraftingSystem Instance
		{
		get; set;
		}

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

		toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
		toolsBTN.onClick.AddListener(delegate { OpenToolsCategory(); });

		survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
		survivalBTN.onClick.AddListener(delegate { OpenSurvivalCategory(); });

		refineBTN = craftingScreenUI.transform.Find("RefineButton").GetComponent<Button>();
		refineBTN.onClick.AddListener(delegate { OpenRefineCategory(); });

		constructionBTN = craftingScreenUI.transform.Find("ConstructionButton").GetComponent<Button>();
		constructionBTN.onClick.AddListener(delegate { OpenConstructionCategory(); });

		//  --- PickAxe --- //

		AxeReq1 = toolsScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<TMPro.TMP_Text>();
		AxeReq2 = toolsScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<TMPro.TMP_Text>();

		craftAxeBTN = toolsScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
		craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });

		//  --- Plank --- //

		PlankReq1 = refineScreenUI.transform.Find("Plank").transform.Find("req1").GetComponent<TMPro.TMP_Text>();

		craftPlankBTN = refineScreenUI.transform.Find("Plank").transform.Find("CraftButton").GetComponent<Button>();
		craftPlankBTN.onClick.AddListener(delegate { CraftAnyItem(PlankBLP); });

		//  --- Wall --- //

		WallReq1 = constructionScreenUI.transform.Find("Wall").transform.Find("req1").GetComponent<TMPro.TMP_Text>();

		craftWallBTN = constructionScreenUI.transform.Find("Wall").transform.Find("CraftButton").GetComponent<Button>();
		craftWallBTN.onClick.AddListener(delegate { CraftAnyItem(WallBLP); });

		//  --- Foundation --- //

		FoundationReq1 = constructionScreenUI.transform.Find("Foundation").transform.Find("req1").GetComponent<TMPro.TMP_Text>();

		craftFoundationBTN = constructionScreenUI.transform.Find("Foundation").transform.Find("CraftButton").GetComponent<Button>();
		craftFoundationBTN.onClick.AddListener(delegate { CraftAnyItem(FoundationBLP); });
		}

	// --- Tool Window --- //
	private void OpenToolsCategory()
		{
		craftingScreenUI.SetActive(false);
		survivalScreenUI.SetActive(false);
		refineScreenUI.SetActive(false);
		constructionScreenUI.SetActive(false);

		toolsScreenUI.SetActive(true);
		}

	// --- Survival Window --- //
	private void OpenSurvivalCategory()
		{
		craftingScreenUI.SetActive(false);
		toolsScreenUI.SetActive(false);
		refineScreenUI.SetActive(false);
		constructionScreenUI.SetActive(false);

		survivalScreenUI.SetActive(true);
		}

	// --- Refine Window --- //
	private void OpenRefineCategory()
		{
		craftingScreenUI.SetActive(false);
		toolsScreenUI.SetActive(false);
		survivalScreenUI.SetActive(false);
		constructionScreenUI.SetActive(false);

		refineScreenUI.SetActive(true);
		}

	// --- Construction Window --- //
	private void OpenConstructionCategory()
		{
		craftingScreenUI.SetActive(false);
		toolsScreenUI.SetActive(false);
		survivalScreenUI.SetActive(false);
		refineScreenUI.SetActive(false);

		constructionScreenUI.SetActive(true);
		}

	// --- Crafting instructions --- //
	private void CraftAnyItem(Blueprint blueprintToCraft)
		{
		SoundManager.Instance.PlaySound(SoundManager.Instance.craftingSound);

		// --- Produce the amount if items according to loop --- //
		for (var i = 0; i < blueprintToCraft.numberOfItemsToProduce; i++)
			{
			InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);
			}

		// --- Remove resources from inventory --- //
		if (blueprintToCraft.numOfRequirements == 1)
			{
			InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
			}
		else if (blueprintToCraft.numOfRequirements == 2)
			{
			InventorySystem.Instance.RemoveItem(blueprintToCraft.Req1, blueprintToCraft.Req1amount);
			InventorySystem.Instance.RemoveItem(blueprintToCraft.Req2, blueprintToCraft.Req2amount);
			}

		// --- Refresh list --- //
		StartCoroutine(Calculate());

		RefreshNeededItems();
		}

	public IEnumerator Calculate()
		{
		yield return 0; // --- So there is no delay --- //

		InventorySystem.Instance.ReCalculateList();

		RefreshNeededItems();
		}

	// --- Update is called once per frame --- //
	private void Update()
		{
		// RefreshNeededItems();

		// --- Lock and unlock cursor if crafting window open or closed --- //

		// --- Pressing "C" opens & closes crafting screen --- //
		if (Input.GetKeyDown(KeyCode.C) && !isOpen && !ConstructionManager.Instance.inConstructionMode)
			{
			Debug.Log("C is pressed");

			craftingScreenUI.SetActive(true);

			craftingScreenUI.GetComponentInParent<Canvas>().sortingOrder = MenuManager.Instance.SetAsFront();

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection();
			SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

			isOpen = true;
			}
		else if (Input.GetKeyDown(KeyCode.C) && isOpen)
			{
			craftingScreenUI.SetActive(false);
			toolsScreenUI.SetActive(false);
			survivalScreenUI.SetActive(false);
			refineScreenUI.SetActive(false);
			constructionScreenUI.SetActive(false);

			if (!InventorySystem.Instance.isOpen)
				{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				SelectionManager.Instance.EnableSelection();
				SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
				}

			isOpen = false;
			}
		}

	// --- Items in stock for crafting --- //
	public void RefreshNeededItems()
		{
		var stone_count = 0;
		var stick_count = 0;
		var log_count = 0;
		var plank_count = 0;

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

				case "Log":
					log_count += 1;
					break;

				case "Plank":
					plank_count += 1;
					break;
				}
			}

		// --- AXE --- //

		AxeReq1.text = "3 Stone [" + stone_count + "]";
		AxeReq2.text = "3 Stick [" + stick_count + "]";

		if (stone_count >= 3 && stick_count >= 3 && InventorySystem.Instance.CheckSlotsAvailable(1))
			{
			craftAxeBTN.gameObject.SetActive(true);
			}
		else
			{
			craftAxeBTN.gameObject.SetActive(false);
			}

		// --- PLANK --- //

		PlankReq1.text = "1 Log [" + log_count + "]";

		if (log_count >= 1 && InventorySystem.Instance.CheckSlotsAvailable(2))
			{
			craftPlankBTN.gameObject.SetActive(true);
			}
		else
			{
			craftPlankBTN.gameObject.SetActive(false);
			}

		// --- Foundation --- //

		FoundationReq1.text = "4 Plank [" + plank_count + "]";

		if (plank_count >= 4 && InventorySystem.Instance.CheckSlotsAvailable(1))
			{
			craftFoundationBTN.gameObject.SetActive(true);
			}
		else
			{
			craftFoundationBTN.gameObject.SetActive(false);
			}

		// --- Wall --- //

		WallReq1.text = "2 Plank [" + plank_count + "]";

		if (plank_count >= 2 && InventorySystem.Instance.CheckSlotsAvailable(1))
			{
			craftWallBTN.gameObject.SetActive(true);
			}
		else
			{
			craftWallBTN.gameObject.SetActive(false);
			}
		}
	}