using UnityEngine;

[System.Serializable]
public class Quest
	{
	public string questName;
	public string questGiver;
	public string questDescription;


	[Header("Bools")]
	public bool accepted;

	public bool declined;

	public bool intialDialogCompleted;

	public bool isCompleted;

	public bool hasNoRequirements;

	[Header("Quest Info")]
	public QuestInfo info;

	//internal bool initialDialogCompleted;
	}