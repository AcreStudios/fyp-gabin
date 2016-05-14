using UnityEngine;

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

    protected BaseClass sTarget;
    protected GameObject currentTarget;

    public GameObject explosionEffect;

    protected bool useNavMesh;
    protected NavMeshAgent agent;
    Vector3 finalDestination;

    public void Start() {
        //Debug.Log(useNavMesh);
        agent = GetComponent<NavMeshAgent>();
        if (!useNavMesh) {
            agent.enabled = false;
        }
    }

    public void Movement(Vector3 value, float speed) {
        if (useNavMesh) {
            if (speed > 0) {
                agent.speed = speed;
                agent.acceleration = speed;
                agent.destination = value;
                agent.angularSpeed = 360;
                //Debug.Log("Movement is true");
            }
        } else
            transform.position = Vector3.MoveTowards(transform.position, value, speed);

    }

    public void Shooting() {

        Vector3 storage;

        storage = WeaponSpray(sprayValue);
        Debug.DrawRay(transform.position, transform.TransformDirection(0, 0, 50) + storage, Color.black, 2f);

        if (Physics.Raycast(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, 50) + storage, out hit)) {
            if (currentTarget != null) {
                if (!currentTarget.GetComponent<BaseClass>()) {
                    currentTarget = null;
                }
            }

            Instantiate(explosionEffect, hit.point, Quaternion.identity);

            if (currentTarget != hit.transform.gameObject) {
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

        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }

    public bool Combat() {

        if (ammoCount > 0) {
            if (timer < Time.time) {
                transform.LookAt(target);
                Shooting();
                ammoCount--;
                timer = Time.time + 0.1f;
            }
            return true;
        } else {
            if (!reloading) {
                timer = Time.time + 1.0f;
                reloading = true;
            }
            if (timer < Time.time) {
                ammoCount = 20;
                reloading = false;
            }
            return false;
        }
    }

    public Vector3 FurthestPoint(GameObject obs) {
        float distance = 0;
        Vector3[] listOfLocation;

        listOfLocation = new Vector3[4];
        listOfLocation[0] = obs.transform.position - new Vector3(0, 0, obs.GetComponent<BoxCollider>().bounds.extents.z + 5);
        listOfLocation[1] = obs.transform.position + new Vector3(0, 0, obs.GetComponent<BoxCollider>().bounds.extents.z + 5);
        listOfLocation[2] = obs.transform.position + new Vector3(obs.GetComponent<BoxCollider>().bounds.extents.x + 5, 0, 0);
        listOfLocation[3] = obs.transform.position + new Vector3(obs.GetComponent<BoxCollider>().bounds.extents.x + 5, 0, 0);

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
