using UnityEngine;
using System.Collections;

public class DumbAI : AIBase {

    public enum States {
        Movement, Attacking
    }

    protected States currentBehaviour;
    protected GameObject storage;
    protected GameObject[] obs;
    protected Vector3 targetPoint;

    void Start() {
        useNavMesh = true;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        base.Start();
        storage = null;
    }

    void Update() {
        switch (currentBehaviour) {
            case States.Movement:

                if (storage != null) {
                    storage = null;
                }

                Movement(target.position, speed);

                if ((target.position - transform.position).magnitude < 50) {
                    currentBehaviour = States.Attacking;
                }
                break;

            case States.Attacking:
                if (Combat()) {
                    if ((target.position - transform.position).magnitude > 60) {
                        currentBehaviour = States.Movement;
                    } 
                    else {
                        agent.speed = 0;
                    }
                } else {
                    if (storage == null) {
                        float distStorage;
                        distStorage = Mathf.Infinity;
                        foreach (GameObject obstacles in obs) {

                            float currentDist = (obstacles.transform.position - transform.position).sqrMagnitude;

                            if (currentDist < distStorage) {
                                distStorage = currentDist;
                                storage = obstacles;
                            }
                        }
                        targetPoint = FurthestPoint(storage);
                    }
                    Movement(targetPoint, speed);
                }
                break;

        }
    }

}
