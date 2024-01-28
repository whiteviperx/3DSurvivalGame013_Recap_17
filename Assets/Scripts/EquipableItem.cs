// Ignore Spelling: Equipable

using System.Collections;

using UnityEngine;

[RequireComponent (typeof (Animator))]
public class EquipableItem:MonoBehaviour
	{
	public Animator animator;

	// --- Start is called before the first frame update --- //
	private void Start()
		{
		animator = GetComponent<Animator> ();
		}

	// --- Update is called once per frame --- //
	private void Update()
		{
		if (Input.GetMouseButtonDown (0) && // --- Left Mouse Button --- //
			InventorySystem.Instance.isOpen == false &&
			CraftingSystem.Instance.isOpen == false &&
			SelectionManager.Instance.handIsVisible == false &&
			!ConstructionManager.Instance.inConstructionMode
			)

			{

			SoundManager.Instance.PlaySound (SoundManager.Instance.toolSwingSound);
			animator.SetTrigger ("hit");

			Debug.Log ("EquipableItem animator.SetTrigger hit");
			}
		}

	public void GetHit()
		{
		GameObject selectedTree = SelectionManager.Instance.selectedTree;

		if (selectedTree != null)
			{
			selectedTree.GetComponent<ChoppableTree> ().GetHit ();
			}
		}
	IEnumerator SwingSoundDelay()
		{
		yield return new WaitForSeconds (0.1f);
		SoundManager.Instance.PlaySound (SoundManager.Instance.toolSwingSound);
		}


	}