using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
   public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("One Handed Weapons")]
    public string oneHandedLightAttackAnimation;
    public string oneHandedHeavyAttackAnimation;

}
