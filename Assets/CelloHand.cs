using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloHand : MonoBehaviour
{
    public GameObject oculusHand;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StringToOculus"))
        {
            oculusHand.SetActive(false);
            other.GetComponent<OculusToString>().inOculusHand = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StringToOculus"))
        {
            oculusHand.SetActive(true);
            other.GetComponent<OculusToString>().inOculusHand = false;
        }
    }
}
