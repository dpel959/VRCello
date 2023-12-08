using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform hitFrame = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    public EffectManager effectManager;

    public DebugTextScript debugText;
    public bool longNoteFirst = false;
    public bool longNoteEntered = false;
    public int currentLongNoteTiming = 0;
    public Vector3 currentLongNoteHitPos;
    public int currentLongNoteDirection = 0;
    public bool vibratoNoteFirst = false;
    private float currentTime = 0f;

    public HandUI handUI = null;
    public bool noteNeedPress = false;
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
            if (boxNoteList[i] != null)
            {
                if (boxNoteList[i].GetComponent<Note>().NoteSpecies == p_noteSpecies)
                {
                    float t_notePosX = boxNoteList[i].transform.localPosition.x;
                    Note t_note = boxNoteList[i].GetComponent<Note>();
                    noteNeedPress = false;
                    for (int x = 0; x < timingBoxs.Length; x++)
                    {
                        if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                        {
                            for (int press = 0; press < 4; press++)
                            {
                                if (t_note.pressFinger[press]) // press
                                {
                                    noteNeedPress = true;
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

                            if (!handUI.isCelloHandAttached && noteNeedPress) // hand attached & what string
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
                                    Debug.Log("Damaged");
                                    PlayerController.Instance.PlayerDamage(5f);
                                    break;
                                default:
                                    break;
                            }

                            AudioManagerScript.Instance.PlaySFX("Clap");

                            if (p_noteSpecies == 2 || p_noteSpecies == 3)
                            {
                                longNoteEntered = true;
                                //currentLongNoteTiming = x;
                                //currentLongNoteHitPos = boxNoteList[i].transform.localPosition;
                            }
                            return true;
                        }
                    }
                }
            }
        }
        //Debug.Log("Damaged what?");
        //currentTime = 0f;
        //ComboManager.Instance.ResetCombo();
        //JudgeEffectManager.Instance.JudgementEffect(timingBoxs.Length); // Miss effect
        //ComboManager.Instance.judgeRecord[4]++;
        //PlayerController.Instance.PlayerDamage(10f);
        //currentTime = 0f;
        return false;
    }

    public void LongNoteCheck()
    {
        effectManager.NoteColorChange(0);
        effectManager.NoteHitEffect(boxNoteList[0].transform.position);
        JudgeEffectManager.Instance.JudgementEffect(0);

        float t_notePosX = boxNoteList[0].transform.localPosition.x;
        Note t_note = boxNoteList[0].GetComponent<Note>();
        for (int x = 0; x < timingBoxs.Length; x++)
        {
            if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
            {
                boxNoteList[0].GetComponent<Note>().HideNote();
                boxNoteList.RemoveAt(0);
            }
        }

        ScoreManager.Instance.IncreaseScore(0);

        ComboManager.Instance.judgeRecord[0]++;

        PlayerController.Instance.PlayerHeal(10f);

        AudioManagerScript.Instance.PlaySFX("Clap");
    }
    public void LongNoteCancel()
    {
        longNoteFirst = false;
        currentLongNoteTiming = 4;
        currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
        currentLongNoteDirection = int.MaxValue;
    }

    public void VibratoCancel()
    {
        vibratoNoteFirst = false;
        currentLongNoteTiming = 4;
        currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
        currentLongNoteDirection = int.MaxValue;
    }

    private void Update()
    {
        if (boxNoteList.Count == 0)
            handUI.GetComponent<Image>().enabled = false;
        else
            handUI.GetComponent<Image>().enabled = true;
        if (boxNoteList.Count != 0)
        {

            if (boxNoteList[0] != null)
            {
                if (boxNoteList[0].GetComponent<Note>().isLongNote)
                {
                    longNoteFirst = true;
                    vibratoNoteFirst = false;

                    if (timingBoxs[0].x <= boxNoteList[0].transform.localPosition.x && boxNoteList[0].transform.localPosition.x <= timingBoxs[0].y)
                        currentLongNoteDirection = boxNoteList[0].GetComponent<Note>().Direction;

                }
                else
                {
                    LongNoteCancel();
                }
                if (boxNoteList[0].GetComponent<Note>().isVibrato)
                {
                    longNoteFirst = false;

                    if (timingBoxs[0].x <= boxNoteList[0].transform.localPosition.x && boxNoteList[0].transform.localPosition.x <= timingBoxs[0].y)
                    {
                        currentLongNoteDirection = boxNoteList[0].GetComponent<Note>().Direction;
                        vibratoNoteFirst = true;
                    }
                }
                else
                {
                    VibratoCancel();
                }
            }
        }
    }
}
