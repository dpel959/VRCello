using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JudgeEffectManager : Singleton<JudgeEffectManager>
{
    [SerializeField] Animator judgeAnimator = null;
    string hit = "Hit";

    [SerializeField] Image judgeImage = null;
    [SerializeField] Sprite[] judgeSprite = null;

    private void Start()
    {
        judgeImage = GetComponent<Image>();
        judgeAnimator = GetComponent<Animator>();
    }

    public void JudgementEffect(int p_num)
    {
        judgeImage.sprite = judgeSprite[p_num];
        judgeAnimator.SetTrigger(hit);
    }
}
