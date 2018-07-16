using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class webCamScript : MonoBehaviour {

    public GameObject webCameraPlane;
    public Button fireButton;
    public Toggle arToggle;
    public VirtualJoyceStick leftJoyceStick;
    public VirtualJoyceStick rightJoyceStick;
    public GameObject lifeImage1;
    public GameObject lifeImage2;
    public GameObject lifeImage3;

    float totalTime = 60f; //2 minutes
    public Text timer;
    public Text mKilledLabel;

    public Vector3 center;
    public Vector3 size;

    public int bulletsPerHit;
    public int lifies;

    public AudioClip warning;
    private const float VOLUME = 0.5f;
    public AudioClip laser;
    public AudioClip getItem;

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

        bulletsPerHit = 1;
        lifies = 3;
 
	}
	
    void OnButtonDown()
    {
        
            GameObject bullet = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.transform.rotation = Camera.main.transform.rotation;
            bullet.transform.position = Camera.main.transform.position;
            rb.AddForce(Camera.main.transform.forward * 500f);
            Destroy(bullet, 1.5f);
            AudioSource.PlayClipAtPoint(laser, transform.position, VOLUME);

         if (bulletsPerHit == 2)
        {
            Spawn15DegreeBullets();
        } else if (bulletsPerHit > 2) {
            Spawn15DegreeBullets();
            Spawn30DegreeBullets();
        }


    }

    private static void Spawn15DegreeBullets()
    {

        GameObject bullet1 = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        GameObject bullet2 = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        bullet1.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y + 15, Camera.main.transform.rotation.z);
        bullet1.transform.position = Camera.main.transform.position;
        bullet2.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y - 15, Camera.main.transform.rotation.z);
        bullet2.transform.position = Camera.main.transform.position;
        rb1.AddForce(bullet1.transform.forward * 500f);
        rb2.AddForce(bullet2.transform.forward * 500f);
        Destroy(bullet1, 1.5f);
        Destroy(bullet2, 1.5f);
    }

    private static void Spawn30DegreeBullets()
    {
          
        GameObject bullet1 = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        GameObject bullet2 = Instantiate(Resources.Load("bullet", typeof(GameObject))) as GameObject;
        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        bullet1.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y + 30, Camera.main.transform.rotation.z);
        bullet1.transform.position = Camera.main.transform.position;
        bullet2.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.x, Camera.main.transform.rotation.y - 30, Camera.main.transform.rotation.z);
        bullet2.transform.position = Camera.main.transform.position;
        rb1.AddForce(bullet1.transform.forward * 500f);
        rb2.AddForce(bullet2.transform.forward * 500f);
        Destroy(bullet1, 1.5f);
        Destroy(bullet2, 1.5f);
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

        float xMaxRange = center.x + size.x / 2 + 1;
        float xMinRange = center.x - size.x / 2 - 1;
        float yMaxRange = center.y + size.y / 2 + 1;
        float yMinRange = center.y - size.y / 2 - 1;
        float zMaxRange = center.z + size.z / 2 + 1;
        float zMinRange = center.z - size.z / 2 - 1;

        if (arToggle.isOn){
            transform.position = Vector3.zero;

            Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            this.transform.localRotation = cameraRotation;

       
        } else {
            this.transform.localRotation = Quaternion.identity;
            this.transform.Rotate(-90, 0, 0);
            Vector3 movePostion = new Vector3(leftJoyceStick.Horizontal(), leftJoyceStick.Vertical(),rightJoyceStick.Vertical());
            Vector3 newPosition = transform.position + movePostion / 3;
            if (newPosition.x < xMaxRange && newPosition.x > xMinRange 
                && newPosition.y < yMaxRange && newPosition.y > yMinRange 
                && newPosition.z < zMaxRange && newPosition.z > zMinRange)
            {
                transform.position = newPosition;
            } 
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

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
           


            AudioSource.PlayClipAtPoint(warning, transform.position);

            if (lifies  == 3) {
                lifeImage3.SetActive(false);
            } else if (lifies == 2)
            {
                lifeImage2.SetActive(false);
            } else if (lifies == 1) {
                lifeImage1.SetActive(false);
            }
            Handheld.Vibrate();
            lifies--;
            flashAnimationScript.isHit = true;
        
        } 

        if (other.tag == "Supply1") {
            bulletsPerHit++;
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(getItem, transform.position, VOLUME);
        }

        if (other.tag == "Supply2")
        {
            totalTime += 10f;
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(getItem, transform.position, VOLUME);
        }

        if (other.tag == "Supply3")
        {
            
            if (lifies == 2) {
                lifies++;
                lifeImage3.SetActive(true);

            } else if (lifies == 1) {
                lifies++;
                lifeImage2.SetActive(true);
             
            }
            Destroy(other.gameObject);
            AudioSource.PlayClipAtPoint(getItem, transform.position, VOLUME);
           
        }

        if (lifies == 0 ){
            SceneManager.LoadScene("FirstScene");
        }

    }



}
