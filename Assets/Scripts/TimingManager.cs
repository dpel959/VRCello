using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
    public List<GameObject> boxNoteList = new List<GameObject>();

    [SerializeField] Transform center = null;
    [SerializeField] RectTransform[] timingRect = null;
    Vector2[] timingBoxs = null;

    public EffectManager effectManager;

    private void Start()
    {
        if (effectManager == null)
            Debug.LogError("TImingManager's effectManager is null");
        timingBoxs = new Vector2[timingRect.Length];

        for(int i = 0; i < timingRect.Length; i++)
        {
            timingBoxs[i].Set(center.localPosition.x - timingRect[i].rect.width / 2, 
                center.localPosition.x + timingRect[i].rect.width / 2);
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            for(int x = 0; x < timingBoxs.Length; x++)
            {
                if(timingBoxs[x].x <= t_notePosX && t_notePosX <= timingBoxs[x].y)
                {
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

                    return true;
                }
            }
        }

        ComboManager.Instance.ResetCombo();
        JudgeEffectManager.Instance.JudgementEffect(timingBoxs.Length); // Miss effect
        ComboManager.Instance.judgeRecord[4]++;
        PlayerController.Instance.PlayerDamage(10f);
        return false;
    }
}
