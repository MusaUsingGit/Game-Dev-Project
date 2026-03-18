using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : CharacterStats
{


    public HealthBar healthBar;

    PlayerAnimationHandler PlayerAnimationHandler;
    public int healthSoftCapLow = 40;
    public int healthSoftCapHigh = 60;
    public int healthHardCap = 100;

    UnityEngine.UI.Image DeathScreen;
    AudioSource[] audioSources;

    void Awake()
    {
        PlayerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
        audioSources = GetComponentsInChildren<AudioSource>();
    }
    void Start()
    {
        SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }
    private int SetMaxHealthFromHealthLevel()
    {
        if (healthLevel >= 10 && healthLevel <= healthSoftCapLow)
        {
            maxHealth = 400 + (healthLevel * 31);
        }
        else if (healthLevel > healthSoftCapLow && healthLevel <= healthSoftCapHigh)
        {
            maxHealth = 400 + (healthSoftCapLow * 31) + ((healthLevel - healthSoftCapLow) * 20);
        }
        else
        {
            maxHealth = 400 + (healthSoftCapLow * 31) + ((healthLevel - healthSoftCapLow) * 10) + ((healthLevel - healthSoftCapHigh) * 2); 
        }
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrenthealth(currentHealth);
        

        PlayerAnimationHandler.PlayTargetAnimation("GetHit", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerAnimationHandler.PlayTargetAnimation("Death", true);
            PlayerAnimationHandler.canRotate = false;
            StartCoroutine(PlayDeathSound());

        }
        else
        {
        audioSources[Random.Range(0, audioSources.Length-2)].Play();
            
        }
    }

    IEnumerator PlayDeathSound()
    {
        audioSources[3].Play();
        yield return new WaitForSeconds(audioSources[3].clip.length);
    }
}
