using UnityEngine;

public class PlayerMovement:MonoBehaviour
	{
	public CharacterController controller;

	public float speed = 12f;

	public float gravity = -9.81f * 2;

	public float jumpHeight = 3f;

	// --- Ground Check --- //
	public Transform groundCheck;

	public float groundDistance = 0.4f;

	public LayerMask groundMask;

	private Vector3 velocity;

	private bool isGrounded;

	// --- Update is called once per frame --- //
	private void Update()
		{
		// --- Checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time --- //
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
			{
			velocity.y = -2f;
			}

		var x = Input.GetAxis("Horizontal");
		var z = Input.GetAxis("Vertical");

		// --- Right is the red Axis, forward is the blue axis --- //
		var move = transform.right * x + transform.forward * z;

		controller.Move(move * (speed * Time.deltaTime));

		// --- Check if the player is on the ground so he can jump --- //
		if (Input.GetButtonDown("Jump") && isGrounded)
			{
			// --- The equation for jumping --- //
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
			}

		velocity.y += gravity * Time.deltaTime;

		controller.Move(velocity * Time.deltaTime);
		}
	}