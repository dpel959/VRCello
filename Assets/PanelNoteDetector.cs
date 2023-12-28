using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNoteDetector : MonoBehaviour
{
    public CelloStringScript celloString;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PanelCollider"))
        {
            celloString.LongNoteCollide();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("PanelColliderEnd"))
        {
            celloString.LongNoteCollideEnd();
            other.gameObject.SetActive(false);
        }
    }
}
