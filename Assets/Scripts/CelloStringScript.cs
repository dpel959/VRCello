using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public DebugTextScript debugText;
    public TimingManager timingManager;
    private Renderer stringRenderer;
    public float threshold = 0.05f;
    private Vector3 bowPrePos = new Vector3(0, 0, 0);
    private Vector3 bowLocalPos = new Vector3(0, 0, 0);
    float currentTime = 0f;
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
        //if (currentTime <= 0.5f)
        //{
        //    currentTime += Time.deltaTime;
        //}
        //else
        //{
        //    bowPrePos = bowLocalPos;
        //    currentTime = 0f;
        //}
        if (collision.transform.CompareTag("Bow"))
        {

            if (timingManager.longNoteEntered)
            {
                if (timingManager.currentLongNoteDirection == 0)
                {
                    if(bowLocalPos.x < bowPrePos.x)
                    {
                        timingManager.CheckTiming(2);
                    }
                    else
                    {
                        timingManager.LongNoteCancel();
                        ComboManager.Instance.ResetCombo();
                        JudgeEffectManager.Instance.JudgementEffect(4); // Miss effect
                        ComboManager.Instance.judgeRecord[4]++;
                        PlayerController.Instance.PlayerDamage(10f);
                    }
                }
                else
                {
                    if (bowLocalPos.x > bowPrePos.x)
                    {
                        timingManager.CheckTiming(3);
                    }
                    else
                    {
                        timingManager.LongNoteCancel();
                        ComboManager.Instance.ResetCombo();
                        JudgeEffectManager.Instance.JudgementEffect(4); // Miss effect
                        ComboManager.Instance.judgeRecord[4]++;
                        PlayerController.Instance.PlayerDamage(10f);
                    }
                }
            }
            else
            {
                if (bowLocalPos.x < bowPrePos.x)
                {
                    timingManager.CheckTiming(2);
                }
                else if (bowLocalPos.x > bowPrePos.x)
                {
                    timingManager.CheckTiming(3);
                }
            }
            if (bowLocalPos.x < bowPrePos.x)
            {
                //timingManager.CheckTiming(0);
                stringRenderer.material.color = new Color(255, 0, 0);
            }
            else if (bowLocalPos.x > bowPrePos.x)
            {
                //timingManager.CheckTiming(1);
                stringRenderer.material.color = new Color(0, 0, 255);
            }
            HapticManager.Instance.PlayHapticClip2();
            bowLocalPos = collision.transform.localPosition;
            debugText.SetDebugText(bowLocalPos.x.ToString());
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            if (timingManager.longNoteEntered)
            {
                timingManager.LongNoteCancel();
                ComboManager.Instance.ResetCombo();
                JudgeEffectManager.Instance.JudgementEffect(4); // Miss effect
                ComboManager.Instance.judgeRecord[4]++;
                PlayerController.Instance.PlayerDamage(10f);
            }
            bowLocalPos = collision.transform.localPosition;
            if (bowLocalPos.x < bowPrePos.x)
            {
                timingManager.CheckTiming(0);
            }
            if (bowLocalPos.x > bowPrePos.x)
            {
                timingManager.CheckTiming(1);
            }
            bowPrePos = bowLocalPos;
            HapticManager.Instance.StopHaptics();
            currentTime = 0f;
            stringRenderer.material.color = new Color(0, 0, 0);
            //if (HapticManager.Instance.hapticCnt == 0)
            //{
            //    HapticManager.Instance.HapticLoopOff();
            //    HapticManager.Instance.StopHaptics();
            //}
        }
    }
}
