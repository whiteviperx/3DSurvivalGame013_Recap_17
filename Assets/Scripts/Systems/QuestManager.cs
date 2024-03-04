using System.Collections.Generic;
using UnityEngine;

public class QuestManager:MonoBehaviour
	{
	public static QuestManager Instance { get; set; }

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

	public List<Quest> allActiveQuests;
	public List<Quest> allCompletedQuests;

	[Header("QuestMenu")]
	public GameObject questMenu;
	public bool isQuestMenuOpen;

	public GameObject activeQuestPrefab;
	public GameObject completedQuestPrefab;

	public GameObject questMenuContent;

	[Header("QuestTracker")]
	public GameObject questTrackerContent;

	private void Update()
		{
		if (Input.GetKeyDown(KeyCode.Q) && !isQuestMenuOpen && !ConstructionManager.Instance.inConstructionMode)
			{
			questMenu.SetActive(true);

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection();
			SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

			isQuestMenuOpen = true;

			}
		else if (Input.GetKeyDown(KeyCode.Q) && isQuestMenuOpen)
			questMenu.SetActive(false);

		if (!CraftingSystem.Instance.isOpen || !InventorySystem.Instance.isOpen)
			{

			}
		}


	public void AddActiveQuest(Quest quest)
		{
		allActiveQuests.Add(quest);
		RefreshQuestList();
		}

	public void MarkQuestCompleted(Quest quest)
		{
		// Remove quest from active list
		allActiveQuests.Remove(quest);
		// Add it into the completed list
		allCompletedQuests.Add(quest);

		RefreshQuestList();
		}



	public void RefreshQuestList()
		{

			foreach (Transform child in questMenuContent.transform)
			{
				Destroy(child.gameObject);
			}

			foreach (Quest activeQuest in allActiveQuests)
			{
			GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
			questPrefab.transform.SetParent(questMenuContent.transform, false);

			QuestRow qRow = questPrefab.GetComponent<QuestRow>();

			qRow.questName.text = activeQuest.questName;
			qRow.questGiver.text = activeQuest.questGiver;

			qRow.isActive = true;
			qRow.isTracking = true;

			qRow.coinAmount.text = $"{activeQuest.info.coinReward}";

			// qRow.firstReward.sprite = "";
			qRow.firstRewardAmount.text = "";

			// qRow.secondReward.sprite = "";
			qRow.secondRewardAmount.text = "";

			}

		foreach (Quest completedQuest in allCompletedQuests)
			{
			GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
			questPrefab.transform.SetParent(questMenuContent.transform, false);

			QuestRow qRow = questPrefab.GetComponent<QuestRow>();

			qRow.questName.text = completedQuest.questName;
			qRow.questGiver.text = completedQuest.questGiver;

			qRow.isActive = false;
			qRow.isTracking = false;

			qRow.coinAmount.text = $"{completedQuest.info.coinReward}";

			// qRow.firstReward.sprite = "";
			qRow.firstRewardAmount.text = "";

			// qRow.secondReward.sprite = "";
			qRow.secondRewardAmount.text = "";

			}
		}

	}