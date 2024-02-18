using UnityEngine;

public class MouseMovement:MonoBehaviour
	{
	public float mouseSensitivity = 100f;

	private float xRotation = 0f;

	private float yRotation = 0f;

	private void Start()
		{
		// --- Locking the cursor to the middle of the screen and making it invisible --- //
		Cursor.lockState = CursorLockMode.Locked;
		}

	private void Update()
		{
		if (InventorySystem.Instance.isOpen == false)
		//if (InventorySystem.Instance.isOpen == false && !DialogSystem.Instance.dialogUIActive)

		//if (InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen)
		//if (InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpen && !MenuManager.Instance.isMenuOpen)

			{
			float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
			float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

			// --- control rotation around x axis (Look up and down) --- //
			xRotation -= mouseY;

			// --- we clamp the rotation so we cant Over-rotate (like in real life) --- //
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			// --- control rotation around y axis (Look up and down) --- //
			yRotation += mouseX;

			// --- applying both rotations --- //
			transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
			}
		}
	}