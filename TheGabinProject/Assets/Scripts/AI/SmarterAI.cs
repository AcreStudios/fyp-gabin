﻿using UnityEngine;
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
                if (storage.tag != "Obs") {
                    currentBehaviour = States.Scouting;
                }
                if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y, transform.position.z), target.transform.position, out hit)) {
                    //Debug.Log(hit.transform.root);
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
                        transform.localScale = new Vector3(1, 2, 1);

                        if ((target.position - transform.position).magnitude > range) {
                            currentBehaviour = States.Scouting;
                            Debug.Log("Changing");
                        }
                    } else {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                } else {
                    transform.localScale = new Vector3(1, 1, 1);
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