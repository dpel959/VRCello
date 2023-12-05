using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugTextScript : MonoBehaviour
{
    TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<TMP_Text>();
    }

    public void SetDebugText(string str)
    {
        debugText.text += str + "\n";
        if (debugText.text.Length > 1000) debugText.text = string.Empty;
    }
}
