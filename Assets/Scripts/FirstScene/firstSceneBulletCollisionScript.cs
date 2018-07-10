using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class firstSceneBulletCollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider col)
    {

        if (col.tag == "StartGame")
        {
            GameObject explosion = Instantiate(Resources.Load("SmallExplosionEffect", typeof(GameObject))) as GameObject;
            explosion.transform.position = transform.position;
            Destroy(col.gameObject);
            Destroy(explosion, 2);
            StartCoroutine(WaitForIt(2f));
            collisionScript.ememiesKilled = 0;

           
        }

    }

    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("ARTextScene");
    }

}
