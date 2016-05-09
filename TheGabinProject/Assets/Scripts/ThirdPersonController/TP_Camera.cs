using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour 
{
	#region Declare variables
	// Target to look at
	public Transform targetLookAt;

	// Mouse, sensitivity and limits
	public string horizontalAxis = "Mouse X", verticalAxis = "Mouse Y", zoomAxis = "Mouse ScrollWheel";
	public bool invertY = false;
	public float sensitivityX = 2f, sensitivityY = 1f, sensitivityZoom = 5f;
	public float minLimitY = -40f, maxLimitY = 80f;

	// Camera distance from target
	public float distance = 5f;
	public float distanceMin = 3f, distanceMax = 10f;

	public float distanceSmoothTime = 0.05f;
	public float smoothX = 0.05f, smoothY = 0.1f;

	float mouseX, mouseY;
	float startDistance;
	float desiredDistance;
	float velocityDistance;

	float velocityX, velocityY, velocityZ;

	Vector3 currentPosition = Vector3.zero;
	Vector3 desiredPosition = Vector3.zero;
	#endregion

	#region Cache components
	public static TP_Camera instance;

	Transform trans;
	#endregion

	// Awake is called before Start
	void Awake()
	{
		instance = this;

		trans = GetComponent<Transform>();
	}

	// Use this for initialization
	void Start()
	{
		distance = Mathf.Clamp(distance, distanceMin, distanceMax);
		startDistance = distance;
		Reset();
	}

	// LateUpdate is called after all Update
	void LateUpdate()
	{
		if(targetLookAt == null)
			return;

		HandlePlayerInput();
		CalculateDesiredPosition();
		PositionUpdate();
	}

	void HandlePlayerInput()
	{
		float deadZone = 0.01f;

		mouseX += Input.GetAxis(horizontalAxis) * sensitivityX;
		if(!invertY)
			mouseY -= Input.GetAxis(verticalAxis) * sensitivityY;
		else
			mouseY += Input.GetAxis(verticalAxis) * sensitivityY;

		// Limit mouseY
		mouseY = TP_Helper.ClampAngle(mouseY, minLimitY, maxLimitY);

		if(Input.GetAxis(zoomAxis) < -deadZone || Input.GetAxis(zoomAxis) > deadZone)
		{
			desiredDistance = Mathf.Clamp(distance - Input.GetAxis(zoomAxis) * sensitivityZoom, distanceMin, distanceMax);
		}
	}

	void CalculateDesiredPosition()
	{
		// Evaluate distance and smooth(S-curve)
		distance = Mathf.SmoothDamp(distance, desiredDistance, ref velocityDistance, distanceSmoothTime);

		// Calculate desired position
		desiredPosition = CalculatePosition(mouseY, mouseX, distance);
	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0f, 0f, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

		return targetLookAt.position + rotation * direction;
	}

	void PositionUpdate()
	{
		float posX = Mathf.SmoothDamp(currentPosition.x, desiredPosition.x, ref velocityX, smoothX);
		float posY = Mathf.SmoothDamp(currentPosition.y, desiredPosition.y, ref velocityY, smoothY);
		float posZ = Mathf.SmoothDamp(currentPosition.z, desiredPosition.z, ref velocityZ, smoothX);
		currentPosition = new Vector3(posX, posY, posZ);

		trans.position = currentPosition;

		trans.LookAt(targetLookAt);
	}

	public void Reset()
	{
		mouseX = 0f;
		mouseY = 10f;
		distance = startDistance;
		desiredDistance = distance;
	}

	public static void UseExistingOrCreateNewMainCamera()
	{
		GameObject tempCamera;
		GameObject tempTargetLookAt;
		TP_Camera myCamera;

		if(Camera.main != null)
		{
			tempCamera = Camera.main.gameObject;
		}
		else
		{
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent<Camera>();
			tempCamera.tag = "MainCamera";
		}

		tempCamera.AddComponent<TP_Camera>();
		myCamera = tempCamera.GetComponent<TP_Camera>();

		tempTargetLookAt = GameObject.Find("Target Look At").gameObject;

		if(tempTargetLookAt == null)
		{
			tempTargetLookAt = new GameObject("Target Look At");
			tempTargetLookAt.transform.position = Vector3.zero;
		}

		myCamera.targetLookAt = tempTargetLookAt.transform;
	}
}
