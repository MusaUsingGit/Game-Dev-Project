using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;    
    public WeaponItem rightHandWeapon;
    public WeaponItem leftHandWeapon;
    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;
    public void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();

    }
    public void Start()
    {
        rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
        leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
        weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        currentRightWeaponIndex = currentRightWeaponIndex + 1;
        if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);        
        }else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }

        else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
        {
            rightHandWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);
        }else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] == null)
        {
            currentRightWeaponIndex = currentRightWeaponIndex + 1;
        }
        if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
        {
            currentRightWeaponIndex = -1;
            rightHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(rightHandWeapon, false);   
        }
    }

    public void ChangeLeftWeapon()
    {
       currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, false);        
        }else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }

        else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
        {
            leftHandWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, false);
        }else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] == null)
        {
            currentLeftWeaponIndex = currentLeftWeaponIndex + 1;
        }
        if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
        {
            currentLeftWeaponIndex = -1;
            leftHandWeapon = unarmedWeapon;
            weaponSlotManager.LoadWeaponOnSlot(leftHandWeapon, false);   
        }
    }
}
