using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHand : Singleton<StringHand>
{
    Animator animator;
    [SerializeField]
    GameObject visualHand;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();    
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

        if (CelloHand.Instance.isStringed)
        {
            visualHand.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            visualHand.transform.localPosition = new Vector3(visualHand.transform.localPosition.x - 0.05f,
                visualHand.transform.localPosition.y - 0.07f, visualHand.transform.localPosition.z - 0.02f);
        }
    }
}
