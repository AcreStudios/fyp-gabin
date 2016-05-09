using UnityEngine;
using System.Collections;

public class BaseClass : MonoBehaviour {

    public float health;
    public GameObject criticalPart;
    public float speed;
    public float speedA;

    void Start() {
    }

    public void DamageRecieved(float weaponDamage, GameObject partHit) {
        //Debug.Log(partHit);
        if (criticalPart == partHit) {
            Destroy(gameObject);
        }

        health -= weaponDamage;

        if (health <= 0){
            Destroy(gameObject);
        }
    }

    public void Frozen() {
        speed /= 2;
        GetComponent<Renderer> ().material.color = Color.blue;

        StartCoroutine(Restoration(2));
    }

    public void Fire() {
        health -= 25;
        GetComponent<Renderer>().material.color = Color.red;

        StartCoroutine(Restoration(3));
    }

    public void Shock() {
        speed = 0;
        GetComponent<Renderer>().material.color = Color.yellow;

        StartCoroutine(Restoration(0.5f));
    }

    IEnumerator Restoration(float statusTime) {
        yield return new WaitForSeconds(statusTime);
        speed = speedA;
        GetComponent<Renderer>().material.color = Color.white;
    }


}
