using UnityEngine;
using System.Collections;

public class DumbAI : AIBase {

    public enum States {
        Movement, Attacking
    }

    protected States currentBehaviour;

    protected GameObject[] obs;
    protected Vector3 targetPoint;
    protected string state;
  


    void Start() {
        StartCoroutine(Restoration(3));
        useNavMesh = true;
        obs = GameObject.FindGameObjectsWithTag("Obs");
        storage = null;
        scale = transform.localScale;
        base.Start();
    }

    void Update() {
        enemyUI.UpdateHealth(Health);
        switch (currentBehaviour) {
            case States.Movement:

                if (storage != null) {
                    storage = null;
                }

                Movement(target.position, speed);
                enemyUI.UpdateState("Advancing to player");
                if ((target.position - transform.position).magnitude < range) {
                    currentBehaviour = States.Attacking;
                    
                }
                break;

            case States.Attacking:
                
                if (Combat() == "Shooting") {
                    if ((target.position - transform.position).magnitude > range) {
                        currentBehaviour = States.Movement;
                        
                        transform.localScale = scale;
                    } 
                    else {
                        enemyUI.UpdateState("Firing");
                        agent.speed = 0;
                        storage = null;
                        transform.localScale = scale;
                    }
                } 
                if (Combat() == "Reloading"){
                    
                    if (storage == null) {
                        storage = RandomObstacle(obs, transform);
                        if (storage != null) {
                            targetPoint = FurthestPoint(storage);
                        }
                    } else {
                        enemyUI.UpdateState("Reloading,Running to hide");
                        Movement(targetPoint, speed);
                        transform.localScale = new Vector3(scale.x, scale.y/2, scale.z);
                    }
                    
                }
                break;

        }
    }

}
