using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoyceStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler{

    public Image bgImage;
    public Image joystickImg;
    public  Vector3 inputVector;

	// Use this for initialization
	void Start () {
        joystickImg = transform.GetChild(0).GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void OnPointerDown(PointerEventData ped){
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform, ped.position,ped.pressEventCamera,out pos)){
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.01) ? inputVector.normalized : inputVector;

            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImage.rectTransform.sizeDelta.x/3),inputVector.z * (bgImage.rectTransform.sizeDelta.y / 3));
        }
    }

    public float Horizontal(){
        if (inputVector.x != 0){
            return inputVector.x;
        } else {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
