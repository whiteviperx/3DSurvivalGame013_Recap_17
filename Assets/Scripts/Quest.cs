using UnityEngine;

[System.Serializable]
public class Quest
	{
	[Header("Bools")]
	public bool accepted;

	public bool declined;

	public bool intialDialogCompleted;

	public bool isCompleted;

	public bool hasNoRequirements;

	[Header("Quest Info")]
	public QuestInfo info;

	internal bool initialDialogCompleted;
	}