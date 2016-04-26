using UnityEngine;
using System.Collections;

public class BasicEnemySpawner : MonoBehaviour {

    public Transform player;
    public GameObject enemy;
    public float spawnArea;

    float spawnTimer;
    public float spawnCooldown;
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
        if (spawnTimer <= Time.time){
            spawnTimer = Time.time;
            spawnTimer += spawnCooldown;
            Instantiate(enemy, SpawnEnemy(player.position), Quaternion.identity);
        }
        
	
	}

    public Vector3 SpawnEnemy(Vector3 value){

        return new Vector3(value.x + Random.Range(-spawnArea, spawnArea), value.y,value.z + Random.Range(-spawnArea, spawnArea));
    }
}
