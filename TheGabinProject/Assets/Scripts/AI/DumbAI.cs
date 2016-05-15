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

                if ((target.position - transform.position).magnitude < range) {
                    currentBehaviour = States.Attacking;
                }
                break;

            case States.Attacking:
                if (Combat()) {
                    if ((target.position - transform.position).magnitude > range) {
                        currentBehaviour = States.Movement;
                    } 
                    else {
                        agent.speed = 0;
                    }
                } else {
                    if (storage == null) {
                        storage = RandomObstacle(obs, transform);
                        if (storage != null) {
                            targetPoint = FurthestPoint(storage);
                        }
                    } else {
                        Movement(targetPoint, speed);
                    }
                    
                }
                break;

        }
    }

}
