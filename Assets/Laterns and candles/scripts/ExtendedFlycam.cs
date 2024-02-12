﻿using UnityEngine;
using System.Collections;

public class ExtendedFlycam : MonoBehaviour
{

	/*
	EXTENDED FLYCAM
		Desi Quintans (CowfaceGames.com), 17 August 2012.
		Based on FlyThrough.js by Slin (http://wiki.unity3d.com/index.php/FlyThrough), 17 May 2011.
 
	LICENSE
		Free as in speech, and free as in beer.
 
	FEATURES
		WASD/Arrows:    Movement
		          Q:    Climb
		          E:    Drop
                      Shift:    Move faster
                    Control:    Move slower
                        End:    Toggle cursor locking to screen (you can also press Ctrl+P to toggle play mode on and off).
	*/

	public float cameraSensitivity = 90;
	public float climbSpeed = 4;
	public float normalMoveSpeed = 10;
	public float slowMoveFactor = 0.25f;
	public float fastMoveFactor = 3;

	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	void Start ()
	{
		Cursor.lockState = CursorLockMode.Locked;
		}

	void Update ()
	{
		rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
		rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp (rotationY, -90, 90);

		transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			transform.position += (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime * transform.forward;
			transform.position += (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime * transform.right;
		}
		else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		{
			transform.position += (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime * transform.forward;
			transform.position += (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime * transform.right;
		}
		else
		{
			transform.position += Input.GetAxis("Vertical") * normalMoveSpeed * Time.deltaTime * transform.forward;
			transform.position += Input.GetAxis("Horizontal") * normalMoveSpeed * Time.deltaTime * transform.right;
		}


		if (Input.GetKey(KeyCode.Q)) { transform.position += climbSpeed * Time.deltaTime * transform.up; }
		if (Input.GetKey(KeyCode.E)) { transform.position -= climbSpeed * Time.deltaTime * transform.up; }

		if (Input.GetKeyDown (KeyCode.End))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			}
	}
}