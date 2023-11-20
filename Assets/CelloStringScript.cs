using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public CelloBowScript celloBow;
    public DebugTextScript debugText;
    private Renderer stringRenderer;
    private Vector3 bowPrePos = new Vector3(0, 0, 0);
    private Vector3 bowLocalPos = new Vector3(0, 0, 0);

    private void Start()
    {
        stringRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowPrePos = collision.transform.localPosition;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bow")){
            bowLocalPos = celloBow.transform.localPosition;
            if (bowLocalPos.x > bowPrePos.x)
            {
                stringRenderer.material.color = new Color(255, 0, 0);
                //debugText.SetDebugText("localpos :" + celloBow.transform.localPosition.y.ToString());
                //debugText.SetDebugText("prePos :" + celloBow.GetPrePosition().y.ToString());
            }
            if (bowLocalPos.x < bowPrePos.x)
            {
                stringRenderer.material.color = new Color(0, 0, 255);
                //debugText.SetDebugText("localpos :" + celloBow.transform.localPosition.y.ToString());
                //debugText.SetDebugText("prePos :" + celloBow.GetPrePosition().y.ToString());
            }
            bowPrePos = bowLocalPos;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            stringRenderer.material.color = new Color(0, 0, 0);
        }
    }
}
