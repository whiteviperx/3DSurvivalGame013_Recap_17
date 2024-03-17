using UnityEngine;

public class MenuManager:MonoBehaviour
	{
	public static MenuManager Instance { get; set; }

	public GameObject menuCanvas, uiCanvas, saveMenu, settingsMenu, menu;

	public bool isMenuOpen;

	public int currentFront = 0;

	public int SetAsFront()
		{
		return currentFront;
		}


	void Awake()
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

	void Update()
		{
		if (Input.GetKeyDown(KeyCode.M) && !isMenuOpen)
			{
			uiCanvas.SetActive(false);
			menuCanvas.SetActive(true);

			isMenuOpen = true;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection();
			SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
			}
		else if (Input.GetKeyDown(KeyCode.M) && isMenuOpen)
			{
			saveMenu.SetActive(false);
			settingsMenu.SetActive(false);
			menu.SetActive(true);

			uiCanvas.SetActive(true);
			menuCanvas.SetActive(false);

			isMenuOpen = false;

			if (CraftingSystem.Instance.isOpen == false && InventorySystem.Instance.isOpen == false)
				{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				}

			SelectionManager.Instance.EnableSelection();
			SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
			}
		}
	}