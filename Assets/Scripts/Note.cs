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
    Image noteImage;

    private void OnEnable()
    {
        if (noteImage == null)
            noteImage = GetComponent<Image>();

        noteImage.enabled = true;
    }

    public void HideNote()
    {
        noteImage.enabled = false;
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
