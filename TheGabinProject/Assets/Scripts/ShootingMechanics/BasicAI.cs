using UnityEngine;
using System.Collections;

public class BasicAI : MonoBehaviour {

    public float health;
    public GameObject target;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed);
        }
        if (health < 0) {
            Destroy(gameObject);
        }
	
	}

}
