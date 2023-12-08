using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform tfPoolParent;
}

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] public ObjectInfo[] objectInfos = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();
    public Queue<GameObject>[] allNoteQueue = new Queue<GameObject>[4];
    public GameObject longNotePanel;
    int poolCount = 15;
    private void Awake()
    {
        for(int i = 0; i < 2; i++)
        {
            allNoteQueue[i] = InsertQueue(objectInfos[i]); // only short note
        }

        allNoteQueue[2] = InsertQueueLongNote(); // long Note queue
        allNoteQueue[3] = InsertQueueVibrato();
        for (int i = 0; i < poolCount; i++)
        {
            RandomEnqueue();
        }
        // 생성,파괴 많이 될 객체 있으면 [1], [2]해서 넣어주면 됨
    }

    Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();

        for (int i = 0; i < objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
            Note t_note = t_clone.GetComponent<Note>();
            for (int j = 0; j < 4; j++)
            {
                int rand = Random.Range(0, 2);
                if(rand == 1)
                {
                    t_note.pressFinger[j] = true;
                    t_note.pressImage[j].SetActive(true);
                }
                else
                {
                    t_note.pressFinger[j] = false;
                    t_note.pressImage[j].SetActive(false);
                }
            }
            t_note.isTemporal = false;
            t_clone.SetActive(false);
            //if (objectInfo.tfPoolParent != null)
            //    t_clone.transform.SetParent(objectInfo.tfPoolParent);
            //else
            t_clone.transform.SetParent(transform);

            t_queue.Enqueue(t_clone);
        }

        return t_queue;
    }

    Queue<GameObject> InsertQueueLongNote()
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        int longRand = Random.Range(1, 3);
        for (int i = 0; i < objectInfos[2].count; i++)
        {
            for (int j = 0; j <= longRand; j++)
            {
                int longNoteRand = Random.Range(2, 4);
                GameObject t_clone = Instantiate(objectInfos[longNoteRand].goPrefab, transform.position, Quaternion.identity);
                Note t_note = t_clone.GetComponent<Note>();
                for (int k = 0; k < 4; k++)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 1)
                    {
                        t_note.pressFinger[k] = true;
                        t_note.pressImage[k].SetActive(true);
                    }
                    else
                    {
                        t_note.pressFinger[k] = false;
                        t_note.pressImage[k].SetActive(false);
                    }
                }
                t_note.isTemporal = false;
                t_clone.SetActive(false);
                if (j == longRand)
                {
                    t_clone.GetComponent<Note>().EndFlag = true;
                }
                t_clone.SetActive(false);
                if (objectInfos[longNoteRand].tfPoolParent != null)
                    t_clone.transform.SetParent(objectInfos[longNoteRand].tfPoolParent);
                else
                    t_clone.transform.SetParent(this.transform);

                t_queue.Enqueue(t_clone);
            }
        }
        return t_queue;
    }

    Queue<GameObject> InsertQueueVibrato()
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        int longRand = Random.Range(1, 3);
        for (int i = 0; i < objectInfos[4].count; i++)
        {
            for (int j = 0; j <= longRand; j++)
            {
                int longNoteRand = Random.Range(4, 6);
                GameObject t_clone = Instantiate(objectInfos[longNoteRand].goPrefab, transform.position, Quaternion.identity);
                Note t_note = t_clone.GetComponent<Note>();
                for (int k = 0; k < 4; k++)
                {
                    int rand = Random.Range(0, 2);
                    if (rand == 1)
                    {
                        t_note.pressFinger[k] = true;
                        t_note.pressImage[k].SetActive(true);
                    }
                    else
                    {
                        t_note.pressFinger[k] = false;
                        t_note.pressImage[k].SetActive(false);
                    }
                }
                t_note.isTemporal = false;
                t_clone.SetActive(false);
                if (j == longRand)
                {
                    t_clone.GetComponent<Note>().EndFlag = true;
                }
                t_clone.SetActive(false);
                if (objectInfos[longNoteRand].tfPoolParent != null)
                    t_clone.transform.SetParent(objectInfos[longNoteRand].tfPoolParent);
                else
                    t_clone.transform.SetParent(this.transform);

                t_queue.Enqueue(t_clone);
            }
        }
        return t_queue;
    }

    public void RandomEnqueue()
    {
        int rand;
        switch (GameManager.Instance.currentStage)
        {
            case GameManager.Stage.Short:
                rand = Random.Range(0, 2);
                break;
            case GameManager.Stage.Long:
                rand = 2;
                break;
            case GameManager.Stage.ShortTwo:
                rand = Random.Range(0, 2);
                break;
            case GameManager.Stage.LongTwo:
                rand = 2;
                break;
            case GameManager.Stage.Vibrato:
                rand = 3;
                break;
            case GameManager.Stage.Song:
                rand = Random.Range(0, 4);
                break;
            default:
                Debug.LogError("Error in RandomEnqueue");
                rand = int.MaxValue;
                break;
        }
        if (rand >= 0 && rand <= 1)
        {
            GameObject temp = allNoteQueue[rand].Dequeue();
            noteQueue.Enqueue(temp);
        }
        else if(rand == 2)
        {
            LongNoteEnqueue(2);
        }
        else if(rand == 3)
        {
            LongNoteEnqueue(3);
        }
    }

    public void LongNoteEnqueue(int num)
    {
        for (int i = 0; i < allNoteQueue[num].Count; i++)
        {
            GameObject t_note = allNoteQueue[num].Dequeue();
            noteQueue.Enqueue(t_note);
            if (t_note.GetComponent<Note>().EndFlag)
                break;
        }
    }
}
