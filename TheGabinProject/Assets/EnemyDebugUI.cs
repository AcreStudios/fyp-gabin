using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyDebugUI : MonoBehaviour 
{
	// Canvas texts
	Text healthText;
	Text stateText;
	Text statusText;

	Transform target;

	
	void Start() 
	{
		healthText = transform.FindChild("Text_Health").GetComponent<Text>();
		stateText = transform.FindChild("Text_State").GetComponent<Text>();
		statusText = transform.FindChild("Text_Status").GetComponent<Text>();

		target = GameObject.Find("Player").transform;
	}

	void Update()
	{
		transform.rotation = Quaternion.LookRotation(transform.position - target.position);
	}
	
	public void UpdateHealth(float health)
	{
		healthText.text = "Health: " + Mathf.RoundToInt(health);
	}

	public void UpdateState(string state)
	{
		stateText.text = "State: " + state;
	}

	public void UpdateStatus(string status)
	{
		statusText.text = "Status: " + status;
	}
}
