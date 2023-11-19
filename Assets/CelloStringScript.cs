using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public CelloBowScript celloBow;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            celloBow.SetPrePosition();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bow")){
            if (celloBow.transform.localPosition.y > celloBow.GetPrePosition().y)
            {
                renderer.material.color = new Color(255, 0, 0);
                celloBow.SetPrePosition();
            }
            if (celloBow.transform.localPosition.y < celloBow.GetPrePosition().y)
            {
                renderer.material.color = new Color(0, 0, 255);
                celloBow.SetPrePosition();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            renderer.material.color = new Color(0, 0, 0);
        }
    }
}
