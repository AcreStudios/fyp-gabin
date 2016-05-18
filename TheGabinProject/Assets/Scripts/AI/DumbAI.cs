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

    Vector3 scale;

    void Start() {
        StartCoroutine(Restoration(3));
        useNavMesh = true;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        storage = null;
        scale = transform.localScale;

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
                        transform.localScale = scale;
                    } 
                    else {
                        agent.speed = 0;
                        storage = null;
                        transform.localScale = scale;
                    }
                } else {
                    
                    if (storage == null) {
                        storage = RandomObstacle(obs, transform);
                        if (storage != null) {
                            targetPoint = FurthestPoint(storage);
                        }
                    } else {
                        Movement(targetPoint, speed);
                        transform.localScale = new Vector3(scale.x, scale.y/2, scale.z);
                    }
                    
                }
                break;

        }
    }

}
