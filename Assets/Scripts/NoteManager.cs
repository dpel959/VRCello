using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    bool noteActive = true;
    TimingManager timingManager;
    [SerializeField] Transform tfNoteAppear = null;
    void Start()
    {
        timingManager = GetComponent<TimingManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if(noteActive)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 60d / bpm)
            {
                GameObject t_note = ObjectPool.Instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.transform.rotation = tfNoteAppear.rotation;
                t_note.SetActive(true);
                t_note.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                timingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm;

                Note t_noteComponent = t_note.GetComponent<Note>();
                if (t_noteComponent.NoteSpecies == 2 || t_noteComponent.NoteSpecies == 3)
                {
                    if (!t_noteComponent.EndFlag)
                    {
                        GameObject longNotePanel = Instantiate(ObjectPool.Instance.longNotePanel, new Vector3(0f,0f,0f), Quaternion.identity);
                        RectTransform panel_rect = longNotePanel.GetComponent<RectTransform>();
                        longNotePanel.transform.SetParent(tfNoteAppear);
                        longNotePanel.transform.position = tfNoteAppear.position;
                        longNotePanel.transform.rotation = tfNoteAppear.rotation;
                        longNotePanel.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        switch (t_noteComponent.NoteSpecies)
                        {
                            case 2:
                                longNotePanel.GetComponent<Note>().Direction = 0;
                                break;
                            case 3:
                                longNotePanel.GetComponent<Note>().Direction = 1;
                                break;
                            default:
                                Debug.LogError("Panel Instantiation Error");
                                break;
                        }
                        panel_rect.sizeDelta = new Vector2(24000f/bpm, 120f);
                        longNotePanel.GetComponent<BoxCollider>().size = new Vector2(24000f / bpm, 120f);
                        panel_rect.anchoredPosition = new Vector2(-24000f/(bpm*2),0f);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            Note t_note = other.GetComponent<Note>();
            if (t_note.isImageEnabled()) // 처리 안된거일때만! Miss 재생.
            {
                JudgeEffectManager.Instance.JudgementEffect(4);
                ComboManager.Instance.ResetCombo();
                ComboManager.Instance.judgeRecord[4]++;
                PlayerController.Instance.PlayerDamage(10f);
            }

            timingManager.boxNoteList.Remove(other.gameObject);

            if (t_note.NoteSpecies == 3) // long_note_right
                ObjectPool.Instance.allNoteQueue[2].Enqueue(other.gameObject);
            else
                ObjectPool.Instance.allNoteQueue[t_note.NoteSpecies].Enqueue(other.gameObject);
            if(t_note.EndFlag)
                ObjectPool.Instance.RandomEnqueue();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Panel"))
        {
            Destroy(other.gameObject);
        }
    }

    // 게임 끝낼때
    public void RemoveAllNote()
    {
        noteActive = false;
        for(int i = 0; i < timingManager.boxNoteList.Count; i++)
        {
            timingManager.boxNoteList[i].SetActive(false);
            ObjectPool.Instance.noteQueue.Enqueue(timingManager.boxNoteList[i]);
        }
    }
}
