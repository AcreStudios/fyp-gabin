using UnityEngine;
using System.Collections;

public class TP_Movement : MonoBehaviour
{
	#region Declare variables
	public float moveSpeed = 10f;
	public float jumpSpeed = 6f;

	public float gravity = 21f; // Fall acceleration
	public float terminalVelocity = 20f; // Max falling speed

	public Vector3 moveVector { get; set; }
	public float verticalVelocity { get; set; }
	#endregion

	#region Cache components
	Transform trans;

	public static TP_Movement instance;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		trans = GetComponent<Transform>();

		instance = this;
	}

	public void MovementUpdate()
	{
		CalculateMovement();
		SnapAlignCharacterWithCamera();
	}

	void CalculateMovement()
	{
		// Convert moveVector into world space
		moveVector = trans.TransformDirection(moveVector);

		// Normalize moveVector if > 1
		if(moveVector.magnitude > 1)
			moveVector = Vector3.Normalize(moveVector);

		// Multiply moveVector by moveSpeed BUT not deltaTime yet
		moveVector *= moveSpeed;

		// Reapply verticalVelocity to moveVector.y
		moveVector = new Vector3(moveVector.x, verticalVelocity, moveVector.z);

		// Apply gravity
		ApplyGravity();

		// Move the character and multiply by deltaTime here instead
		TP_Controller.characterController.Move(moveVector * Time.deltaTime);

	}

	void ApplyGravity()
	{
		if(moveVector.y > -terminalVelocity)
			moveVector = new Vector3(moveVector.x, moveVector.y - gravity * Time.deltaTime, moveVector.z);

		if(TP_Controller.characterController.isGrounded && moveVector.y < -1)
			moveVector = new Vector3(moveVector.x, -1, moveVector.z);
	}

	public void Jump()
	{
		if(TP_Controller.characterController.isGrounded)
			verticalVelocity = jumpSpeed;
	}

	void SnapAlignCharacterWithCamera()
	{
		if(moveVector.x != 0 || moveVector.z != 0)
		{
			trans.rotation = Quaternion.Euler(trans.eulerAngles.x, Camera.main.transform.eulerAngles.y, trans.eulerAngles.z);
		}
	}
}
