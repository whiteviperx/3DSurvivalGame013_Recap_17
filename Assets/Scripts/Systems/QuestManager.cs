using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager:MonoBehaviour
{
	public static QuestManager Instance { get; set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy ( gameObject );
		}
		else
		{
			Instance = this;
		}
	}

	public List<Quest> allActiveQuests;

	public List<Quest> allCompletedQuests;

	[Header ( "QuestMenu" )]
	public GameObject questMenu;

	public bool isQuestMenuOpen;

	public GameObject activeQuestPrefab;

	public GameObject completedQuestPrefab;

	public GameObject questMenuContent;

	[Header ( "QuestTracker" )]
	public GameObject questTrackerContent;

	public GameObject trackerRowPrefab;

	public List<Quest> allTrackedQuests;

	public void TrackQuest(Quest quest)
	{
		allTrackedQuests.Add ( quest );
		RefreshTrackerList ();
	}

	public void UnTrackQuest(Quest quest)
	{
		allTrackedQuests.Remove ( quest );
		RefreshTrackerList ();
	}

	public void RefreshTrackerList()
	{
		// Destroying the previous list
		foreach (Transform child in questTrackerContent.transform)
			Destroy ( child.gameObject );

		foreach (Quest trackedQuest in allTrackedQuests)
		{
			var trackerPrefab = Instantiate ( trackerRowPrefab, Vector3.zero, Quaternion.identity );
			trackerPrefab.transform.SetParent ( questTrackerContent.transform, false );

			var tRow = trackerPrefab.GetComponent<TrackerRow> ();

			tRow.questName.text = trackedQuest.questName;
			tRow.description.text = trackedQuest.questDescription;

			var req1 = trackedQuest.info.firstRequirementItem;
			var req1Amount = trackedQuest.info.firstRequirementAmount;

			var req2 = trackedQuest.info.secondRequirementItem;
			var req2Amount = trackedQuest.info.secondRequirementAmount;

			if (req2 != "") // if we have 2 requirements
			{
				tRow.requirements.text = $"{req1} " + InventorySystem.Instance.CheckItemAmount ( req1 ) + "/" + $"{req1Amount}\n" +
			   $"{req2} " + InventorySystem.Instance.CheckItemAmount ( req2 ) + "/" + $"{req2Amount}";
			}
			else // if we have only one
			{
				tRow.requirements.text = $"{req1} " + InventorySystem.Instance.CheckItemAmount ( req1 ) + "/" + $"{req1Amount}";
			}

			if (trackedQuest.info.hasCheckpoints)
			{
				var existingText = tRow.requirements.text;
				tRow.requirements.text = PrintCheckpoints ( trackedQuest, existingText );
			}
		}
	}

	private string PrintCheckpoints(Quest trackedQuest, string existingText)
	{
		var finalText = existingText;

		foreach (Checkpoint cp in trackedQuest.info.checkpoints)
		{
			if
			(cp.isCompleted)
			{
				finalText = finalText + "\n" + cp.name + " [Completed]";
			}
			else
			{
				finalText = finalText + "\n" + cp.name;
			}
		}
		return finalText;
	}
	private void Update()
	{
		if (Input.GetKeyDown ( KeyCode.Q ) && !isQuestMenuOpen && !ConstructionManager.Instance.inConstructionMode)
		{
			questMenu.SetActive ( true );

			questMenu.transform.Find ( "QuestTracker" ).GetComponent<Canvas> ().sortingOrder = MenuManager.Instance.SetAsFront ();

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			SelectionManager.Instance.DisableSelection ();
			SelectionManager.Instance.GetComponent<SelectionManager> ().enabled = false;

			isQuestMenuOpen = true;
		}
		else if (Input.GetKeyDown ( KeyCode.Q ) && isQuestMenuOpen)
			questMenu.SetActive ( false );

		if (!CraftingSystem.Instance.isOpen || !InventorySystem.Instance.isOpen)
		{
		}
	}

	public void AddActiveQuest(Quest quest)
	{
		allActiveQuests.Add ( quest );
		TrackQuest ( quest );
		RefreshQuestList ();
	}

	public void MarkQuestCompleted(Quest quest)
	{
		// Remove quest from active list
		allActiveQuests.Remove ( quest );

		// Add it into the completed list
		allCompletedQuests.Add ( quest );
		UnTrackQuest ( quest );
		RefreshQuestList ();
	}

	public void RefreshQuestList()
	{
		foreach (Transform child in questMenuContent.transform)
			Destroy ( child.gameObject );

		foreach (Quest activeQuest in allActiveQuests)
		{
			var questPrefab = Instantiate ( activeQuestPrefab, Vector3.zero, Quaternion.identity );
			questPrefab.transform.SetParent ( questMenuContent.transform, false );

			var qRow = questPrefab.GetComponent<QuestRow> ();

			qRow.questName.text = activeQuest.questName;
			qRow.questGiver.text = activeQuest.questGiver;

			qRow.thisQuest = activeQuest;

			qRow.isActive = true;
			qRow.isTracking = true;

			qRow.coinAmount.text = $"{activeQuest.info.coinReward}";

			if (activeQuest.info.rewardItem1 != "")
			{
				qRow.firstReward.sprite = GetSpriteForItem ( activeQuest.info.rewardItem1 ); ;
				qRow.firstRewardAmount.text = "";
			}
			else
			{
				qRow.firstReward.gameObject.SetActive ( false );
				qRow.firstRewardAmount.text = "";
			}

			if (activeQuest.info.rewardItem2 != "")
			{
				qRow.secondReward.sprite = GetSpriteForItem ( activeQuest.info.rewardItem2 ); ;
				qRow.secondRewardAmount.text = "";
			}
			else
			{
				qRow.secondReward.gameObject.SetActive ( false );
				qRow.secondRewardAmount.text = "";
			}
		}

		foreach (Quest completedQuest in allCompletedQuests)
		{
			var questPrefab = Instantiate ( completedQuestPrefab, Vector3.zero, Quaternion.identity );
			questPrefab.transform.SetParent ( questMenuContent.transform, false );

			var qRow = questPrefab.GetComponent<QuestRow> ();

			qRow.questName.text = completedQuest.questName;
			qRow.questGiver.text = completedQuest.questGiver;

			qRow.isActive = false;
			qRow.isTracking = false;

			qRow.coinAmount.text = $"{completedQuest.info.coinReward}";

			if (completedQuest.info.rewardItem1 != "")
			{
				qRow.firstReward.sprite = GetSpriteForItem ( completedQuest.info.rewardItem1 ); ;
				qRow.firstRewardAmount.text = "";
			}
			else
			{
				qRow.firstReward.gameObject.SetActive ( false );
				qRow.firstRewardAmount.text = "";
			}

			if (completedQuest.info.rewardItem2 != "")
			{
				qRow.secondReward.sprite = GetSpriteForItem ( completedQuest.info.rewardItem2 ); ;
				qRow.secondRewardAmount.text = "";
			}
			else
			{
				qRow.secondReward.gameObject.SetActive ( false );
				qRow.secondRewardAmount.text = "";
			}
		}
	}

	private Sprite GetSpriteForItem(string item)
	{
		var itemToGet = Resources.Load<GameObject> ( item );
		return itemToGet.GetComponent<Image> ().sprite;
	}
}