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
    public Queue<GameObject>[] allNoteQueue = new Queue<GameObject>[2];
    int count = 15;
    private void Awake()
    {
        allNoteQueue[0] = InsertQueue(objectInfos[0]); // short_left
        allNoteQueue[1] = InsertQueue(objectInfos[1]); // short_right
        for (int i = 0; i < count; i++)
            RandomEnqueue();
        // 생성,파괴 많이 될 객체 있으면 [1], [2]해서 넣어주면 됨
    }

    Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for(int i =0; i < objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            if (objectInfo.tfPoolParent != null)
                t_clone.transform.SetParent(objectInfo.tfPoolParent);
            else
                t_clone.transform.SetParent(this.transform);

            t_queue.Enqueue(t_clone);
        }
        return t_queue;
    }

    public void RandomEnqueue()
    {
        int rand = Random.Range(0, allNoteQueue.Length); 
        noteQueue.Enqueue(allNoteQueue[rand].Dequeue());
    }
}
