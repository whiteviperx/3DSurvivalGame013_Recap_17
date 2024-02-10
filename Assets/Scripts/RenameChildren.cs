// I have been using this script to name children of parent. Just put script in script folder and it can be used by right clicking on the parent and filling out info. Very simple.

using UnityEditor;

using UnityEngine;

public class RenameChildren:EditorWindow
	{
	private static readonly Vector2Int size = new(250, 100);

	private string childrenPrefix;

	private int startIndex;

	[MenuItem("GameObject/Rename children")]
	public static void ShowWindow()
		{
		EditorWindow window = GetWindow<RenameChildren>();
		window.minSize = size;
		window.maxSize = size;
		}

	private void OnGUI()
		{
		childrenPrefix = EditorGUILayout.TextField("Children prefix", childrenPrefix);
		startIndex = EditorGUILayout.IntField("Start index", startIndex);
		if (GUILayout.Button("Rename children"))
			{
			GameObject [] selectedObjects = Selection.gameObjects;
			for (int objectI = 0; objectI < selectedObjects.Length; objectI++)
				{
				Transform selectedObjectT = selectedObjects [objectI].transform;
				for (int childI = 0, i = startIndex; childI < selectedObjectT.childCount; childI++)
					selectedObjectT.GetChild(childI).name = $"{childrenPrefix}{i++}";
				}
			}
		}
	}