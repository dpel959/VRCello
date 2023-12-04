using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public TMP_Text healthText;
    public Image[] healthImages;
    public float lerpSpeed;

    private void Start()
    {
        HealthBarUpdate();
    }
    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
    }
    public void HealthBarUpdate()
    {
        HealthTextUpdate();
        HealthBarFill();
        ColorChange();
    }

    private void HealthTextUpdate()
    {
        healthText.text = "Health : " + PlayerController.Instance.CurrentHealth + "%";
    }

    private void HealthBarFill()
    {
        for(int i =0; i < healthImages.Length; i++)
        {
            healthImages[i].fillAmount = Mathf.Lerp(healthImages[i].fillAmount,
                PlayerController.Instance.CurrentHealth / PlayerController.Instance.MaxHealth, lerpSpeed);
        }
    }

    private void ColorChange()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, 
            PlayerController.Instance.CurrentHealth / PlayerController.Instance.MaxHealth);

        for(int i =0; i < healthImages.Length; i++)
            healthImages[i].color = healthColor;
    }
}
