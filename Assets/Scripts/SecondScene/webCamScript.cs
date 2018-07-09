﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class webCamScript : MonoBehaviour {

    public GameObject webCameraPlane;
    public Button fireButton;
    public Toggle arToggle;
    public VirtualJoyceStick joyceStick;
    public Text timer;
    public Text mKilledLabel;
    float totalTime = 60f; //2 minutes
	// Use this for initialization
	protected void Start () {

        if (Application.isMobilePlatform)
        {
            GameObject cameraParent = new GameObject("camParent");
            cameraParent.transform.position = this.transform.position;
            this.transform.parent = cameraParent.transform;
            cameraParent.transform.Rotate(Vector3.right, 90);
        }

        Input.gyro.enabled = true;

        WebCamTexture webCameraTexture = new WebCamTexture();
        webCameraPlane.GetComponent<MeshRenderer>().material.mainTexture = webCameraTexture;
        webCameraTexture.Play();
       
        fireButton.onClick.AddListener(OnButtonDown);
        arToggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(arToggle);
        });



	}
	
    void OnButtonDown()
    {

        GameObject bullet = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.rotation = Camera.main.transform.rotation;
        bullet.transform.position = Camera.main.transform.position;
        rb.AddForce(Camera.main.transform.forward * 500f);
        Destroy(bullet, 3);

    }

    void ToggleValueChanged(Toggle change)
    {
        if (arToggle.isOn){
            webCameraPlane.SetActive(true);

        }else if (!arToggle.isOn){
            webCameraPlane.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        HandleCamera();
        HandleTime();
        mKilledLabel.text = "Destoryed: " + collisionScript.ememiesKilled;
    }

    private void HandleCamera()
    {
        Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
        this.transform.localRotation = cameraRotation;
        if (!arToggle.isOn){
            Vector3 cameraPosition = new Vector3(joyceStick.Horizontal(),joyceStick.Horizontal(), joyceStick.Vertical());
            transform.position = cameraPosition;

        }
    }
     
    private void HandleTime()
    {
        totalTime -= Time.deltaTime;
        if (totalTime > 0)
        {
            UpdateLevelTimer(totalTime);
        }

        if (totalTime < 0)
        {
            SceneManager.LoadScene("FirstScene");
        }
    }

    public void UpdateLevelTimer(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.RoundToInt(totalSeconds % 60f);

        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        timer.text = minutes.ToString("00") + ":" + seconds.ToString("00"); 
    }
}
