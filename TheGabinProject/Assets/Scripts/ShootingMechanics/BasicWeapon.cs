﻿using UnityEngine;
using System.Collections;

public class BasicWeapon : MonoBehaviour {

	//[System.Serializable]
	public struct Weapons{
		public string weaponsName;
		public float weaponsDamage;
		public float weaponsAttackSpeed;
		public float weaponsSpray;
		public bool projectile;
	}

	public Weapons[] weapons;

    public float sprayValue;
    
    public float shootInterval;
    public float weaponDamage;
    public GameObject explosionEffect;

    float shootCooldown;
    GameObject currentTarget;
    RaycastHit hit;
    BaseClass target;
    

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if (shootCooldown <= Time.time)
            {
                // Debug.Log("WORKING");
                shootCooldown = Time.time + shootInterval;
                //Debug.DrawRay(transform.position, transform.TransformDirection(0, 0, 50) + WeaponSpray(sprayValue), Color.black, 2f);
                if (Physics.Raycast(transform.position, transform.TransformDirection(0, 0, 50) + WeaponSpray(sprayValue), out hit))
                {
                    Instantiate(explosionEffect, hit.point, Quaternion.identity);
                    if (currentTarget != hit.transform.gameObject)
                    {
                        if (hit.transform.root.gameObject.GetComponent<BaseClass>())
                        {
                            currentTarget = hit.transform.gameObject;
                            target = hit.transform.root.gameObject.GetComponent<BaseClass>();
                        }
                        else
                        {
                            currentTarget = null;
                        }
                    }
                    if (currentTarget != null)
                    {
                        target.DamageRecieved(weaponDamage, hit.transform.gameObject);
                    }
                }
            }
        }
    }

    public Vector3 WeaponSpray(float value){
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value),Random.Range(-value, value));
    }
}
