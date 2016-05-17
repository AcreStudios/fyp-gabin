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

    Vector3 scale;

    void Start() {
        useNavMesh = true;
        currentBehaviour = States.Scouting;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        scale = transform.localScale;
        base.Start();
    }

    void Update() {

        switch (currentBehaviour) {
            case States.Movement:
                Movement(targetPoint, speed);
                if (agent.velocity == Vector3.zero) { 
                    currentBehaviour = States.Camping;
                }
                break;

            case States.Camping:
                if (storage.tag != "Obs") {
                    currentBehaviour = States.Scouting;
                }
                if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y, transform.position.z), target.transform.position, out hit)) {
                    if (hit.transform.root == target.transform) {
                        currentBehaviour = States.Scouting;
                    }
                }

                if (shotAt) {
                    cTimer = Time.time + crouchTime;
                    shotAt = false;
                }

                if (cTimer < Time.time) {
                    if (Combat()) {
                        transform.localScale = scale;

                        if ((target.position - transform.position).magnitude > range) {
                            currentBehaviour = States.Scouting;
                            Debug.Log("Changing");
                        }
                    } else {
                        transform.localScale = new Vector3(scale.x, scale.y / 2, scale.z);
                    }
                } else {
                    transform.localScale = new Vector3(scale.x, scale.y / 2, scale.z);
                }

                break;

            case States.Scouting:
                storage = RandomObstacleFromPlayer(obs,target);

                //instance = storage.GetComponent<BaseClass>();
                if (storage != null) {
                    targetPoint = FurthestPoint(storage);
                    currentBehaviour = States.Movement;
                }
                break;
        }
    }
}