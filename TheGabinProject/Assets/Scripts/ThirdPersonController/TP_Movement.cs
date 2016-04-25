using UnityEngine;
using System.Collections;

public class TP_Movement : MonoBehaviour
{
	#region Declare variables
	public float moveSpeed = 10f;

	public Vector3 moveVector { get; set; }
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

		// Multiply moveVector by moveSpeed and deltaTime
		moveVector *= moveSpeed * Time.deltaTime;

		// Move the character
		TP_Controller.characterController.Move(moveVector);

	}

	void SnapAlignCharacterWithCamera()
	{
		if(moveVector.x != 0 || moveVector.z != 0)
		{
			trans.rotation = Quaternion.Euler(trans.eulerAngles.x, Camera.main.transform.eulerAngles.y, trans.eulerAngles.z);
		}
	}
}
