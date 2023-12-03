using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";
    Image effectImage;

    private void Start()
    {
        effectImage = GetComponentInChildren<Image>();
    }
    public void NoteHitEffect(Vector3 notePos)
    {
        transform.position = notePos;
        noteHitAnimator.SetTrigger(hit);
    }

    public void NoteColorChange(int timingBoxNum)
    {
        switch (timingBoxNum)
        {
            case 0:
                effectImage.color = Color.yellow;
                break;
            case 1:
                effectImage.color = Color.blue;
                break;
            case 2:
                effectImage.color = Color.green;
                break;
            case 3:
                effectImage.color = Color.magenta;
                break;
            default:
                Debug.LogError("other Timing?");
                break;
        }

    }
}
