using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{
    [SerializeField]
    Song song;
    public void OnClickStartButton()
    {
        song.SelectSong();
        song.GameStart();
    }
}
