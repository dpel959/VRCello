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
    [SerializeField] ObjectInfo[] objectInfos = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();
    public Queue<GameObject>[] allNoteQueue = new Queue<GameObject>[3];
    public GameObject longNotePanel;
    int poolCount = 15;
    private void Awake()
    {
        for(int i = 0; i < 2; i++)
        {
            allNoteQueue[i] = InsertQueue(objectInfos[i]); // only short note
        }

        allNoteQueue[2] = InsertQueueLongNote(); // long Note queue
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
            for(int j = 0; j < 4; j++)
                t_clone.GetComponent<Note>().pressFinger[j] = false;
            int rand = Random.Range(0, 4);
            t_clone.GetComponent<Note>().pressFinger[rand] = true;
            t_clone.GetComponent<Note>().pressImage[rand].SetActive(true);
            t_clone.SetActive(false);
            //if (objectInfo.tfPoolParent != null)
            //    t_clone.transform.SetParent(objectInfo.tfPoolParent);
            //else
                t_clone.transform.SetParent(this.transform);

            t_queue.Enqueue(t_clone);
        }

        return t_queue;
    }

    Queue<GameObject> InsertQueueLongNote()
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        int rand = Random.Range(1, 3);
        for (int i = 0; i < objectInfos[2].count; i++)
        {
            for (int j = 0; j <= rand; j++)
            {
                int longNoteRand = Random.Range(2, 4);
                GameObject t_clone = Instantiate(objectInfos[longNoteRand].goPrefab, transform.position, Quaternion.identity);
                for (int k = 0; k < 4; k++)
                    t_clone.GetComponent<Note>().pressFinger[k] = false;
                int pressRand = Random.Range(0, 4);
                t_clone.GetComponent<Note>().pressFinger[pressRand] = true;
                t_clone.GetComponent<Note>().pressImage[pressRand].SetActive(true);
                t_clone.SetActive(false);
                if (j == rand)
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
        int rand = Random.Range(0, 3); // 주의 3는 없는 것과 같음
        if (rand >= 0 && rand <= 1)
        {
            GameObject temp = allNoteQueue[rand].Dequeue();
            noteQueue.Enqueue(temp);
        }
        else if(rand == 2)
        {
            LongNoteEnqueue();
        }
    }

    public void LongNoteEnqueue()
    {
        for (int i = 0; i < allNoteQueue[2].Count; i++)
        {
            GameObject t_note = allNoteQueue[2].Dequeue();
            noteQueue.Enqueue(t_note);
            if (t_note.GetComponent<Note>().EndFlag)
                break;
        }
    }
}
