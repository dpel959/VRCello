using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [field:SerializeField]
    public int NoteSpecies { get; set; }
    [field: SerializeField]
    public bool EndFlag { get; set; }
    [field: SerializeField]
    public int Direction { get; set; } //0 = left, 1 = right
    public float noteSpeed = 400f;
    public bool[] pressFinger = new bool[4];
    public GameObject[] pressImage;

    public bool isTemporal = false;
    public bool isLongNote = false;
    public bool isVibrato = false;
    Image noteImage;
    [SerializeField]
    Image arrowImage;
    [SerializeField]
    Image vibrateImage;
    private void OnEnable()
    {
        if (noteImage == null) 
            noteImage = GetComponent<Image>();

        noteImage.enabled = true;
        if (gameObject.CompareTag("Note"))
        {
            arrowImage.enabled = true;
            if (vibrateImage != null)
                vibrateImage.enabled = true;
            for (int i = 0; i < 4; i++)
            {
                if (pressFinger[i])
                    pressImage[i].SetActive(true);
            }
        }
    }

    public void HideNote()
    {
        noteImage.enabled = false;
        if (vibrateImage != null)
            vibrateImage.enabled = false;
        if (gameObject.CompareTag("Note"))
        {
            arrowImage.enabled = false;
            for (int i = 0; i < 4; i++)
            {
                pressImage[i].SetActive(false);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public bool isImageEnabled()
    {
        return noteImage.enabled;
    }
}
