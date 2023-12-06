using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusToString : MonoBehaviour
{
    public GameObject stringHand;
    public bool inOculusHand = false;
    void Update()
    {
        
        if (inOculusHand)
        {
            stringHand.transform.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            stringHand.transform.localPosition = new Vector3(stringHand.transform.localPosition.x-0.05f,
                stringHand.transform.localPosition.y-0.07f, stringHand.transform.localPosition.z-0.02f);
        }
    }
}
