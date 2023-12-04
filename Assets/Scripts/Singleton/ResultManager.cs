using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : Singleton<ResultManager>
{
    [SerializeField] TMP_Text[] judgeText = null;
    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text maxComboText = null;
    Vector3 originPos;

    void Start()
    {
        originPos = transform.position;
        transform.position = new Vector3(10000, 10000, 0);
    }

    public void ShowResult()
    {
        transform.position = originPos;
        for (int i = 0; i < judgeText.Length; i++)
            judgeText[i].text = string.Format("{0:#,##0}", ComboManager.Instance.judgeRecord[i]);
        scoreText.text = string.Format("{0:#,##0}", ScoreManager.Instance.CurrentScore);
        maxComboText.text = string.Format("{0:#,##0}", ComboManager.Instance.MaxCombo);
    }
}
