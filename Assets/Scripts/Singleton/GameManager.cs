using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public NoteManager[] noteManagers;

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
