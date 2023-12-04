using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public DebugTextScript debugText;
    public TimingManager timingManager;
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
            //HapticManager.Instance.PlayHapticClip2();
            //if (HapticManager.Instance.hapticCnt == 0)
            //{
            //    HapticManager.Instance.HapticLoopOn();
            //}
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            //bowLocalPos = collision.transform.localPosition;
            if (bowLocalPos.x > bowPrePos.x)
            {
                //timingManager.CheckTiming(0);
                stringRenderer.material.color = new Color(255, 0, 0);
            }
            if (bowLocalPos.x < bowPrePos.x)
            {
                //timingManager.CheckTiming(1);
                stringRenderer.material.color = new Color(0, 0, 255);
            }
            HapticManager.Instance.PlayHapticClip2();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowLocalPos = collision.transform.localPosition;
            if (bowLocalPos.x > bowPrePos.x)
            {
                timingManager.CheckTiming(1);
                stringRenderer.material.color = new Color(255, 0, 0);
            }
            if (bowLocalPos.x < bowPrePos.x)
            {
                timingManager.CheckTiming(0);
                stringRenderer.material.color = new Color(0, 0, 255);
            }
            bowPrePos = bowLocalPos;
            HapticManager.Instance.StopHaptics();
            stringRenderer.material.color = new Color(0, 0, 0);
            //if (HapticManager.Instance.hapticCnt == 0)
            //{
            //    HapticManager.Instance.HapticLoopOff();
            //    HapticManager.Instance.StopHaptics();
            //}
        }
    }
}
