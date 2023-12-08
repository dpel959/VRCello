using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboManager : Singleton<ComboManager>
{
    [SerializeField] Image goComboImage = null;
    [SerializeField] TMP_Text comboText = null;

    Animator animator;
    string animComboUp = "ComboUp";
    public int CurrentCombo { get; set; }
    public int MaxCombo { get; set; }

    public int[] judgeRecord = new int[5];

    private void Start()
    {
        for(int i= 0; i < 5; i++)
        {
            judgeRecord[i] = 0;
        }
        CurrentCombo = 0;
        MaxCombo = 0;
        animator = GetComponent<Animator>();   
        goComboImage = GetComponentInChildren<Image>();
        comboText = GetComponentInChildren<TMP_Text>();
        ResetCombo();
    }

    public void IncreaseCombo(int p_num = 1)
    {
        CurrentCombo += 1;
        comboText.text = string.Format("{0:#,##0}", CurrentCombo);

        if (MaxCombo < CurrentCombo)
            MaxCombo = CurrentCombo;

        if(CurrentCombo > 1)
        {
            goComboImage.gameObject.SetActive(true);
            comboText.gameObject.SetActive(true);

            animator.SetTrigger(animComboUp);
        }
    }

    public void ResetCombo()
    {
        CurrentCombo = 0;
        for (int i = 0; i < 5; i++)
        {
            judgeRecord[i] = 0;
        }
        //goComboImage.gameObject.SetActive(false);
        //comboText.gameObject.SetActive(false);
    }
}
