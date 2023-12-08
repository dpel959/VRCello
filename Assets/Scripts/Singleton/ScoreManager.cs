using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] int increaseScore = 10;
    public int CurrentScore { get; set; }

    Animator animator;
    string animScoreUp = "ScoreUp";

    [SerializeField] float[] weight = null;
    [SerializeField] int comboBonusScore = 10;
    void Awake()
    {
        animator = GetComponent<Animator>();
        CurrentScore = 0;
        scoreText = GetComponentInChildren<TMP_Text>();
        scoreText.text = "Score : 0";
    }

    public void IncreaseScore(int JudgementState)
    {
        //increase Combo
        ComboManager.Instance.IncreaseCombo();

        int t_bonusComboScore = (ComboManager.Instance.CurrentCombo / 10) * comboBonusScore;

        // increase Score
        int t_increaseScore = increaseScore + t_bonusComboScore;

        t_increaseScore = (int)(t_increaseScore * weight[JudgementState]);

        CurrentScore += t_increaseScore;

        scoreText.text = string.Format("Score : {0:#,##0}", CurrentScore);

        animator.SetTrigger(animScoreUp);


    }

    public void ResetScore()
    {
        CurrentScore = 0;
    }
}
