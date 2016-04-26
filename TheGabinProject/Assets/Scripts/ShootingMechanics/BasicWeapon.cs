using UnityEngine;
using System.Collections;

public class BasicWeapon : MonoBehaviour {

	[System.Serializable]
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
    HealthScript target;
     
    

	void Start () {
        //weapons[0].weaponsName = "HEY";
        //weapons[1].weaponsName = "HEY";

       // Debug.Log(weapons[0].weaponsName);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            if (shootCooldown <= Time.time)
            {
                //Debug.Log("WORKING");
                Vector3 weaponSprayed;
                weaponSprayed = transform.TransformDirection(0, 0, 50) + WeaponSpray(sprayValue);
                shootCooldown += shootInterval;
                Debug.DrawRay(transform.position, weaponSprayed, Color.red, 0.25f);
                if (Physics.Raycast(transform.position,weaponSprayed, out hit))
                {
                    // Debug.Log("HITTING");
                    Instantiate(explosionEffect, hit.point, Quaternion.identity);
                    if (currentTarget != hit.transform.gameObject)
                    {
                        if (hit.transform.gameObject.GetComponent<HealthScript>())
                        {
                            currentTarget = hit.transform.gameObject;
                            target = hit.transform.gameObject.GetComponent<HealthScript>();
                            //Debug.Log("Game Object has AI Script");
                        }
                        else
                        {
                            currentTarget = null;
                        }
                    }
                    if (currentTarget != null)
                    {
                        target.health -= weaponDamage;
                    }
                }
            }
        }
    }

    public Vector3 WeaponSpray(float value){
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value),Random.Range(-value, value));
    }
}
