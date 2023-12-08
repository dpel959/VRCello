using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Song : MonoBehaviour
{
    public string songName;
    public string composer;
    public int bpm;
    public int maxScore;
    [SerializeField]
    public GameManager.Stage stage;

    TMP_Text[] songTexts;
    // Start is called before the first frame update
    void Start()
    {
        maxScore = 0;
        songTexts = GetComponentsInChildren<TMP_Text>();
        for(int i =0; i < songTexts.Length; i++)
        {
            switch (i)
            {
                case 0:
                    songTexts[i].text = songName;
                    break;
                case 1:
                    songTexts[i].text = composer;
                    break;
                case 2:
                    songTexts[i].text = "BPM : " +bpm;
                    break;
                case 3:
                    songTexts[i].text = "Max Score\n" + maxScore; 
                    break;
                default:
                    Debug.LogError("SongArray Errored");
                    break;
            }

        }
    }

    public void SelectSong()
    {
        GameManager.Instance.bpm = bpm;
        GameManager.Instance.currentStage = stage;
    }

    public void GameStart() {
        GameManager.Instance.StageON();
    }
}
