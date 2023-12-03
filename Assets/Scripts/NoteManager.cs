using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    TimingManager theTimingManager;

    [SerializeField] Transform tfNoteAppear = null;
    [SerializeField] GameObject goNote = null;
    // Start is called before the first frame update
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
            GameObject t_note = Instantiate(goNote, tfNoteAppear.position, tfNoteAppear.rotation);
            t_note.transform.SetParent(this.transform);
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
                JudgeEffectManager.Instance.JudgementEffect(4);

            theTimingManager.boxNoteList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
