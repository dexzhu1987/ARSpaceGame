using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstSceneSound : MonoBehaviour {

    public AudioSource mAudio;
    public AudioClip bcgMusic;
    public AudioClip annoucer; 
	// Use this for initialization
	void Start () {
        mAudio.loop = true;
        mAudio.clip = bcgMusic;
        mAudio.volume = 0.5f;
        mAudio.Play();

        AudioSource.PlayClipAtPoint(annoucer, transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
