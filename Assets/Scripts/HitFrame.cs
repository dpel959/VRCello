using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFrame : MonoBehaviour
{
    bool musicStart = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!musicStart)
        {
            if (other.CompareTag("Note"))
            {
                //AudioManagerScript.Instance.PlayBGM("BGM1");
                musicStart = true;
            }
        }
    }
}
