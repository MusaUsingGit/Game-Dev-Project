using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Animator animator;
    public int healthSoftCapLow = 50;
    public int healthSoftCapHigh = 60;
    public int healthHardCap = 100;

    public UnityEngine.AI.NavMeshAgent navMeshAgent;

      AudioSource[] audioSources;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSources = GetComponentsInChildren<AudioSource>();
        SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        navMeshAgent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
    }
    void Start()
    {
        


    }
    private int SetMaxHealthFromHealthLevel()
    {
        if (healthLevel >= 1 && healthLevel <= 40)
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
        
        audioSources[Random.Range(0, audioSources.Length)].Play();

        animator.Play("GetHit");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Death");
            navMeshAgent.enabled = false;
            
            // Handle enemy death here (e.g., disable movement, remove enemy from scene, etc.)
            //disable game object after death animation finishes
            gameObject.GetComponent<EnemyManager>().enabled = false; // Disable the EnemyManager script to stop AI behavior
            StartCoroutine(DisableAfterDeathAnimation());
            gameObject.GetComponent<Collider>().enabled = false; // Disable the collider to prevent further interactions

        }
    }

    private IEnumerator DisableAfterDeathAnimation()
    {
        // Wait for the length of the death animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 5f); 
        gameObject.SetActive(false);
    }
}
