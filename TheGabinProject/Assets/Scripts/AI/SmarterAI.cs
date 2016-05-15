using UnityEngine;
using System.Collections;

public class SmarterAI : AIBase {

    public enum States {
        Movement, Reload, Shooting, Scouting, Camping
    }

    float cTimer;
    public States currentBehaviour;

    protected GameObject storage;
    protected GameObject[] obs;
    protected Vector3 targetPoint;
    public float crouchTime;

    void Start() {
        useNavMesh = true;
        currentBehaviour = States.Scouting;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        DamageRecieved(0, gameObject);
        base.Start();
    }

    void Update() {

        switch (currentBehaviour) {
            case States.Movement:
                Movement(targetPoint, speed);
                //if ((targetPoint - transform.position).sqrMagnitude < 5) {
                if (agent.velocity == Vector3.zero) { 
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
                        transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);
                        transform.localScale = new Vector3(1, 3, 1);

                        if ((target.position - transform.position).magnitude > range) {
                            currentBehaviour = States.Scouting;
                            Debug.Log("Changing");
                        }
                    } else {
                        transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
                        transform.localScale = new Vector3(1, 1.5f, 1);
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, 0.75f, transform.position.z);
                    transform.localScale = new Vector3(1, 1.5f, 1);
                }

                break;

            case States.Scouting:
                storage = RandomObstacle(obs,target);
                

                if (storage != null) {
                    targetPoint = FurthestPoint(storage);
                    currentBehaviour = States.Movement;
                }
                break;
        }
    }
}