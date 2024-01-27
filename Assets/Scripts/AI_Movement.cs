using UnityEngine;

public class AI_Movement:MonoBehaviour
	{
	private Animator animator;

	public float moveSpeed = 0.2f;

	private Vector3 stopPosition;

	private float walkTime;
	public float walkCounter;
	private float waitTime;
	public float waitCounter;

	private int WalkDirection;

	public bool isWalking;

	// --- Start is called before the first frame update --- //
	private void Start()
		{
		animator = GetComponent<Animator> ();

		// --- So that all the prefabs don't move/stop at the same time --- //
		walkTime = Random.Range (1, 2);
		waitTime = Random.Range (3, 9);

		waitCounter = waitTime;
		walkCounter = walkTime;

		ChooseDirection ();
		}

	// --- Update is called once per frame --- //
	private void Update()
		{
		if (isWalking)
			{
			animator.SetBool ("isRunning", true);

			walkCounter -= Time.deltaTime;

			switch (WalkDirection)
				{
				case 0:
					transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
					transform.position += moveSpeed * Time.deltaTime * transform.forward;
					break;

				case 1:
					transform.localRotation = Quaternion.Euler (0f, 90, 0f);
					transform.position += moveSpeed * Time.deltaTime * transform.forward;
					break;

				case 2:
					transform.localRotation = Quaternion.Euler (0f, -90, 0f);
					transform.position += moveSpeed * Time.deltaTime * transform.forward;
					break;

				case 3:
					transform.localRotation = Quaternion.Euler (0f, 180, 0f);
					transform.position += moveSpeed * Time.deltaTime * transform.forward;
					break;
				}

			if (walkCounter <= 0)
				{
				stopPosition = new (transform.position.x, transform.position.y, transform.position.z);
				isWalking = false;
				// --- Stop movement --- //
				transform.position = stopPosition;
				animator.SetBool ("isRunning", false);
				// --- Reset the waitCounter --- //
				waitCounter = waitTime;
				}
			}
		else
			{
			waitCounter -= Time.deltaTime;

			if (waitCounter <= 0)
				ChooseDirection ();
			}
		}

	public void ChooseDirection()
		{
		WalkDirection = Random.Range (0, 4);

		isWalking = true;
		walkCounter = walkTime;
		}
	}