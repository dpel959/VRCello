using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0d;

    TimingManager theTimingMaanger;

    [SerializeField] Transform tfNoteAppear = null;
    [SerializeField] GameObject goNote = null;
    // Start is called before the first frame update
    void Start()
    {
        theTimingMaanger = GetComponent<TimingManager>();
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
            theTimingMaanger.boxNoteList.Add(t_note);
            currentTime -= 60d / bpm;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            theTimingMaanger.boxNoteList.Remove(other.gameObject);
            Destroy(other.gameObject);
        }
    }
}
