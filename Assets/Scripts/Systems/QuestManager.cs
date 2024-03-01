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
	}