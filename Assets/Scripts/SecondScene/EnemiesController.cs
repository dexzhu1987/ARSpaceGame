using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour {

    public GameObject enemy;
    public GameObject supply1;
    public GameObject supply2;
    public GameObject supply3;
    public GameObject initEnemy1;
    public GameObject initEnemy2;
    public GameObject initEnemy3;
    public Vector3  center;
    public Vector3 size;
    private const float SPAWNGAP = 0.8f; //0.8 sec
    private float mNextSpawnTime;
    private const float SUPPLYSPAWANGAP = 3f;
    private float mNextSupplySpawnTime;
    public static List<GameObject> allEnemies;
    public static List<GameObject> allSupplies;

	// Use this for initialization
	void Start () {
        mNextSpawnTime = SPAWNGAP;
        mNextSupplySpawnTime = SUPPLYSPAWANGAP;
        allEnemies = new List<GameObject>();
        allEnemies.Add(initEnemy1);
        allEnemies.Add(initEnemy2);
        allEnemies.Add(initEnemy3);
        allSupplies = new List<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
 
        if (webCamScript.isGameOn){
            mNextSpawnTime -= Time.deltaTime;
            mNextSupplySpawnTime -= Time.deltaTime;
            if (mNextSpawnTime < 0)
            {
                spawnEnemies();
            }


            if (mNextSupplySpawnTime < 0 && !webCamScript.isAROn)
            {
                spawnSupplies();
            }
        }
       
	}

    public void spawnEnemies()
    {
        mNextSpawnTime = SPAWNGAP;
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        GameObject en = Instantiate(enemy, pos, Quaternion.identity);
        float xSpin = Random.Range(0, 360);
        en.transform.rotation = Quaternion.Euler(0, xSpin, 0);
        allEnemies.Add(en);
    }

    public void spawnSupplies(){
        mNextSupplySpawnTime = SUPPLYSPAWANGAP;
        Vector3 pos = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(supply1);
        gameObjects.Add(supply2);
        gameObjects.Add(supply3);
        int supplyNumber = 0;
        int needToReroll = 0;
        do
        {
            supplyNumber = Random.Range(0, gameObjects.Count);
            needToReroll = Random.Range(0, 2); //lower the supply3 chance to 1/2 * 1/3
        } while (supplyNumber == (gameObjects.Count-1) && needToReroll != 0); 

        GameObject su = Instantiate(gameObjects[supplyNumber], pos, Quaternion.identity);
        allSupplies.Add(su);
    }


}
