using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloBowScript : MonoBehaviour
{
    private Vector3 prePosition = new Vector3(0,0,0);
    
    public void SetPrePosition()
    {
        prePosition = transform.localPosition;
    }

    public Vector3 GetPrePosition()
    {
        return prePosition;
    }
}
