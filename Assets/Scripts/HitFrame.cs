using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFrame : MonoBehaviour
{
    AudioSource audioSource;
    bool musicStart = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!musicStart)
        {
            if (other.CompareTag("Note"))
            {
                audioSource.Play();
                musicStart = true;
            }
        }
    }
}
