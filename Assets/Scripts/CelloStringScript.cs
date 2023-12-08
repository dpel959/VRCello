using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelloStringScript : MonoBehaviour
{
    public DebugTextScript debugText;
    public TimingManager timingManager;
    private Renderer stringRenderer;
    float currentTime = 0f;
    float longNoteCurTime = 0f;
    public float threshold = 0.5f;
    public float vibratoThreshold = 50f;
    private Vector3 bowPrePos = new Vector3(0, 0, 0);
    private Vector3 bowLocalPos = new Vector3(0, 0, 0);
    Quaternion handPreRot = Quaternion.identity;
    private void Start()
    {
        stringRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bow"))
        {
            bowPrePos = collision.transform.localPosition;
            handPreRot = CelloHand.Instance.transform.rotation;
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

            bowLocalPos = collision.transform.localPosition;
            //timingManager.longNoteEntered
            if (timingManager.longNoteFirst || timingManager.vibratoNoteFirst)
            {
                if (timingManager.longNoteFirst)
                {
                    timingManager.CheckTiming(2);
                    timingManager.CheckTiming(3);
                }
                else
                {
                    timingManager.CheckTiming(4);
                    timingManager.CheckTiming(5);
                }

                if (currentTime <= 5.0f)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    handPreRot = CelloHand.Instance.transform.rotation;
                    bowPrePos = bowLocalPos;
                    currentTime = 0;
                }
                if (longNoteCurTime >= 1.0f)
                {
                    if (timingManager.currentLongNoteDirection == 0)
                    {
                        if (bowLocalPos.x - bowPrePos.x < -threshold)
                        {
                                Debug.Log("correct");
                                JudgeEffectManager.Instance.JudgementEffect(0);

                                ScoreManager.Instance.IncreaseScore(0);

                                ComboManager.Instance.judgeRecord[0]++;

                                PlayerController.Instance.PlayerHeal(10f);

                                AudioManagerScript.Instance.PlaySFX("Clap");
                                bowPrePos = bowLocalPos;
                                longNoteCurTime = 0f;
                        }
                    }
                    else if (timingManager.currentLongNoteDirection == 1)
                    {
                        if (bowLocalPos.x - bowPrePos.x > threshold)
                        {
                                Debug.Log("correct");
                                JudgeEffectManager.Instance.JudgementEffect(0);

                                ScoreManager.Instance.IncreaseScore(0);

                                ComboManager.Instance.judgeRecord[0]++;

                                PlayerController.Instance.PlayerHeal(10f);

                                AudioManagerScript.Instance.PlaySFX("Clap");
                                bowPrePos = bowLocalPos;
                                longNoteCurTime = 0f;
                        }
                    }
                }
            }
            if (bowLocalPos.x < bowPrePos.x)
            {
                stringRenderer.material.color = new Color(255, 0, 0);
            }
            else if (bowLocalPos.x > bowPrePos.x)
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
            //if (timingManager.longNoteEntered)
            //{
            //    timingManager.LongNoteCancel();
            //    ComboManager.Instance.ResetCombo();
            //    JudgeEffectManager.Instance.JudgementEffect(4); // Miss effect
            //    ComboManager.Instance.judgeRecord[4]++;
            //    PlayerController.Instance.PlayerDamage(10f);
            //}
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
        if (longNoteCurTime <= 1.0f)
        {
            longNoteCurTime += Time.deltaTime;
        }
        if (longNoteCurTime > 1.0f && timingManager.vibratoNoteFirst)
        {
            if (Mathf.Abs(CelloHand.Instance.transform.rotation.y - handPreRot.y) >= vibratoThreshold)
            {
                HapticManager.Instance.SetHapticClip1();
                HapticManager.Instance.PlayHapticBoth();
                JudgeEffectManager.Instance.JudgementEffect(0);

                ScoreManager.Instance.IncreaseScore(0);

                ComboManager.Instance.judgeRecord[0]++;

                PlayerController.Instance.PlayerHeal(10f);

                AudioManagerScript.Instance.PlaySFX("Clap");
                handPreRot = CelloHand.Instance.transform.rotation;
                longNoteCurTime = 0f;
            }
        }
    }
}
