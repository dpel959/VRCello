using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
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
