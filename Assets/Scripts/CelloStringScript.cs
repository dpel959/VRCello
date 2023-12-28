using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public DebugTextScript debugText;
    public TimingManager timingManager;
    private Renderer stringRenderer;
    float currentTime = 0f;
    float vibratoTime = 0f;
    public float longThreshold = 0.5f;
    public float vibratoThreshold = 50f;
    public Vector3 bowPrePos = new Vector3(0, 0, 0);
    public Vector3 bowCurPos = new Vector3(0, 0, 0);
    Quaternion handPreRot = Quaternion.identity;
    public bool bowAttached = false;

    private bool isPlayerVibrato = false;
    private void Start()
    {
        stringRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowAttached = true;
            bowPrePos = collision.transform.localPosition;
            handPreRot = CelloHand.Instance.transform.localRotation;
            if (HapticManager.Instance.hapticCnt == 0)
            {
                HapticManager.Instance.HapticLoopOn();
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowCurPos = collision.transform.localPosition;
            if (timingManager.longNoteFirst || timingManager.vibratoNoteFirst)
            {
                if (currentTime <= 1.0f)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    bowPrePos = bowCurPos;
                    currentTime = 0f;
                }
                if (!timingManager.longNoteHitted)
                {
                    if (timingManager.currentLongNoteDirection == 0)
                    {
                        if (bowPrePos.x - bowCurPos.x >= longThreshold)
                        {
                            //vibratoTime = 0f;

                            if (timingManager.vibratoNoteFirst)
                                timingManager.CheckTiming(4);
                            else
                                timingManager.CheckTiming(2);
                        }
                    }
                    else if (timingManager.currentLongNoteDirection == 1)
                    {
                        if (bowCurPos.x - bowPrePos.x >= longThreshold)
                        {
                            //vibratoTime = 0f;

                            if (timingManager.vibratoNoteFirst)
                                timingManager.CheckTiming(5);
                            else
                                timingManager.CheckTiming(3);
                        }
                    }
                }
            }
            if (bowCurPos.x < bowPrePos.x)
            {
                stringRenderer.material.color = new Color(255, 0, 0);
            }
            else if (bowCurPos.x > bowPrePos.x)
            {
                stringRenderer.material.color = new Color(0, 0, 255);
            }
            if (CelloHand.Instance.isStringed)
            {
                if(OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch) ||
                    OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch) ||
                    OVRInput.Get(OVRInput.RawButton.LIndexTrigger) ||
                    OVRInput.Get(OVRInput.RawButton.LHandTrigger))
                {
                    HapticManager.Instance.PlayHapticBoth();
                }
                else
                {
                    HapticManager.Instance.PlayHapticRight();
                }
            }
            else
            {
                HapticManager.Instance.PlayHapticRight();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowCurPos = collision.transform.localPosition;
            bowAttached = false;
            if (bowCurPos.x < bowPrePos.x) // left
            {
                timingManager.CheckTiming(0);
            }
            if (bowCurPos.x > bowPrePos.x) // right
            {
                timingManager.CheckTiming(1);
            }
            bowPrePos = bowCurPos;
            stringRenderer.material.color = new Color(0, 0, 0);
            if (HapticManager.Instance.hapticCnt == 0)
            {
                HapticManager.Instance.HapticLoopOff();
                HapticManager.Instance.StopHaptics();
            }
        }
    }

    private void Update()
    {
            if (vibratoTime <= 1.0f)
            {
                vibratoTime += Time.deltaTime;
            }
            if (vibratoTime > 1.0f)
            {
                if (Mathf.Abs(CelloHand.Instance.transform.localRotation.eulerAngles.x - handPreRot.eulerAngles.x) >= vibratoThreshold)
                    isPlayerVibrato = true;
                else
                    isPlayerVibrato = false;
                handPreRot = CelloHand.Instance.transform.localRotation;
                vibratoTime = 0f;
            }
    }

    public void LongNoteCollide()
    {
        if (timingManager.longNoteHitted)
        {
            if (timingManager.currentLongNoteDirection == 0 && bowAttached)
            {
                if (bowPrePos.x - bowCurPos.x >= longThreshold)
                {
                    if (timingManager.vibratoNoteFirst)
                    {
                        if (isPlayerVibrato)
                            timingManager.LongNoteCheck();
                        else
                            timingManager.MissCheck();
                    }
                    else
                        timingManager.LongNoteCheck();

                    bowPrePos = bowCurPos;
                }
                else
                {
                    timingManager.MissCheck();
                }
            }
            else if (timingManager.currentLongNoteDirection == 1 && bowAttached)
            {
                if (bowCurPos.x - bowPrePos.x >= longThreshold)
                {
                    if (timingManager.vibratoNoteFirst)
                    {
                        if (isPlayerVibrato)
                            timingManager.LongNoteCheck();
                        else
                            timingManager.MissCheck();
                    }
                    else
                        timingManager.LongNoteCheck();

                    bowPrePos = bowCurPos;
                }
                else
                    timingManager.MissCheck();
            }
        }
    }

    public void LongNoteCollideEnd()
    {
        LongNoteCollide();
        timingManager.longNoteHitted = false;
        timingManager.panelNoteDetector.transform.localPosition = new Vector3(1000f,
           timingManager.panelNoteDetector.transform.localPosition.y, timingManager.panelNoteDetector.transform.localPosition.z);
        //longnote end «ÿ¡÷±‚
    }
}
