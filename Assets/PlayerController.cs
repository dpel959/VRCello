using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TimingManager timingManager;

    private void Start()
    {
        if (timingManager == null)
            Debug.LogError("There is no Timing manager");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("space pressed");
            timingManager.CheckTiming(); 
        }
    }
}
