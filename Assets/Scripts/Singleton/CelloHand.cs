using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloHand : Singleton<CelloHand>
{
    public GameObject oculusHand;
    [SerializeField]
    GameObject visualHand;
    public bool isStringed = false;
    private void Awake()
    {
        isStringed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cello"))
        {
            oculusHand.SetActive(false);
            visualHand.SetActive(true);
            isStringed = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cello"))
        {
            oculusHand.SetActive(true);
            visualHand.SetActive(false);
            isStringed = false;
        }
    }
}
