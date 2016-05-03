using UnityEngine;

using System.Collections;

public class BasicAI : BaseClass {

    public RaycastHit hit;

    // Use this for initialization

    public void Movement(Vector3 value, float speed) {
        transform.position = Vector3.MoveTowards(transform.position, value, speed);
    }

    public void Shooting(/*float damage,*/ float sprayValue) {
        Vector3 storage;

        storage = WeaponSpray(sprayValue);
        Debug.DrawRay(transform.position, transform.TransformDirection(0, 0, 50) + storage, Color.black, 2f);

        if (Physics.Raycast(transform.position, transform.TransformDirection(0, 0, 50) + storage, out hit)){
            //Debug.Log(hit.transform.gameObject);
        }
    }

    public Vector3 WeaponSpray(float value) {
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }


}
