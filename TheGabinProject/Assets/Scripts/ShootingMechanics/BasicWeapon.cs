using UnityEngine;
using System.Collections;

public class BasicWeapon : MonoBehaviour {

    public enum BulletTypes {
        Frozen,
        Fire,
        Shock,
        Normal
    }

    public float sprayValue;
    public float shootInterval;
    public float weaponDamage;
    public GameObject explosionEffect;
    public BulletTypes bulletType;
    int weaponIndex;

    float shootCooldown;
    GameObject currentTarget;
    RaycastHit hit;
    BaseClass target;


    void Update() {

        if (Input.GetKeyDown("q")) {
            if (weaponIndex < 3) {
                weaponIndex++;
            }
            bulletType = (BulletTypes)weaponIndex;
        }

        if (Input.GetKeyDown("e")) {
            if (weaponIndex > 0) {
                weaponIndex--;
            }
            bulletType = (BulletTypes)weaponIndex;
        }
       
        if (Input.GetMouseButton(0)) {
            if (shootCooldown <= Time.time) {

                shootCooldown = Time.time + shootInterval;
                if (Physics.Raycast(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, 50) + WeaponSpray(sprayValue), out hit)) {
                    if (currentTarget != null) {
                        if (!currentTarget.GetComponent<BaseClass>()) {
                            currentTarget = null;
                        }
                    }

                    Instantiate(explosionEffect, hit.point, Quaternion.identity);
                    if (currentTarget != hit.transform.gameObject) {
                        if (hit.transform.root.gameObject.GetComponent<BaseClass>()) {
                            currentTarget = hit.transform.gameObject;
                            target = hit.transform.root.gameObject.GetComponent<BaseClass>();
                        } else {
                            currentTarget = null;
                        }
                    }
                    if (currentTarget != null) {
                        
                        target.DamageRecieved(weaponDamage, hit.transform.gameObject);

                        switch (bulletType) {
                            case BulletTypes.Frozen:
                                target.Frozen();
                                break;
                            case BulletTypes.Fire:
                                target.Fire();
                                break;
                            case BulletTypes.Shock:
                                target.Shock();
                                break;
                        }
                    }
                }
            }
        }
    }

    public Vector3 WeaponSpray(float value) {
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }
}
