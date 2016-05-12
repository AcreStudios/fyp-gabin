using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
	#region Declare variables
	float moveSpeedMultiplier = 1f;
	float stationaryTurnSpeed = 180f;
	float movingTurnSpeed = 360f;

	bool onGround;

	// For animator component 
	Vector3 moveInput; // Move vector
	float turnAmount;
	float forwardAmount;
	Vector3 velocity;

	float jumpSpeed = 10f;

	IComparer rayHitComparer;
	#endregion

	#region Cache components
	Transform trans;
	Rigidbody rigidBody;
	Animator animator;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		trans = GetComponent<Transform>();

		rigidBody = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	void Start() 
	{
		SetupAnimator();
	}

	public void Move(Vector3 move)
	{
		if(move.magnitude > 1)
			move.Normalize();

		this.moveInput = move;

		velocity = rigidBody.velocity;

		ConvertMoveInput();
		ApplyExtraTurnRotation();
		GroundCheck();
		UpdateAnimator();
	}

	void SetupAnimator()
	{
		animator = GetComponent<Animator>();

		foreach(Animator childAnimator in GetComponentsInChildren<Animator>())
		{
			if(childAnimator != animator)
			{
				animator.avatar = childAnimator.avatar;
				Destroy(childAnimator);
				break;
			}
		}
	}

	void OnAnimatorMove()
	{
		if(onGround && Time.deltaTime > 0)
		{
			Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

			v.y = rigidBody.velocity.y;
			rigidBody.velocity = v;
		}
	}

	void ConvertMoveInput()
	{
		Vector3 localMove = trans.InverseTransformDirection(moveInput);

		turnAmount = Mathf.Atan2(localMove.x, localMove.z);
		forwardAmount = localMove.z;
	}

	void UpdateAnimator()
	{
		animator.applyRootMotion = true;

		animator.SetFloat("Forward", forwardAmount, .1f, Time.deltaTime);
		animator.SetFloat("Turn", turnAmount, .1f, Time.deltaTime);
	}

	void ApplyExtraTurnRotation()
	{
		float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
		trans.Rotate(0f, turnAmount * turnSpeed * Time.deltaTime, 0f);
	}

	// Check if the character is on the ground or airborne
	void GroundCheck()
	{
		// Creates a ray with the character's transform as origin on the y-axis and the -y axis direction
		Ray ray = new Ray(trans.position + Vector3.up * -.5f, -Vector3.up);

		RaycastHit[] hits = Physics.RaycastAll(ray, .5f); // Perform a raycast using the ray
		rayHitComparer = new RayHitComparer();

		System.Array.Sort(hits, rayHitComparer); // Sort the hits using our computer (based on distance)

		if(velocity.y < jumpSpeed * .5f)
		{
			// Assume that the character is airborne and falling
			onGround = false;
			rigidBody.useGravity = true;

			foreach(var hit in hits)
			{
				// Check whether we hit a non-trigger collider
				if(!hit.collider.isTrigger)
				{
					if(velocity.y <= 0)
					{
						rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point, Time.deltaTime * 5f);
					}

					onGround = true; // Since we found our collider, we grounded
					rigidBody.useGravity = false; // Disable gravity cos we are using the above to ground the character
					break; // Ignore other hits
				}
			}
		}
	}

	class RayHitComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);


		}
	}
}
