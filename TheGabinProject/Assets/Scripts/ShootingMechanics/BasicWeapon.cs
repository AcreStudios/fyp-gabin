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
    public float range;
    int weaponIndex;

    float shootCooldown;
    GameObject currentTarget;
    RaycastHit hit;
    BaseClass target;
    Vector3 storage;


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
                storage = WeaponSpray(sprayValue);
                Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, range) + storage, Color.red, 2f);
                if (Physics.Raycast(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, range) + storage, out hit)) {
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
                                target.speed = 0.5f;
                                StartCoroutine(target.Restoration(3));
                                break;
                            case BulletTypes.Fire:
                                StartCoroutine(target.Persistent(1,20));
                                break;
                            case BulletTypes.Shock:
                                target.speed = 0;
                                StartCoroutine(target.Restoration(2));
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
