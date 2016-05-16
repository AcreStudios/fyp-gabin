using UnityEngine;
using System.Collections;

public class BaseClass : MonoBehaviour {

    public float health;
    protected GameObject criticalPart;
    public float speed;
    protected bool shotAt;
    public bool destrutibles;

    float originalSpeed;

    public float Health {
        get {
            return this.health;
        }
        set {
            if (value <= 0) {
                if (destrutibles) {
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
                    transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
                    GetComponent<Renderer>().material.color = Color.black;
                    transform.tag = "Untagged";
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

    public IEnumerator Restoration(float statusTime) {
        yield return new WaitForSeconds(statusTime);
        speed = 1;
    }

    public IEnumerator Persistent(float healthReduction, float ticks) {
        if (ticks > 0) {
            yield return new WaitForSeconds(0.1f);
            Health -= healthReduction;
            ticks--;
            StartCoroutine(Persistent(healthReduction, ticks));
        }
    }
}
