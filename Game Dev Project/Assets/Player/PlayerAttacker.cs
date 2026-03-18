using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    PlayerAnimationHandler animHandler;
    PlayerInventory playerInventory;

    private void Awake()
    {
        animHandler = GetComponentInChildren<PlayerAnimationHandler>();
        playerInventory = GetComponentInChildren<PlayerInventory>();
    }
    public void handleLightAttack(WeaponItem weapon)
    {
      animHandler.PlayTargetAnimation(weapon.oneHandedLightAttackAnimation, true);
    }
    public void handleHeavyAttack(WeaponItem weapon)
    {
        animHandler.PlayTargetAnimation(weapon.oneHandedHeavyAttackAnimation, true);
    }
}
