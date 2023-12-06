using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloHand : Singleton<CelloHand>
{
    public GameObject oculusHand;
    public bool isStringed = false;
    public int whatString = int.MaxValue;
    private void Awake()
    {
        isStringed = false;
        whatString = int.MaxValue;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StringToOculus"))
        {
            oculusHand.SetActive(false);
            isStringed = true;
            other.GetComponent<OculusToString>().inOculusHand = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StringToOculus"))
        {
            oculusHand.SetActive(true);
            isStringed = false;
            other.GetComponent<OculusToString>().inOculusHand = false;
        }
    }
}
