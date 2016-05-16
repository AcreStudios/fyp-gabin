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
        StartCoroutine(Restoration(3));
        useNavMesh = true;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        storage = null;

        base.Start();
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
                        transform.localScale = new Vector3(transform.localScale.x, 2, transform.localScale.z);
                    } 
                    else {
                        agent.speed = 0;
                        storage = null;
                        transform.localScale = new Vector3(transform.localScale.x, 2, transform.localScale.z);
                    }
                } else {
                    
                    if (storage == null) {
                        storage = RandomObstacle(obs, transform);
                        if (storage != null) {
                            targetPoint = FurthestPoint(storage);
                        }
                    } else {
                        Movement(targetPoint, speed);
                        transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
                    }
                    
                }
                break;

        }
    }

}
