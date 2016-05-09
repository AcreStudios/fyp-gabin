using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour 
{
	#region Declare variables
	// String for input axes
	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";
	public string jumpButton = "Jump";
	#endregion

	#region Cache components
	public static CharacterController characterController;
	public static TP_Controller instance;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		characterController = GetComponent<CharacterController>();
		instance = this;

		TP_Camera.UseExistingOrCreateNewMainCamera();
	}

	// Update is called once per frame
	void Update()
	{
		if(Camera.main == null) // To prevent crashes
			return;

		GetLocomotionInput();
		HandleActionInput();

		TP_Movement.instance.MovementUpdate();
	}

	void GetLocomotionInput()
	{
		float deadZone = 0.1f;

		TP_Movement.instance.verticalVelocity = TP_Movement.instance.moveVector.y;
		TP_Movement.instance.moveVector = Vector3.zero;

		if(Input.GetAxis(verticalAxis) > deadZone || Input.GetAxis(verticalAxis) < -deadZone)
			TP_Movement.instance.moveVector += new Vector3(0, 0, Input.GetAxis(verticalAxis));

		if(Input.GetAxis(horizontalAxis) > deadZone || Input.GetAxis(horizontalAxis) < -deadZone)
			TP_Movement.instance.moveVector += new Vector3(Input.GetAxis(horizontalAxis), 0, 0);
	}

	void HandleActionInput()
	{
		if(Input.GetButton(jumpButton))
			DoJump();
	}

	void DoJump()
	{
		TP_Movement.instance.Jump();
	}
}
