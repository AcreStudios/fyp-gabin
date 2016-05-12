using UnityEngine;
using System.Collections;

public class UserInput : MonoBehaviour 
{
	#region Declare variables
	public bool walkByDefault = false;

	Transform cam;
	Vector3 camForward;
	Vector3 move;
	#endregion

	#region Cache components
	CharacterMovement characterMovement;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		characterMovement = GetComponent<CharacterMovement>();
	}

	// Use this for initialization
	void Start()
	{
		if(Camera.main != null)
			cam = Camera.main.transform;
	}

	// FixedUpdate is called per physics tick
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		if(cam != null)
		{
			camForward = Vector3.Scale(cam.forward, new Vector3(1f,0f,1f)).normalized;

			move = v * camForward + h * cam.right;
		}
		else
			move = v * Vector3.forward + h * Vector3.right;

		if(move.magnitude > 1)
			move.Normalize();

		bool walkToggle = Input.GetKey(KeyCode.LeftShift);
		float walkMultiplier = 1f;

		if(walkByDefault)
		{
			if(walkToggle)
				walkMultiplier = 1f;
			else
				walkMultiplier = .5f;
		}
		else
		{
			if(walkToggle)
				walkMultiplier = .5f;
			else
				walkMultiplier = 1f;
		}

		move *= walkMultiplier;
		characterMovement.Move(move);
	}
}
