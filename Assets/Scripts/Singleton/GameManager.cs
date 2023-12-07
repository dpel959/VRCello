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
    int preSideNoteManger = int.MaxValue;
    public Stage currentStage = Stage.Song;
    public enum Stage
    {
        Short = 0,
        Long,
        ShortTwo,
        LongTwo,
        Song
    }

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
                // note from object pool
                GameObject t_note = ObjectPool.Instance.noteQueue.Dequeue();
                Note t_noteComponent = t_note.GetComponent<Note>();
                float heightRand; int noteManagerRand; int sideNoteManagerRand = int.MaxValue; 
                int sideRand = Random.Range(0, 4); // 3이면 노트 동시 출현
              
                if(GameManager.Instance.currentStage == GameManager.Stage.Short ||
                   GameManager.Instance.currentStage == GameManager.Stage.Long)
                {
                    sideRand = int.MaxValue;
                }else if (GameManager.Instance.currentStage == GameManager.Stage.ShortTwo ||
                   GameManager.Instance.currentStage == GameManager.Stage.LongTwo
                    )
                {
                    sideRand = 3;
                }

                // if long Note
                if (t_noteComponent.NoteSpecies == 2 || t_noteComponent.NoteSpecies == 3)
                {
                    if (longNoteHeight == float.MaxValue) // 전 롱노트 없음
                    {
                        heightRand = Random.Range(-40f, 41f);
                        longNoteHeight = heightRand;
                        noteManagerRand = Random.Range(0, noteManagers.Length);
                        preNoteManager = noteManagerRand;

                        if (sideRand == 3) // 사이드 있다고 판정되었을 때
                        {
                            if (GameManager.Instance.currentStage == GameManager.Stage.LongTwo ||
                                GameManager.Instance.currentStage == GameManager.Stage.Song)
                            {
                                if (noteManagerRand == noteManagers.Length - 1)
                                    sideNoteManagerRand = noteManagerRand - 1 ;
                                else
                                    sideNoteManagerRand = noteManagerRand + 1;
                                preSideNoteManger = sideNoteManagerRand;
                            }
                        }
                    }
                    else // 전 롱노트 존재
                    {
                        heightRand = longNoteHeight;
                        noteManagerRand = preNoteManager;
                        if(preSideNoteManger != int.MaxValue) // 전 사이드 존재
                        {
                            sideNoteManagerRand = preSideNoteManger;
                            sideRand = 3;
                        }
                    }
                }
                else // if short note
                {
                    heightRand = Random.Range(-40f, 41f);
                    noteManagerRand = Random.Range(0, noteManagers.Length);
                    if(sideRand == 3 &&
                        (GameManager.Instance.currentStage == GameManager.Stage.ShortTwo ||
                        GameManager.Instance.currentStage == GameManager.Stage.Song))
                    {
                        if (noteManagerRand == noteManagers.Length - 1)
                            sideNoteManagerRand = noteManagerRand - 1;
                        else
                            sideNoteManagerRand = noteManagerRand + 1;
                    }
                }

                // allocation to noteManager
                t_note.transform.SetParent(noteManagers[noteManagerRand].transform);
                t_note.transform.position = noteManagers[noteManagerRand].tfNoteAppear.position;
                t_note.transform.rotation = noteManagers[noteManagerRand].tfNoteAppear.rotation;
                t_note.SetActive(true);
                t_note.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                t_note.transform.localPosition = new Vector3(t_note.transform.localPosition.x, t_note.transform.localPosition.y + heightRand,
                    t_note.transform.localPosition.z);
                noteManagers[noteManagerRand].timingManager.boxNoteList.Add(t_note);

                if (noteManagers[noteManagerRand].isFirst)
                {
                    noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition = new Vector3(noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition.x
                        , -(noteManagers[noteManagerRand].timingManager.boxNoteList[0].GetComponent<RectTransform>().anchoredPosition.y + 23f) / 350f
                        , noteManagers[noteManagerRand].timingManager.handUI.transform.localPosition.z);
                    noteManagers[noteManagerRand].isFirst = false;
                }

                // 복제. press 전부 없게 하고 isTemporal true하기
                if(sideRand == 3 && sideNoteManagerRand != int.MaxValue)
                {
                    if (GameManager.Instance.currentStage == Stage.ShortTwo ||
                        GameManager.Instance.currentStage == Stage.LongTwo ||
                        GameManager.Instance.currentStage == Stage.Song)
                    {
                        GameObject temp_obj = Instantiate(t_note, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        Debug.Log("note : " + noteManagerRand);
                        Debug.Log("side : "+ sideNoteManagerRand);
                        temp_obj.transform.SetParent(noteManagers[sideNoteManagerRand].transform);
                        temp_obj.transform.position = noteManagers[sideNoteManagerRand].tfNoteAppear.position;
                        temp_obj.transform.rotation = noteManagers[sideNoteManagerRand].tfNoteAppear.rotation;
                        temp_obj.SetActive(true);
                        temp_obj.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

                        temp_obj.transform.localPosition = new Vector3(temp_obj.transform.localPosition.x, temp_obj.transform.localPosition.y + heightRand,
                            temp_obj.transform.localPosition.z);
                        noteManagers[sideNoteManagerRand].timingManager.boxNoteList.Add(t_note);

                        Note temp_note = temp_obj.GetComponent<Note>();
                        temp_note.isTemporal = true;
                        for(int i= 0; i < 4; i++)
                        {
                            temp_note.pressFinger[i] = false;
                            temp_note.pressImage[i].SetActive(false);
                        }

                        if (noteManagers[sideNoteManagerRand].isFirst)
                        {
                            noteManagers[sideNoteManagerRand].timingManager.handUI.transform.localPosition = new Vector3(noteManagers[sideNoteManagerRand].timingManager.handUI.transform.localPosition.x
                                , -(noteManagers[sideNoteManagerRand].timingManager.boxNoteList[0].GetComponent<RectTransform>().anchoredPosition.y + 23f) / 350f
                                , noteManagers[sideNoteManagerRand].timingManager.handUI.transform.localPosition.z);
                            noteManagers[sideNoteManagerRand].isFirst = false;
                        }
                    }
                }


                if (t_noteComponent.NoteSpecies == 2 || t_noteComponent.NoteSpecies == 3)
                {
                    if (!t_noteComponent.EndFlag)
                    {
                        GameObject longNotePanel = Instantiate(ObjectPool.Instance.longNotePanel, new Vector3(0f, 0f, 0f), Quaternion.identity);
                        RectTransform panelRect = longNotePanel.GetComponent<RectTransform>();
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
                        panelRect.sizeDelta = new Vector2(36000f / bpm, 120f);
                        longNotePanel.GetComponent<BoxCollider>().size = new Vector2(36000f / bpm, 120f);
                        panelRect.anchoredPosition = new Vector2(-24000f / (bpm * 2), heightRand);

                        if(sideRand == 3)
                        {
                            GameObject sideLongNotePanel = Instantiate(ObjectPool.Instance.longNotePanel, new Vector3(0f, 0f, 0f), Quaternion.identity);
                            RectTransform side_panelRect = sideLongNotePanel.GetComponent<RectTransform>();
                            Debug.Log(sideNoteManagerRand);
                            sideLongNotePanel.transform.SetParent(noteManagers[sideNoteManagerRand].tfNoteAppear);
                            sideLongNotePanel.transform.position = noteManagers[sideNoteManagerRand].tfNoteAppear.position;
                            sideLongNotePanel.transform.rotation = noteManagers[sideNoteManagerRand].tfNoteAppear.rotation;
                            sideLongNotePanel.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                            switch (t_noteComponent.NoteSpecies)
                            {
                                case 2:
                                    sideLongNotePanel.GetComponent<Note>().Direction = 0;
                                    break;
                                case 3:
                                    sideLongNotePanel.GetComponent<Note>().Direction = 1;
                                    break;
                                default:
                                    Debug.LogError("Panel Instantiation Error");
                                    break;
                            }
                            side_panelRect.sizeDelta = new Vector2(36000f / bpm, 120f);
                            sideLongNotePanel.GetComponent<BoxCollider>().size = new Vector2(36000f / bpm, 120f);
                            side_panelRect.anchoredPosition = new Vector2(-24000f / (bpm * 2), heightRand);

                        }
                    }
                    else
                    {
                        longNoteHeight = float.MaxValue;
                        preNoteManager = int.MaxValue;
                        preSideNoteManger = int.MaxValue;
                    }
                }
                currentTime -= 60d / bpm;
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
