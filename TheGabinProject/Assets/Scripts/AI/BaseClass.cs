using UnityEngine;
using System.Collections;

public class BaseClass : MonoBehaviour {


    public float health;
    public GameObject criticalPart;

    public void DamageRecieved(float weaponDamage, GameObject partHit) {

        if (criticalPart == partHit) {
            Destroy(gameObject);
        }

        health -= weaponDamage;

        if (health <= 0){
            Destroy(gameObject);
        }
    }


}
