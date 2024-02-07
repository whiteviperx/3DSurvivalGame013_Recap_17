using UnityEngine;
using UnityEngine.UI;

public class SelectionManager:MonoBehaviour
	{
	// --- Singleton --- //
	public static SelectionManager Instance
		{
		get; set;
		}

	// --- Make sure dot is on target & not a nearby object --- //
	public bool onTarget;

	// --- To select only 1 object at a time --- //
	public GameObject selectedObject;

	// --- Info text UI below white circle --- //
	public GameObject interaction_Info_UI;

	private Text interaction_text;

	// --- Changing from dot to hand --- //
	public Image centerDotImage;

	public Image handIcon;

	public bool handIsVisible;

	public GameObject selectedTree;

	public GameObject chopHolder;

	private void Start()
		{
		onTarget = false;
		interaction_text = interaction_Info_UI.GetComponent<Text>();
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
		}

	private void Update()
		{
		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
			{
			var selectionTransform = hit.transform;

			var interactable = selectionTransform.GetComponent<InteractableObject>();

			ChoppableTree choppableTree = selectionTransform.GetComponent<ChoppableTree>();

			if (choppableTree && choppableTree.playerInRange)
				{
				choppableTree.canBeChopped = true;
				selectedTree = choppableTree.gameObject;
				chopHolder.gameObject.SetActive(true);
				}
			else
				{
				if (selectedTree != null)
					{
					selectedTree.gameObject.GetComponent<ChoppableTree>().canBeChopped = false;
					selectedTree = null;
					chopHolder.gameObject.SetActive(false);
					}
				}

			if (interactable && interactable.playerInRange)
				{
				// --- Shows that we are on target --- //
				onTarget = true;

				// --- Selects a single object --- //
				selectedObject = interactable.gameObject;

				// --- Interaction info --- //
				interaction_text.text = interactable.GetItemName();
				interaction_Info_UI.SetActive(true);

				// --- Compare tags to only interact and show hand with pickable items --- //
				if (interactable.CompareTag("Pickable"))
					{
					centerDotImage.gameObject.SetActive(false);
					handIcon.gameObject.SetActive(true);

					handIsVisible = true;
					}
				else
					{
					handIcon.gameObject.SetActive(false);
					centerDotImage.gameObject.SetActive(true);

					handIsVisible = false;
					}
				}
			else // --- If there is a hit, but without an interactable Script --- //
				{
				onTarget = false;
				interaction_Info_UI.SetActive(false);
				handIcon.gameObject.SetActive(false);
				centerDotImage.gameObject.SetActive(true);

				handIsVisible = false;
				}
			}
		else // --- If there is no hit at all --- //
			{
			onTarget = false;
			interaction_Info_UI.SetActive(false);
			handIcon.gameObject.SetActive(false);
			centerDotImage.gameObject.SetActive(true);

			handIsVisible = false;
			}
		}

	public void DisableSelection()
		{
		handIcon.enabled = false;
		centerDotImage.enabled = false;
		interaction_Info_UI.SetActive(false);

		selectedObject = null;
		}

	public void EnableSelection()
		{
		handIcon.enabled = true;
		centerDotImage.enabled = true;
		interaction_Info_UI.SetActive(true);
		}
	}