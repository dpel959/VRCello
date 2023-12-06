using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHand : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();    
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            animator.SetBool("XButton", true);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            animator.SetBool("YButton", true);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            animator.SetBool("Trigger", true);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger))
        {
            animator.SetBool("Grab", true);
        }
        if (OVRInput.GetUp(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            animator.SetBool("XButton", false);
        }
        if (OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            animator.SetBool("YButton", false);
        }
        if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            animator.SetBool("Trigger", false);
        }
        if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger))
        {
            animator.SetBool("Grab", false);
        }
    }
}
