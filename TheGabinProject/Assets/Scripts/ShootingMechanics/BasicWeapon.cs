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

    float ammoCount;
    float reloadTimer;
    bool reloadedYet;


    void Update() {
        Debug.Log(ammoCount);
        if (Input.GetKeyDown("q")) {
            if (weaponIndex < 3) {
                weaponIndex++;
            }
            ammoCount = 0;
            reloadTimer = Time.time;
            bulletType = (BulletTypes)weaponIndex;
        }

        if (Input.GetKeyDown("e")) {
            if (weaponIndex > 0) {
                weaponIndex--;
            }
            ammoCount = 0;
            reloadTimer = Time.time;
            bulletType = (BulletTypes)weaponIndex;
        }

        if (Input.GetKeyDown("r")) {
            switch (bulletType) {
                case BulletTypes.Frozen:
                case BulletTypes.Fire:
                case BulletTypes.Shock:
                    if (ammoCount !=5)
                    ammoCount = 0;
                    break;
                case BulletTypes.Normal:
                    if (ammoCount != 20)
                    ammoCount = 0;
                    break;

            }
        }

        if (ammoCount > 0) {

            if (Input.GetKey(KeyCode.LeftControl)) {
                if (shootCooldown <= Time.time) {

                    shootCooldown = Time.time + shootInterval;
                    storage = WeaponSpray(sprayValue);
                    Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, range) + storage, Color.red, 2f);
                    if (Physics.Raycast(transform.position + new Vector3(0, transform.localScale.y / 4, 0), transform.TransformDirection(0, 0, range) + storage, out hit)) {
                        ammoCount--;
                        if (currentTarget != null) {
                            if (!currentTarget.GetComponent<BaseClass>()) {
                                currentTarget = null;
                            }
                        }

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
                                    target.enemyUI.UpdateState("Frozen");
                                    StartCoroutine(target.Restoration(3));
                                    break;
                                case BulletTypes.Fire:
                                    StartCoroutine(target.Persistent(1, 20));
                                    target.enemyUI.UpdateState("Fire");
                                    break;
                                case BulletTypes.Shock:
                                    target.speed = 0;
                                    target.enemyUI.UpdateState("Shock");
                                    StartCoroutine(target.Restoration(2));
                                    break;
                            }
                        }
                        GameObject explosion = (GameObject)Instantiate(explosionEffect, hit.point, Quaternion.identity);
                        explosion.GetComponent<ExplosionEffectScript>().bulletState = bulletType.ToString();

                    }
                }
            }
        } else {
            if (reloadTimer < Time.time) {
                if (!reloadedYet) {
                    reloadTimer = Time.time + 2;
                    reloadedYet = true;
                } else {
                    reloadedYet = false;
                    switch (bulletType) {
                        case BulletTypes.Frozen:
                        case BulletTypes.Fire:
                        case BulletTypes.Shock:
                            ammoCount = 5;
                            break;
                        case BulletTypes.Normal:
                            ammoCount = 20;
                            break;
                    }
                }
            }
        }
    }

    public Vector3 WeaponSpray(float value) {
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }
}
