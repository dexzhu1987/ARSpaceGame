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
    public Image lifeImage1;
    public Image lifeImage2;
    public Image lifeImage3;

    float totalTime = 60f; //2 minutes
    public Text timer;
    public Text mKilledLabel;

    public Vector3 center;
    public Vector3 size;
    public int lifies = 3;
    public List<Image> lifiesList = new List<Image>();

    public AudioClip warning;

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


        lifiesList.Add(lifeImage1);
        lifiesList.Add(lifeImage2);
        lifiesList.Add(lifeImage3);
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
  
        float xMaxRange = center.x + size.x / 2 + 1;
        float xMinRange = center.x - size.x / 2 - 1;
        float yMaxRange = center.y + size.y / 2 + 1;
        float yMinRange = center.y - size.y / 2 - 1;
        float zMaxRange = center.z + size.z / 2 + 1;
        float zMinRange = center.z - size.z / 2 - 1;
        if (!arToggle.isOn){
            Vector3 movePostion = new Vector3(leftJoyceStick.Horizontal(), leftJoyceStick.Vertical(),rightJoyceStick.Vertical());
            Vector3 newPosition = transform.position + movePostion / 3;
            if (newPosition.x < xMaxRange && newPosition.x > xMinRange 
                && newPosition.y < yMaxRange && newPosition.y > yMinRange 
                && newPosition.z < zMaxRange && newPosition.z > zMinRange)
            {
                transform.position = newPosition;
            } 
        } else {
            transform.position = Vector3.zero;
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

            if (lifiesList.Count > 0) {
                Destroy(lifiesList[0]);
                lifiesList.RemoveAt(0);
            }

            lifies--;

          
        } 

        if (lifies == 0 ){
            SceneManager.LoadScene("FirstScene");
        }

    }
}
