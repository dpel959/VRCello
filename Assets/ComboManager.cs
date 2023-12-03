using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboManager : Singleton<ComboManager>
{
    [SerializeField] Image goComboImage = null;
    [SerializeField] TMP_Text comboText = null;

    int currentCombo = 0;

    Animator animator;
    string animComboUp = "ComboUp";
    public int CurrentCombo { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();   
        goComboImage = GetComponentInChildren<Image>();
        comboText = GetComponentInChildren<TMP_Text>();
        ResetCombo();
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += 1;
        comboText.text = string.Format("{0:#,##0}", currentCombo);

        if(currentCombo > 1)
        {
            goComboImage.gameObject.SetActive(true);
            comboText.gameObject.SetActive(true);

            animator.SetTrigger(animComboUp);
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        goComboImage.gameObject.SetActive(false);
        comboText.gameObject.SetActive(false);
    }
}
