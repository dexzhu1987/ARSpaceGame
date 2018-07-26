using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashAnimationScript : MonoBehaviour {

    private Animator m_Animator;
    public static bool isHit;

    void Start()
    {
        //This gets the Animator, which should be attached to the GameObject you are intending to animate.
        m_Animator = gameObject.GetComponent<Animator>();
        // The GameObject cannot jump
        isHit = false;
    }

    public void Update()
    {
        if (isHit){
            m_Animator.SetBool("isHit", true);
            StartCoroutine(SetBack());
        }
    }

    IEnumerator SetBack()
    {

            yield return new WaitForSeconds(1f);
            isHit = false;
            m_Animator.SetBool("isHit", false);
           

    }
}
