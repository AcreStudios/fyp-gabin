﻿using UnityEngine;

using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AIBase : BaseClass {

    protected RaycastHit hit;
    protected bool reloading;
    protected float timer;
    protected float ammoCount;
    public float weaponDamage;
    public float sprayValue;
    public Transform target;
    public float range;

    protected BaseClass sTarget;
    protected GameObject currentTarget;

    public GameObject explosionEffect;

    Transform head;
    Transform gun;
    Transform gunpoint;
    protected bool useNavMesh;
    protected NavMeshAgent agent;
    Vector3 finalDestination;
    GameObject toReturn;

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        if (!useNavMesh) {
            agent.enabled = false;
        }

        head = transform.Find("Head");
        gun = transform.Find("Gun");
        gunpoint = gun.transform.Find("Gunpoint");
        criticalPart = head.gameObject;
    }

    public void Movement(Vector3 value, float speed) {
        if (useNavMesh) {
            agent.speed = speed*20;
            agent.acceleration = speed*20;
            agent.destination = value;
            agent.angularSpeed = 360;

        } else
            transform.position = Vector3.MoveTowards(transform.position, value, speed);

    }

    public void Shooting() {

        Vector3 storage;

        storage = WeaponSpray(sprayValue);
        Debug.DrawRay(gunpoint.position, gun.TransformDirection(0, 0, range) + storage, Color.black, 2f);

        if (Physics.Raycast(gunpoint.position, gun.TransformDirection(0, 0, range) + storage, out hit)) {
            if (currentTarget != null) {
                if (!currentTarget.GetComponent<BaseClass>()) {
                    currentTarget = null;
                }
            }

            Instantiate(explosionEffect, hit.point, Quaternion.identity);

            if (currentTarget != hit.transform.gameObject && hit.transform.root != transform) {
                if (hit.transform.root.gameObject.GetComponent<BaseClass>()) {
                    currentTarget = hit.transform.gameObject;
                    sTarget = hit.transform.root.gameObject.GetComponent<BaseClass>();
                } else {
                    currentTarget = null;
                }
            }
            if (currentTarget != null) {
                sTarget.DamageRecieved(weaponDamage, hit.transform.gameObject);
            }
        }
    }

    public Vector3 WeaponSpray(float value) {

        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), 0);
    }

    public bool Combat() {

        if (ammoCount > 0) {
            if (timer < Time.time) {
                transform.LookAt(target.Find("Model_Player"));
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
                gun.LookAt(target.Find("Model_Player"));
                Shooting();
                ammoCount--;
                timer = Time.time + 0.1f;
            }
            return true;
        } else {
            if (!reloading) {
                timer = Time.time + 3.0f;
                reloading = true;
            }
            if (timer < Time.time) {
                ammoCount = 20;
                reloading = false;
            }
            return false;
        }
    }

    public GameObject RandomObstacle(GameObject[] obst, Transform reference) {

        toReturn = null;

        foreach (GameObject obstacles in obst) {

            float currentDist = (obstacles.transform.position - reference.position).magnitude;
            //Debug.Log(currentDist);
            if (currentDist < range) {
                if (Random.value > 0.9) {
                    toReturn = obstacles;
                }
            }
        }
        return toReturn;
    }

    public GameObject RandomObstacleFromPlayer(GameObject[] obst, Transform reference) {

        toReturn = null;

        foreach (GameObject obstacles in obst) {

            float currentDist = (reference.position - obstacles.transform.position).magnitude;
            //Debug.Log(currentDist);
            if (currentDist < range) {
                if (Random.value > 0.9) {
                    toReturn = obstacles;
                }
            }
        }
        return toReturn;
    }



    public Vector3 FurthestPoint(GameObject obs) {
        float distance = 0;
        Vector3[] listOfLocation;

        listOfLocation = new Vector3[4];
        listOfLocation[0] = obs.transform.position - new Vector3(0, 0, obs.GetComponent<BoxCollider>().bounds.extents.z + 0.5f);
        listOfLocation[1] = obs.transform.position + new Vector3(0, 0, obs.GetComponent<BoxCollider>().bounds.extents.z + 0.5f);
        listOfLocation[2] = obs.transform.position + new Vector3(obs.GetComponent<BoxCollider>().bounds.extents.x + 0.5f, 0, 0);
        listOfLocation[3] = obs.transform.position + new Vector3(obs.GetComponent<BoxCollider>().bounds.extents.x + 0.5f, 0, 0);

        foreach (Vector3 location in listOfLocation) {
            if ((target.position - location).sqrMagnitude > distance) {
                finalDestination = location;
                distance = (target.position - location).sqrMagnitude;
            }
        }

        if (finalDestination.z == obs.transform.position.z) {
            finalDestination += new Vector3(0, 0, Random.Range(-obs.transform.localScale.z, obs.transform.localScale.z));
        }

        if (finalDestination.x == obs.transform.position.x) {
            finalDestination += new Vector3(Random.Range(-obs.transform.localScale.x, obs.transform.localScale.x), 0, 0);
        }
        return finalDestination;
    }
}
