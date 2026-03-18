using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;  
    public int currentWeaponDamage = 25; 
    AudioSource hitNoise; 

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
        hitNoise = GetComponent<AudioSource>();
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    public void OnTriggerEnter(Collider collision)
    {
        //play the sound effect for hitting an enemy or player here
        hitNoise.Play();
        if (collision.tag == "player")
        {
             
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(currentWeaponDamage); // Example damage value
            }
        }

        if (collision.tag == "Enemy")
        {
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(currentWeaponDamage); // Example damage value
            }
        }
    }

}
