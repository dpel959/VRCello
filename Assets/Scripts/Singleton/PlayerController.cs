using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
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
            timingManager.CheckTiming(); 
        }
    }
}
