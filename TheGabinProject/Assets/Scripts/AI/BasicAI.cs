using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

    
    public GameObject target;
    public float speed;
   
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.color = Color.red;

        target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }

	
	}

}
