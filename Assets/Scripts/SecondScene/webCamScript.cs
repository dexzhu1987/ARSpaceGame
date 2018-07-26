using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class webCamScript : MonoBehaviour {
    
    public static bool isAROn;
    public static bool isGameOn;

    public GameObject webCameraPlane;
    public Button fireButton;
    public GameObject restartButton;
    public GameObject exitButton;
    public Toggle arToggle;
    public VirtualJoyceStick leftJoyceStick;
    public VirtualJoyceStick rightJoyceStick;
    public GameObject lifeImage1;
    public GameObject lifeImage2;
    public GameObject lifeImage3;
    public Text warningText;
    private Animator warningTextAnimator;

    float totalTime = 60f; //2 minutes
    public Text timer;
    public Text mKilledLabel;

    public Vector3 center;
    public Vector3 size;

    private const float VOLUME = 0.5f;
    public AudioClip warning;
    public AudioClip laser;
    public AudioClip getItem;

    private int bulletsPerHit;
    private int lifies;
    private bool isGameOver;
    private int placeNumber;

    private TouchScreenKeyboard keyboard;
  

	public void Start () {

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
        arToggle.isOn = isAROn;
        isGameOn = true;
        isGameOver = false;
        collisionScript.ememiesKilled = 0;
        restartButton.SetActive(false);
        exitButton.SetActive(false);
        EnemiesController.allEnemiesNumber = 0;
        EnemiesController.allSuppliesNumber = 0;
        placeNumber = 0;

        warningTextAnimator = warningText.GetComponent<Animator>();
      

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
            isAROn = true;
        }else if (!arToggle.isOn){
            webCameraPlane.SetActive(false);
            isAROn = false;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        HandleCamera();
        if (isGameOn){
            HandleTime();
            string killedText = "Destoryed: " + collisionScript.ememiesKilled + "         Highest: ";
            int highest = PlayerPrefs.GetInt("first");
            if ( highest > collisionScript.ememiesKilled){
                killedText += highest;
            } else {
                killedText += collisionScript.ememiesKilled;
            }
            mKilledLabel.text = killedText;
        } 
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
            if (Application.isMobilePlatform){
                this.transform.Rotate(-90, 0, 0);
            }
            if (isGameOn){
                Vector3 movePostion = new Vector3(leftJoyceStick.Horizontal(), leftJoyceStick.Vertical(),rightJoyceStick.Vertical());
                Vector3 newPosition = transform.position + movePostion / 3;
                if (newPosition.x < xMaxRange && newPosition.x > xMinRange 
                && newPosition.y < yMaxRange && newPosition.y > yMinRange 
                && newPosition.z < zMaxRange && newPosition.z > zMinRange)
                {
                    transform.position = newPosition;
                    warningTextAnimator.SetBool("isAtEdge", false);
                } else {
                    warningTextAnimator.SetBool("isAtEdge", true);
                }   
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
            //SceneManager.LoadScene("FirstScene");
            isGameOn = false;
            ClearGameScene();
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

        if (other.tag == "Supply1")
        {
            bulletsPerHit++;
            suppliesRelatedMethod(other);
        }

        if (other.tag == "Supply2")
        {
            totalTime += 10f;
            suppliesRelatedMethod(other);
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
            suppliesRelatedMethod(other);
           
        }

        if (lifies == 0 )
        {
            isGameOn = false;
            //SceneManager.LoadScene("FirstScene");
            ClearGameScene();
        }

    }

    private void suppliesRelatedMethod(Collider other)
    {
        Destroy(other.gameObject);
        AudioSource.PlayClipAtPoint(getItem, transform.position, VOLUME);
        EnemiesController.allSuppliesNumber--;
    }

    private void ClearGameScene()
    {
        foreach (GameObject en in EnemiesController.allEnemies)
        {
            Destroy(en);
        }
        foreach (GameObject su in EnemiesController.allSupplies)
        {
            Destroy(su);
        }
        EnemiesController.allEnemies.Clear();
        EnemiesController.allSupplies.Clear();
        if (!isGameOver){
            GameObject gameover1 = Instantiate(Resources.Load("GameOver", typeof(GameObject))) as GameObject;
            isGameOver = true;
            int firstPlaceInt = PlayerPrefs.GetInt("first");
            int secondPlaceInt = PlayerPrefs.GetInt("second");
            int thirdPlaceInt = PlayerPrefs.GetInt("third");
            if (collisionScript.ememiesKilled > firstPlaceInt)
            {
                placeNumber = 1;
                PlayerPrefs.SetInt("first", collisionScript.ememiesKilled);
                if (Application.isMobilePlatform)
                {
                    keyboard = TouchScreenKeyboard.Open("first", TouchScreenKeyboardType.Default);

                }
                GameObject place = Instantiate(Resources.Load("FirstPlace", typeof(GameObject))) as GameObject;
            }
            else if (collisionScript.ememiesKilled > secondPlaceInt)
            {
                placeNumber = 2;
                PlayerPrefs.SetInt("second", collisionScript.ememiesKilled);
                if (Application.isMobilePlatform)
                {
                    keyboard = TouchScreenKeyboard.Open("second", TouchScreenKeyboardType.Default);

                }
                GameObject place = Instantiate(Resources.Load("SecondPlace", typeof(GameObject))) as GameObject;
            }
            else if (collisionScript.ememiesKilled > thirdPlaceInt)
            {
                placeNumber = 3;
                PlayerPrefs.SetInt("third", collisionScript.ememiesKilled);
                if (Application.isMobilePlatform)
                {
                    keyboard = TouchScreenKeyboard.Open("third", TouchScreenKeyboardType.Default);

                }
                GameObject place = Instantiate(Resources.Load("ThirdPlace", typeof(GameObject))) as GameObject;
            }
           
           
        }
        bulletsPerHit = 1;
        Camera.main.transform.position = Vector3.zero;
        restartButton.SetActive(true);
        exitButton.SetActive(true);
     
      


    }


    private void OnGUI()
    {
        TouchScreenKeyboard.Status status = keyboard.status;
        if (status == TouchScreenKeyboard.Status.Done){
            if (placeNumber ==1) {
                PlayerPrefs.SetString("firstName", keyboard.text);
            } else if (placeNumber == 2){
                PlayerPrefs.SetString("secondName", keyboard.text);
            } else if (placeNumber == 3) {
                PlayerPrefs.SetString("thirdName", keyboard.text);
            }

        }
    }

    public void ReStartGame(){
        SceneManager.LoadScene("ARTextScene");
        webCamScript.isAROn = arToggle.isOn;
    }

    public void Exit()
    {
        SceneManager.LoadScene("FirstScene");
    }
}
