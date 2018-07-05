
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class collisionScript : MonoBehaviour
{
   
    public static int count = 3;
    public static int ememiesKilled;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //for this to work both need colliders, one must have rigid body (spaceship) the other must have is trigger checked.
    void OnTriggerEnter(Collider col)
    {
        
        if (col.tag == "Player"){
            GameObject explosion = Instantiate(Resources.Load("FlareMobile", typeof(GameObject))) as GameObject;
            explosion.transform.position = transform.position;
            Destroy(col.gameObject);
            Destroy(explosion, 2);
            Destroy(gameObject);
            ememiesKilled++;
        }



        if (GameObject.FindGameObjectsWithTag("Player").Length == 0 )
        {

            GameObject enemy = Instantiate(Resources.Load("enemy", typeof(GameObject))) as GameObject;
            GameObject enemy1 = Instantiate(Resources.Load("enemy1", typeof(GameObject))) as GameObject;
            GameObject enemy2 = Instantiate(Resources.Load("enemy2", typeof(GameObject))) as GameObject;
            GameObject enemy3 = Instantiate(Resources.Load("enemy3", typeof(GameObject))) as GameObject;

            count--;

        }

        if (count == 0)
        {
            SceneManager.LoadScene("FirstScene");
        }


    }

}
