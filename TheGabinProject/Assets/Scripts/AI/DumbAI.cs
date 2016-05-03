using UnityEngine;
using System.Collections;

public class DumbAI : BasicAI {

    public enum States {
        Movement, Reload, Shooting
    }

    public States currentBehaviour;
    public float damage;
    public float speed;
    public Transform player;

    public int ammoCount;
    public float timer;
    public bool reloadedYet;
	// Use this for initialization
	void Start () {
	}
	
    void Update() {

        if (ammoCount > 0){
            if ((player.position - transform.position).magnitude < 1)
            {
                currentBehaviour = States.Shooting;
            }
            else
            {
                currentBehaviour = States.Movement;
            }
        }

        else{
            currentBehaviour = States.Reload;
        }

        switch (currentBehaviour)
        {
            case States.Movement:
                Movement(player.position, speed);
                break;

            case States.Reload:
                if (!reloadedYet) {
                    timer = Time.time + 1.0f;
                    reloadedYet = true;
                }
                Movement(player.position, -speed);

                if (timer < Time.time) {
                    ammoCount = 20;
                    reloadedYet = false;
                }
                break;

            case States.Shooting:
                if (timer < Time.time)
                {
                    transform.LookAt(player);
                    Shooting(1);
                    ammoCount--;
                    timer = Time.time + 0.1f;
                }
                break;
        }
    }

}
