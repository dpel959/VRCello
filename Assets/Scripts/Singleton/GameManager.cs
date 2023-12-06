using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public NoteManager[] noteManagers;

    public int bpm = 0;
    double currentTime = 0d;

    bool noteActive = true;
    float longNoteHeight = float.MaxValue;
    int preNoteManager = int.MaxValue;

    private void Awake()
    {
        if (noteManagers.Length != 4)
        {
            Debug.LogError("There is not 4 note Managers!");
        }
    }
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick))
        {
            Debug.Log("result!");
            for (int i = 0; i < noteManagers.Length; i++)
            {
                noteManagers[i].RemoveAllNote();
                ResultManager.Instance.ShowResult();
            }
        }

        if (noteActive)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 60d / bpm)
            {

                GameObject t_note = ObjectPool.Instance.noteQueue.Dequeue();
                Note t_noteComponent = t_note.GetComponent<Note>();
                float heightRand; int noteManagerRand;
                if (t_noteComponent.NoteSpecies == 2 || t_noteComponent.NoteSpecies == 3)
                {
                    if (longNoteHeight == float.MaxValue)
                    {
                        heightRand = Random.Range(-40f, 41f);
                        longNoteHeight = heightRand;
                        noteManagerRand = Random.Range(0, noteManagers.Length);
                        preNoteManager = noteManagerRand;
                    }
                    else
                    {
                        heightRand = longNoteHeight;
                        noteManagerRand = preNoteManager;
                    }
                }
                else
                {
                    heightRand = Random.Range(-40f, 41f);
                    noteManagerRand = Random.Range(0, noteManagers.Length);
                }
                t_note.transform.SetParent(noteManagers[noteManagerRand].transform);
                t_note.transform.position = noteManagers[noteManagerRand].tfNoteAppear.position;
                t_note.transform.rotation = noteManagers[noteManagerRand].tfNoteAppear.rotation;
                t_note.SetActive(true);
                t_note.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                t_note.transform.localPosition = new Vector3(t_note.transform.localPosition.x, t_note.transform.localPosition.y + heightRand,
                    t_note.transform.localPosition.z);
                noteManagers[noteManagerRand].timingManager.boxNoteList.Add(t_note);
                currentTime -= 60d / bpm;

                if (noteManagers[noteManagerRand].isFirst)
                {
                    noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition = new Vector3(noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition.x
                    , -(noteManagers[noteManagerRand].timingManager.boxNoteList[0].GetComponent<RectTransform>().anchoredPosition.y + 23f) / 350f
                    , noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition.z);
                    noteManagers[noteManagerRand].isFirst = false;
                }

                if (t_noteComponent.NoteSpecies == 2 || t_noteComponent.NoteSpecies == 3)
                {
                    if (!t_noteComponent.EndFlag)
                    {
                        GameObject longNotePanel = Instantiate(ObjectPool.Instance.longNotePanel, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        RectTransform panel_rect = longNotePanel.GetComponent<RectTransform>();
                        longNotePanel.transform.SetParent(noteManagers[noteManagerRand].tfNoteAppear);
                        longNotePanel.transform.position = noteManagers[noteManagerRand].tfNoteAppear.position;
                        longNotePanel.transform.rotation = noteManagers[noteManagerRand].tfNoteAppear.rotation;
                        longNotePanel.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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
                        panel_rect.sizeDelta = new Vector2(36000f / bpm, 120f);
                        longNotePanel.GetComponent<BoxCollider>().size = new Vector2(36000f / bpm, 120f);
                        panel_rect.anchoredPosition = new Vector2(-24000f / (bpm * 2), heightRand);
                    }
                    else
                    {
                        longNoteHeight = float.MaxValue;
                    }
                }
            }
        }
    }



    public void PlayerDead()
    {
        Debug.Log("Player Dead!");
        AudioManagerScript.Instance.PlaySFX("Dead");
        ResultManager.Instance.ShowResult();
        for (int i = 0; i < noteManagers.Length; i++)
        {
            noteManagers[i].RemoveAllNote();
        }
    }
}
