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

    public bool longNoteHitted = false;
    public int currentLongNoteTiming = 0;
    public int currentLongNoteDirection = 0;
    public bool longNoteWellPlayed = false;
    public bool longNoteFirst = false;
    public bool vibratoNoteFirst = false;

    [field:SerializeField]
    public GameObject panelNoteDetector;

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

        //for (int i = 0; i < boxNoteList.Count; i++)
        //{
        // 이거 일단 다 0으로 바꿈. boxNoteList[i]가 원본. 이상하면 for 하고 i로 바꾸기
        if (boxNoteList.Count > 0)
        {
            if (boxNoteList[0] != null)
            {
                if (boxNoteList[0].GetComponent<Note>().NoteSpecies == p_noteSpecies)
                {
                    float t_notePosX = boxNoteList[0].transform.localPosition.x;
                    Note t_note = boxNoteList[0].GetComponent<Note>();
                    noteNeedPress = false;
                    for (int x = 0; x < timingBoxs.Length; x++)
                    {
                        if (timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                        {
                            for (int press = 0; press < 4; press++)
                            {
                                if (t_note.pressFinger[press]) // need to press
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

                            if (p_noteSpecies >= 2 && p_noteSpecies <= 5)
                            {
                                if (longNoteWellPlayed)
                                    x = 0; //perfect
                                if (t_note.EndFlag)
                                {
                                    longNoteHitted = false;
                                    panelNoteDetector.transform.localPosition = new Vector3(1000f,
                                        panelNoteDetector.transform.localPosition.y, panelNoteDetector.transform.localPosition.z);
                                }
                                else
                                {
                                    longNoteHitted = true;
                                    currentLongNoteTiming = x;
                                    panelNoteDetector.transform.localPosition = new Vector3(boxNoteList[0].transform.localPosition.x,
                                        panelNoteDetector.transform.localPosition.y, panelNoteDetector.transform.localPosition.z);
                                }
                                longNoteWellPlayed = false;
                            }

                            effectManager.NoteColorChange(x);
                            effectManager.NoteHitEffect(boxNoteList[0].transform.position);
                            JudgeEffectManager.Instance.JudgementEffect(x);

                            boxNoteList[0].GetComponent<Note>().HideNote();
                            boxNoteList.RemoveAt(0);

                            ScoreManager.Instance.IncreaseScore(x);

                            ComboManager.Instance.judgeRecord[x]++;
                            Debug.Log(x + " : "+ComboManager.Instance.judgeRecord[x]);

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

                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void MissCheck()
    {
        ComboManager.Instance.MissCombo();
        JudgeEffectManager.Instance.JudgementEffect(timingBoxs.Length); // Miss effect
        ComboManager.Instance.judgeRecord[4]++;
        PlayerController.Instance.PlayerDamage(10f);
        longNoteWellPlayed = false;
    }

    public void LongNoteCheck()
    {
        effectManager.NoteColorChange(currentLongNoteTiming);
        effectManager.NoteHitEffect(panelNoteDetector.transform.position);
        JudgeEffectManager.Instance.JudgementEffect(currentLongNoteTiming);

        ScoreManager.Instance.IncreaseScore(currentLongNoteTiming);

        ComboManager.Instance.judgeRecord[currentLongNoteTiming]++;
        Debug.Log(currentLongNoteTiming + " : " + ComboManager.Instance.judgeRecord[currentLongNoteTiming]);

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
                Debug.Log("Damaged");
                PlayerController.Instance.PlayerDamage(5f);
                break;
            default:
                break;
        }

        longNoteWellPlayed = true;

        AudioManagerScript.Instance.PlaySFX("Clap");
    }

    public void LongNoteCancel()
    {
        longNoteFirst = false;
        currentLongNoteTiming = 4;
        //currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
        currentLongNoteDirection = int.MaxValue;
    }

    public void VibratoCancel()
    {
        vibratoNoteFirst = false;
        currentLongNoteTiming = 4;
        //currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
        currentLongNoteDirection = int.MaxValue;
    }

    public void AllLongNoteCancel()
    {
        longNoteFirst = false;
        vibratoNoteFirst = false;
        currentLongNoteTiming = 4;
        //currentLongNoteHitPos = new Vector3(0f, 0f, 0f);
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
                    vibratoNoteFirst = false;
                    longNoteFirst = true;
                    if (timingBoxs[3].x <= boxNoteList[0].transform.localPosition.x && boxNoteList[0].transform.localPosition.x <= timingBoxs[3].y)
                    {

                        if (!longNoteHitted)
                            currentLongNoteDirection = boxNoteList[0].GetComponent<Note>().Direction;
                    }
                }
                else if (boxNoteList[0].GetComponent<Note>().isVibrato)
                {
                    longNoteFirst = false;
                    vibratoNoteFirst = true;
                    if (timingBoxs[3].x <= boxNoteList[0].transform.localPosition.x && boxNoteList[0].transform.localPosition.x <= timingBoxs[3].y)
                    {
                        if (!longNoteHitted)
                            currentLongNoteDirection = boxNoteList[0].GetComponent<Note>().Direction;
                    }
                }
                else
                {
                    AllLongNoteCancel();
                }
            }
        }
    }
}
