using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {

    public GameObject enemy;
    public Vector3  center;
    public Vector3 size;
    private const float SPAWNGAP = 0.8f; //1 sec
    private float mNextSpawnTime;
	// Use this for initialization
	void Start () {
        mNextSpawnTime = SPAWNGAP;
	}

    public void spawnEnemies() {
        mNextSpawnTime = SPAWNGAP;
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject en = Instantiate(enemy, pos, Quaternion.identity);
        float xSpin = Random.Range(0, 360);
        en.transform.rotation = Quaternion.Euler(0, xSpin, 0);

    }
	
	// Update is called once per frame
	void Update () {
       
        mNextSpawnTime -= Time.deltaTime;
           
        if (mNextSpawnTime < 0){
            spawnEnemies();

        }
           
  
	}


}
