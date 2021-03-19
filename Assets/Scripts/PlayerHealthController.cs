using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth, maxHealth;

    public float invincibleLenght;
    private float _invincibleCounter;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
    }

    void Update()
    {
        if(_invincibleCounter > 0)
        {
            _invincibleCounter -= Time.deltaTime;
        }
    }

    public void DamageToPlayer(int damageAmount)
    {
        if(_invincibleCounter <= 0 && !GameManager.instance.ending)
        {
            AudioManager.instance.PlaySFX(7);
            currentHealth -= damageAmount;

            UIController.instance.ShowDamage();

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDied();

                AudioManager.instance.StopBGM();
                AudioManager.instance.PlaySFX(6);
                AudioManager.instance.StopSFX(7);
            }
            _invincibleCounter = invincibleLenght;

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
        }
    }

    public void PickupHealth(int healthAmount)
    {
        currentHealth += healthAmount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
    }
}
