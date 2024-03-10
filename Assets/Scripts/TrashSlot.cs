using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashSlot:MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
	{
	public GameObject trashAlertUI;

	private TMPro.TMP_Text textToModify;

	public Sprite trash_closed;

	public Sprite trash_opened;

	private Image imageComponent;

	private Button YesBTN, NoBTN;

	private GameObject draggedItem
		{
		get
			{
			return DragDrop.itemBeingDragged;
			}
		}

	private GameObject itemToBeDeleted;

	public string itemName
		{
		get
			{
			string name = itemToBeDeleted.name;
			string toRemove = "(Clone)";
			string result = name.Replace(toRemove, "");
			return result;
			}
		}

	private void Start()
		{
		imageComponent = transform.Find("background").GetComponent<Image>();

		textToModify = trashAlertUI.transform.Find("Text").GetComponent<TMPro.TMP_Text>();

		YesBTN = trashAlertUI.transform.Find("Yes").GetComponent<Button>();
		YesBTN.onClick.AddListener(delegate
			{
				DeleteItem();
				});

		NoBTN = trashAlertUI.transform.Find("No").GetComponent<Button>();
		NoBTN.onClick.AddListener(delegate
			{
				CancelDeletion();
				});
		}

	public void OnDrop(PointerEventData eventData)
		{
		//itemToBeDeleted = DragDrop.itemBeingDragged.gameObject;
		if (draggedItem.GetComponent<InventoryItem>().isTrashable == true)
			{
			itemToBeDeleted = draggedItem.gameObject;

			StartCoroutine(notifyBeforeDeletion());
			}
		}

	private IEnumerator notifyBeforeDeletion()
		{
		trashAlertUI.SetActive(true);
		textToModify.text = "Throw away this " + itemName + "?";
		yield return new WaitForSeconds(1f);
		}

	private void CancelDeletion()
		{
		imageComponent.sprite = trash_closed;
		trashAlertUI.SetActive(false);
		}

	private void DeleteItem()
		{
		imageComponent.sprite = trash_closed;
		DestroyImmediate(itemToBeDeleted.gameObject);
		InventorySystem.Instance.ReCalculateList();
		CraftingSystem.Instance.RefreshNeededItems();
		trashAlertUI.SetActive(false);
		}

	public void OnPointerEnter(PointerEventData eventData)
		{
		if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
			{
			imageComponent.sprite = trash_opened;
			}
		}

	public void OnPointerExit(PointerEventData eventData)
		{
		if (draggedItem != null && draggedItem.GetComponent<InventoryItem>().isTrashable == true)
			{
			imageComponent.sprite = trash_closed;
			}
		}
	}