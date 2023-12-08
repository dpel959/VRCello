using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFrame : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!AudioManagerScript.Instance.isMusicStart && (GameManager.Instance.currentStage == GameManager.Stage.Song))
        {
            if (other.CompareTag("Note"))
            {
                AudioManagerScript.Instance.PlayBGM("BGM1");
                AudioManagerScript.Instance.isMusicStart = true;
            }
        }
    }
}
