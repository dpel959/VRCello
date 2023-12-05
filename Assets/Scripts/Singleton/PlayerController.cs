using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public TimingManager timingManager;
    public HealthBar healthBar;

    private float currenthealth;
    public float CurrentHealth { 
        get
        {
            return currenthealth;
        }

        set
        {
            currenthealth = value;
            if (currenthealth > MaxHealth)
            {
                currenthealth = MaxHealth;
            }else if(currenthealth <= 0)
            {
                IsDead = true;
                //GameManager.Instance.PlayerDead();
            }
        }
    }
    [field: SerializeField]
    public float MaxHealth { get; set; }
    [field: SerializeField]
    public bool IsDead { get; set; }

    private void Start()
    {
        IsDead = false;
        CurrentHealth = MaxHealth;
        if (timingManager == null)
            Debug.LogError("PlayerManager isn't have TimingManager");
        if (healthBar == null)
            Debug.LogError("PlayerManager isn't have HelathBar");
    }

    public void PlayerDamage(float damage)
    {
        CurrentHealth -= damage;
        healthBar.HealthBarUpdate();
    }

    public void PlayerHeal(float heal)
    {
        CurrentHealth += heal;
        healthBar.HealthBarUpdate();
    }
}
