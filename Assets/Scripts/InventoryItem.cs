// Ignore Spelling: Trashable Equippable

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem:MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
	{
	// --- Is this item trashable --- //
	public bool isTrashable;

	// --- Item Info UI --- //
	private GameObject itemInfoUI;

	private TMPro.TMP_Text itemInfoUI_ItemName;

	private TMPro.TMP_Text itemInfoUI_ItemDescription;

	private TMPro.TMP_Text itemInfoUI_ItemFunctionality;

	public string thisName, thisDescription, thisFunctionality;

	// --- Consumption --- //
	private GameObject itemPendingConsumption;

	public bool isConsumable;

	// --- Consumption Effects --- //
	public float healthEffect;

	public float foodEffect;

	public float waterEffect;

	// --- Equipping --- //
	public bool isEquippable;

	private GameObject itemPendingEquipping;

	public bool isInsideQuickSlot;

	public bool isSelected;

	public bool isUsable;

	private void Start()
		{
		itemInfoUI = InventorySystem.Instance.ItemInfoUI;
		itemInfoUI_ItemName = itemInfoUI.transform.Find("itemName").GetComponent<TMPro.TMP_Text>();
		itemInfoUI_ItemDescription = itemInfoUI.transform.Find("itemDescription").GetComponent<TMPro.TMP_Text>();
		itemInfoUI_ItemFunctionality = itemInfoUI.transform.Find("itemFunctionality").GetComponent<TMPro.TMP_Text>();
		}

	private void Update()
		{
		if (isSelected)
			{
			gameObject.GetComponent<DragDrop>().enabled = false;
			}
		else
			{
			gameObject.GetComponent<DragDrop>().enabled = true;
			}
		}

	// --- Triggered when the mouse enters into the area of the item that has this script --- //
	public void OnPointerEnter(PointerEventData eventData)
		{
		itemInfoUI.SetActive(true);
		itemInfoUI_ItemName.text = thisName;
		itemInfoUI_ItemDescription.text = thisDescription;
		itemInfoUI_ItemFunctionality.text = thisFunctionality;
		}

	// --- Triggered when the mouse exits the area of the item that has this script --- //
	public void OnPointerExit(PointerEventData eventData)
		{
		itemInfoUI.SetActive(false);
		}

	// --- Triggered when the mouse is clicked over the item that has this script --- //
	public void OnPointerDown(PointerEventData eventData)
		{
		// --- Right Mouse Button Click on --- //
		if (eventData.button == PointerEventData.InputButton.Right)
			{
			if (isConsumable)
				{
				// --- Setting this specific gameobject to be the item we want to destroy later --- //
				itemPendingConsumption = gameObject;
				ConsumingFunction(healthEffect, foodEffect, waterEffect);
				}

			if (isEquippable && isInsideQuickSlot == false && EquipSystem.Instance.CheckIfFull() == false)
				{
				EquipSystem.Instance.AddToQuickSlots(gameObject);
				isInsideQuickSlot = true;
				}
			if (isUsable)
				{
				ConstructionManager.Instance.itemToBeDestroyed = gameObject;

				gameObject.SetActive(false); // Even though we can't see the item

				UseItem();
				}
			}
		}

	// --- Triggered when the mouse button is released over the item that has this script --- //
	public void OnPointerUp(PointerEventData eventData)
		{
		if (eventData.button == PointerEventData.InputButton.Right)
			{
			if (isConsumable && itemPendingConsumption == gameObject)
				{
				DestroyImmediate(gameObject);
				InventorySystem.Instance.ReCalculateList();
				CraftingSystem.Instance.RefreshNeededItems();
				}
			}
		}

	private void UseItem()
		{
		itemInfoUI.SetActive(false);

		InventorySystem.Instance.isOpen = false;
		InventorySystem.Instance.inventoryScreenUI.SetActive(false);

		CraftingSystem.Instance.isOpen = false;
		CraftingSystem.Instance.craftingScreenUI.SetActive(false);
		CraftingSystem.Instance.toolsScreenUI.SetActive(false);
		CraftingSystem.Instance.survivalScreenUI.SetActive(false);
		CraftingSystem.Instance.refineScreenUI.SetActive(false);
		CraftingSystem.Instance.constructionScreenUI.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		SelectionManager.Instance.EnableSelection();
		SelectionManager.Instance.enabled = true;

		switch (gameObject.name)
			{
			case "Foundation(Clone)":
				ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel");
				break;

			case "Foundation":
				ConstructionManager.Instance.ActivateConstructionPlacement("FoundationModel"); // For testing
				break;

			case "Wall(Clone)":
				ConstructionManager.Instance.ActivateConstructionPlacement("WallModel");
				break;

			case "Wall":
				ConstructionManager.Instance.ActivateConstructionPlacement("WallModel"); // For testing
				break;

			default:

				// do nothing
				break;
			}
		}

	private void ConsumingFunction(float healthEffect, float foodEffect, float waterEffect)
		{
		itemInfoUI.SetActive(false);

		HealthEffectCalculation(healthEffect);

		FoodEffectCalculation(foodEffect);

		WaterEffectCalculation(waterEffect);
		}

	private static void HealthEffectCalculation(float healthEffect)
		{
		// --- Health --- //

		float healthBeforeConsumption = PlayerState.Instance.currentHealth;
		float maxHealth = PlayerState.Instance.maxHealth;

		if (healthEffect != 0)
			{
			if ((healthBeforeConsumption + healthEffect) > maxHealth)
				{
				PlayerState.Instance.SetHealth(maxHealth);
				}
			else
				{
				PlayerState.Instance.SetHealth(healthBeforeConsumption + healthEffect);
				}
			}
		}

	private static void FoodEffectCalculation(float foodEffect)
		{
		// --- Food --- //

		float foodBeforeConsumption = PlayerState.Instance.currentFood;
		float maxFood = PlayerState.Instance.maxFood;

		if (foodEffect != 0)
			{
			if ((foodBeforeConsumption + foodEffect) > maxFood)
				{
				PlayerState.Instance.SetFood(maxFood);
				}
			else
				{
				PlayerState.Instance.SetFood(foodBeforeConsumption + foodEffect);
				}
			}
		}

	private static void WaterEffectCalculation(float waterEffect)
		{
		// --- Water --- //

		float waterBeforeConsumption = PlayerState.Instance.currentWaterPercent;
		float maxWater = PlayerState.Instance.maxWaterPercent;

		if (waterEffect != 0)
			{
			if ((waterBeforeConsumption + waterEffect) > maxWater)
				{
				PlayerState.Instance.SetWater(maxWater);
				}
			else
				{
				PlayerState.Instance.SetWater(waterBeforeConsumption + waterEffect);
				}
			}
		}
	}