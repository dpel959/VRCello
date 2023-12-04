using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    bool noteActive = true;
    TimingManager theTimingManager;

    [SerializeField] Transform tfNoteAppear = null;
    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
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
                theTimingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm;
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

            theTimingManager.boxNoteList.Remove(other.gameObject);

            ObjectPool.Instance.allNoteQueue[t_note.noteSpecies].Enqueue(other.gameObject);
            ObjectPool.Instance.RandomEnqueue();
            other.gameObject.SetActive(false);
        }
    }

    // 게임 끝낼때
    public void RemoveAllNote()
    {
        noteActive = false;
        for(int i = 0; i < theTimingManager.boxNoteList.Count; i++)
        {
            theTimingManager.boxNoteList[i].SetActive(false);
            ObjectPool.Instance.noteQueue.Enqueue(theTimingManager.boxNoteList[i]);
        }
    }
}
