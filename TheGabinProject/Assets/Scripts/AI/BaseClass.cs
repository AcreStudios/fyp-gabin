using UnityEngine;
using System.Collections;

public class BaseClass : MonoBehaviour {

    public float health;
    protected GameObject criticalPart;
    public float speed;
    public float speedA;
    protected bool shotAt;
    public bool destrutibles;
    public int ticks;

    public float Health {
        get {
            return this.health;
        }
        set {
            if (value <= 0) {
                if (destrutibles) {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
                    transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                    Destroy(this);
                } else {
                    Destroy(gameObject);
                }
            } else {
                health = value;
            }
        }
    }

    public void DamageRecieved(float weaponDamage, GameObject partHit) {

        if (criticalPart == partHit) {
            Destroy(gameObject);
        }
        shotAt = true;
        Health -= weaponDamage;
    }

    public void Frozen() {
        speed /= 2;
        GetComponent<Renderer>().material.color = Color.blue;
        StartCoroutine(Restoration(2));
    }

    public void Fire() {
        if (ticks >= 0) {
            Health -= 1;
            GetComponent<Renderer>().material.color = Color.red;
            StartCoroutine(Persistent(0.1f));
            ticks--;
        } 
        else {
            StartCoroutine(Restoration(0));
        }
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
        ticks = 20;
    }

    IEnumerator Persistent(float persistentTime) {
        yield return new WaitForSeconds(persistentTime);
        Fire();
    }
}
