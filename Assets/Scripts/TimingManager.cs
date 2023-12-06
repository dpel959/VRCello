using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform hitFrame = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    public EffectManager effectManager;

    public DebugTextScript debugText;
    public bool longNoteEntered = false;
    public int currentLongNoteTiming = 0;
    public Vector3 currentLongNoteHitPos;
    public int currentLongNoteDirection = 0;
    private float currentTime = 0f;
    public HandUI handUI;
    private void Start()
    {
        if (effectManager == null)
            Debug.LogError("TImingManager's effectManager is null");
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(hitFrame.localPosition.x - timingRect[i].rect.width / 2, 
                hitFrame.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming(int p_noteSpecies)
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            if (boxNoteList[i].GetComponent<Note>().NoteSpecies == p_noteSpecies)
            {
                float t_notePosX = boxNoteList[i].transform.localPosition.x;
                Note t_note = boxNoteList[i].GetComponent<Note>();
                for (int x = 0; x < timingBoxs.Length; x++)
                {
                    if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                    {
                        for(int press = 0; press < 4; press++)
                        {
                            if (t_note.pressFinger[press])
                            {
                                switch (press)
                                {
                                    case 0:
                                        if (!OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch))
                                            return true;
                                        break;
                                    case 1:
                                        if (!OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
                                            return true;
                                        break;
                                    case 2:
                                        if (!OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
                                            return true;
                                        break;
                                    case 3:
                                        if (!OVRInput.Get(OVRInput.RawButton.LHandTrigger))
                                            return true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        if (!handUI.isCelloHandAttached)
                            return true;

                        effectManager.NoteColorChange(x);
                        effectManager.NoteHitEffect(boxNoteList[i].transform.position);
                        JudgeEffectManager.Instance.JudgementEffect(x);


                        boxNoteList[i].GetComponent<Note>().HideNote();
                        boxNoteList.RemoveAt(i);

                        ScoreManager.Instance.IncreaseScore(x);

                        ComboManager.Instance.judgeRecord[x]++;

                        switch (x)
                        {
                            case 0: //perfect
                                PlayerController.Instance.PlayerHeal(10f);
                                break;
                            case 1: //cool
                                PlayerController.Instance.PlayerHeal(5f);
                                break;
                            case 2: //good
                                break;
                            case 3: //bad
                                PlayerController.Instance.PlayerDamage(5f);
                                break;
                            default:
                                break;
                        }

                        AudioManagerScript.Instance.PlaySFX("Clap");

                        if (p_noteSpecies == 2 || p_noteSpecies == 3)
                        {
                            if (boxNoteList[i].GetComponent<Note>().EndFlag)
                            {
                                LongNoteCancel();
                            }
                            else
                            {
                                longNoteEntered = true;
                                currentLongNoteTiming = x;
                                currentLongNoteHitPos = boxNoteList[i].transform.position;
                                currentLongNoteDirection = boxNoteList[i].GetComponent<Note>().Direction;
                            }
                        }
                        debugText.SetDebugText("LongnoteEnter:" + longNoteEntered);
                        debugText.SetDebugText("LongnoteTiming:" + currentLongNoteTiming);
                        debugText.SetDebugText("Longnotedirection:" + currentLongNoteDirection);
                        return true;
                    }
                }
            }
        }
        if (longNoteEntered)
        {
            debugText.SetDebugText("!LongnoteEntered!");
            if (currentTime <= 0.5f)
            {
                currentTime += Time.deltaTime;
                return true;
            }
            else
            {
                effectManager.NoteColorChange(currentLongNoteTiming);
                effectManager.NoteHitEffect(currentLongNoteHitPos);
                JudgeEffectManager.Instance.JudgementEffect(currentLongNoteTiming);

                ScoreManager.Instance.IncreaseScore(currentLongNoteTiming);

                ComboManager.Instance.judgeRecord[currentLongNoteTiming]++;

                switch (currentLongNoteTiming)
                {
                    case 0: //perfect
                        PlayerController.Instance.PlayerHeal(10f);
                        break;
                    case 1: //cool
                        PlayerController.Instance.PlayerHeal(5f);
                        break;
                    case 2: //good
                        break;
                    case 3: //bad
                        break;
                    default:
                        break;
                }

                AudioManagerScript.Instance.PlaySFX("Clap");

                currentTime = 0f;

                return true;
            }
        }

        ComboManager.Instance.ResetCombo();
        JudgeEffectManager.Instance.JudgementEffect(timingBoxs.Length); // Miss effect
        ComboManager.Instance.judgeRecord[4]++;
        PlayerController.Instance.PlayerDamage(10f);
        return false;
    }

    public void LongNoteCancel()
    {
        longNoteEntered = false;
        currentLongNoteTiming = 0;
        currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
        currentLongNoteDirection = 0;
    }
}
