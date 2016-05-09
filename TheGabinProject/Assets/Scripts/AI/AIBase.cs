using UnityEngine;

using System.Collections;

public class AIBase : BaseClass {

    public RaycastHit hit;
    public bool reloading;
    public float timer;
    public float ammoCount;
    public Transform target;

    // Use this for initialization

    public void Movement(Vector3 value, float speed) {

        transform.position = Vector3.MoveTowards(transform.position, value, speed);
    }

    public void Shooting(/*float damage,*/ float sprayValue) {

        Vector3 storage;

        storage = WeaponSpray(sprayValue);
        Debug.DrawRay(transform.position, transform.TransformDirection(0, 0, 50) + storage, Color.black, 2f);

        if (Physics.Raycast(transform.position, transform.TransformDirection(0, 0, 50) + storage, out hit))
        {
            //Debug.Log(hit.transform.gameObject);
        }
    }

    public Vector3 WeaponSpray(float value) {

        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }

    public bool Combat() {

        if (ammoCount > 0)
        {
            if (timer < Time.time)
            {
                transform.LookAt(target);
                Shooting(1);
                ammoCount--;
                timer = Time.time + 0.1f;
                //Debug.Log("Firing, bullets left: " + ammoCount);
            }
            return true;
        }
        else
        {
            if (!reloading)
            {
                //Debug.Log("Reloading");
                timer = Time.time + 1.0f;
                reloading = true;
            }
            if (timer < Time.time)
            {
                ammoCount = 20;
                reloading = false;
            }
            return false;
        }
    }
}
