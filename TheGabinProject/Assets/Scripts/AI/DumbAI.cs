using UnityEngine;
using System.Collections;

public class DumbAI : AIBase {

    public enum States {
        Movement, Attacking
    }

    public States currentBehaviour;
    public float damage;

    void Start() {
    }

    void Update() {

        switch (currentBehaviour)
        {
            case States.Movement:
                Movement(target.position, speed);
                if ((target.position - transform.position).magnitude < 30){
                    currentBehaviour = States.Attacking;
                }
                break;

            case States.Attacking:
                if (Combat())
                {
                    //Debug.Log("Firing");
                    if ((target.position - transform.position).magnitude > 30)
                    {
                        currentBehaviour = States.Movement;
                    }
                }
                else
                {
                   //Debug.Log("Reloading");
                    Movement(target.position, -speed);
                }
                break;

        }
    }

}
