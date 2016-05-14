using UnityEngine;
using System.Collections;

public class SmarterAI : AIBase {

    public enum States {
        Movement, Reload, Shooting, Scouting, Camping
    }

    float cTimer;
    protected States currentBehaviour;

    protected GameObject storage;
    protected GameObject[] obs;
    protected Vector3 targetPoint;
    public float crouchTime;

    void Start() {
        //useNavMesh = true;
        currentBehaviour = States.Scouting;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        DamageRecieved(0, gameObject);
        base.Start();
    }

    void Update() {

        switch (currentBehaviour) {
            case States.Movement:
                Movement(targetPoint, speed);
                if ((targetPoint - transform.position).sqrMagnitude < 5) {
                    currentBehaviour = States.Camping;
                }
                break;

            case States.Camping:
                if (Physics.Linecast(new Vector3(transform.position.x, 0.1f, transform.position.z), target.transform.position, out hit)) {
                    if (hit.transform == target) {
                        currentBehaviour = States.Scouting;
                    }
                }

                if (shotAt) {
                    cTimer = Time.time + crouchTime;
                    shotAt = false;
                }

                if (cTimer < Time.time) {
                    if (Combat()) {
                        transform.position = new Vector3(transform.position.x, 2.5f, transform.position.z);

                        if ((target.position - transform.position).magnitude > 60) {
                            currentBehaviour = States.Scouting;
                        }
                    } else {
                        transform.position = new Vector3(transform.position.x, 1.25f, transform.position.z);
                        transform.localScale = new Vector3(5, 2.5f, 5);
                    }
                } else {
                    transform.localScale = new Vector3(5, 2.5f, 5);
                }

                break;

            case States.Scouting:
                float distStorage;
                distStorage = Mathf.Infinity;
                foreach (GameObject obstacles in obs) {

                    float currentDist = (target.position - obstacles.transform.position).sqrMagnitude;

                    if (currentDist < distStorage) {
                        distStorage = currentDist;
                        storage = obstacles;
                    }
                }
                targetPoint = FurthestPoint(storage);
                currentBehaviour = States.Movement;
                break;
        }
    }
}