using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    TimingManager theTimingManager;

    [SerializeField] Transform tfNoteAppear = null;
    void Start()
    {
        theTimingManager = GetComponent<TimingManager>();
    }

    // Update is called once per frame
    void Update()
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            if (other.GetComponent<Note>().isImageEnabled()) // 贸府 救等芭老锭父! Miss 犁积.
            {
                JudgeEffectManager.Instance.JudgementEffect(4);
                ComboManager.Instance.ResetCombo();
            }

            theTimingManager.boxNoteList.Remove(other.gameObject);

            ObjectPool.Instance.noteQueue.Enqueue(other.gameObject);
            other.gameObject.SetActive(false);

            //Destroy(other.gameObject);
        }
    }
}
