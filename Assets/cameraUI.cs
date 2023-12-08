using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraUI : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Camera.main.transform.rotation;

        gameObject.transform.position = Camera.main.transform.position;
    }
}
