using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    public bool isCelloHandAttached = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CelloHand"))
            isCelloHandAttached = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CelloHand"))
            isCelloHandAttached = false;
    }
}
