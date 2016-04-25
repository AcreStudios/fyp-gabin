using UnityEngine;
using System.Collections;

public class BasicEnemySpawner : MonoBehaviour {

    public Transform player;
    public GameObject enemy;
    public float spawnArea;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Instantiate(enemy, SpawnEnemy(player.position), Quaternion.identity);
	
	}

    public Vector3 SpawnEnemy(Vector3 value){

        return new Vector3(value.x + Random.Range(-spawnArea, spawnArea), value.y,value.z + Random.Range(-spawnArea, spawnArea));
    }
}
