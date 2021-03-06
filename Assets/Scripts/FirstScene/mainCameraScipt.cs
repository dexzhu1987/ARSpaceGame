﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainCameraScipt : MonoBehaviour {


    public Button fireButton;
    public Text firstPlace;
    public Text secondPlace;
    public Text thirdPlace;


	// Use this for initialization
	void Start () {
        if (Application.isMobilePlatform)
        {
            GameObject cameraParent = new GameObject("camParent");
            cameraParent.transform.position = this.transform.position;
            this.transform.parent = cameraParent.transform;
            cameraParent.transform.Rotate(Vector3.right, 90);
        }

        Input.gyro.enabled = true;

        int firstPlaceInt = PlayerPrefs.GetInt("first");
        string firstName = PlayerPrefs.GetString("firstName");

        firstPlace.text = "1). " + HandleName(firstName) + "       " + firstPlaceInt;

        int secondPlaceInt = PlayerPrefs.GetInt("second");
        string secondName = PlayerPrefs.GetString("secondName");
        secondPlace.text = "2). "+ HandleName(secondName) + "  " + secondPlaceInt;

        int thirdPlaceInt = PlayerPrefs.GetInt("third");
        string thirdName = PlayerPrefs.GetString("thirdName");
        thirdPlace.text = "3). "+ HandleName(thirdName)+"  "+ thirdPlaceInt;
    
        fireButton.onClick.AddListener(OnButtonDown);
    
       
	}

    private string HandleName(string name){
        if (name.Length == 0 ){
            return "DEX";
        } else if (name.Length > 0 && name.Length <= 5){
            return name;
        } else if (name.Length > 5){
            return name.Substring(0, 5);
        }
        return "DEX";
    }

    void OnButtonDown()
    {

        GameObject bullet = Instantiate(Resources.Load("bullet1", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = Camera.main.transform.rotation;
        bullet.transform.position = Camera.main.transform.position;
        rb.AddForce(Camera.main.transform.forward * 500f);
        Destroy(bullet, 5);

        GetComponent<AudioSource>().Play();
  

    }
	
	// Update is called once per frame
	void Update () {
        //Camera.main.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);

        Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        this.transform.localRotation = cameraRotation;
	}
}
