using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultManager : Singleton<ResultManager>
{
    [SerializeField] TMP_Text[] judgeText = null;
    [SerializeField] TMP_Text scoreText = null;
    [SerializeField] TMP_Text maxComboText = null;
    [SerializeField]
    RectTransform originPos;
    [SerializeField]
    GameObject[] allUIs;

    void Start()
    {
        //originPos = GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = new Vector3(10000, 10000);
    }

    public void ShowResult()
    {
        GetComponent<RectTransform>().position = originPos.position;
        TurnOffUIs();
        for (int i = 0; i < judgeText.Length; i++)
            judgeText[i].text = string.Format("{0:#,##0}", ComboManager.Instance.judgeRecord[i]);
        scoreText.text = string.Format("{0:#,##0}", ScoreManager.Instance.CurrentScore);
        maxComboText.text = string.Format("{0:#,##0}", ComboManager.Instance.MaxCombo);
    }
    public void HideResult()
    {
        GetComponent<RectTransform>().position = new Vector3(10000, 10000);
        TurnOnUIs();
        ComboManager.Instance.ResetCombo();
        ScoreManager.Instance.ResetScore();
    }
    public void TurnOnUIs()
    {
        for(int i =0; i < allUIs.Length; i++)
        {
            allUIs[i].SetActive(true);
        }
    }

    public void TurnOffUIs()
    {
        for (int i = 0; i < allUIs.Length; i++)
        {
            allUIs[i].SetActive(false);
        }
    }
}
