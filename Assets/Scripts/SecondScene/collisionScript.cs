﻿
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class collisionScript : MonoBehaviour
{
    private const float VOLUME = 0.7f;
    public AudioClip explosionSound;
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
            GameObject explosion = Instantiate(Resources.Load("SmallExplosionEffect", typeof(GameObject))) as GameObject;
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            explosion.transform.position = transform.position;
            Destroy(col.gameObject);
            Destroy(explosion, 2);
            Destroy(gameObject);
            ememiesKilled++;
            EnemiesController.allEnemiesNumber--;
        }
       
    }

  
}
