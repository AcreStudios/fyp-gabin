using UnityEngine;
using System.Collections;

public class SmarterAI : AIBase {

    public enum States {
        Movement, Reload, Shooting, Scouting, Camping
    }

    public States currentBehaviour;
    public float damage;

    public GameObject storage;
    public GameObject[] obs;
    // Use this for initialization
    void Start() {
        currentBehaviour = States.Scouting;
        obs = GameObject.FindGameObjectsWithTag("Obs");
    }

    void Update() {

        if (storage != null)
        {
            Debug.DrawLine(transform.position, storage.transform.position, Color.red);
        }

        switch (currentBehaviour)
        {
            case States.Movement:
                Movement(storage.transform.position, speed);
                if (transform.position == storage.transform.position)
                {
                    currentBehaviour = States.Camping;
                }
                break;

            case States.Camping:

                if (Combat()){
                    transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);
                    transform.localScale = new Vector3(5, 5, 5);

                    if ((target.position - transform.position).magnitude > 40)
                    {
                        currentBehaviour = States.Scouting;
                    }
                }
                else{
                    transform.position = new Vector3(transform.position.x, 1.25f, transform.position.z);
                    transform.localScale = new Vector3(5, 2.5f, 5);
                }
                break;

            case States.Scouting:
               // Debug.Log("Scouting!");
                float distStorage;
                distStorage = Mathf.Infinity;
                foreach (GameObject obstacles in obs)
                {

                    float currentDist = (target.position - obstacles.transform.position).sqrMagnitude;

                    if (currentDist < distStorage)
                    {
                        distStorage = currentDist;
                        storage = obstacles;
                    }
                }
                currentBehaviour = States.Movement;
                break;

        }
    }

}