﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay
{
    enum LastViewed
    {
        MEELE_WEAPON,
        RANGED_WEAPON,
        ARMOUR,
        GENERAL
    }

    public static InventoryDisplay Instance { get; private set; } = new InventoryDisplay();

    private LastViewed lastViewed = LastViewed.MEELE_WEAPON;

    public GameObject[] slots;
    public GameObject[] quickSelectSlots;
    public GameObject equipedWeaponSlot;
    public GameObject[] allSlots;
    public Sprite baseSlotTexture;

    public void Load(GameObject[] slots, GameObject[] quickBarSlots, Sprite baseSprite, GameObject UI, GameObject equipedWeaponSlot)
    {
        this.equipedWeaponSlot = equipedWeaponSlot;
        this.quickSelectSlots = quickBarSlots;
        this.slots = slots;
        this.baseSlotTexture = baseSprite;

        List<GameObject> allSlots = slots.ToList();
        allSlots.AddRange(quickBarSlots);
        this.allSlots = allSlots.ToArray();

        for (int i = 0; i < slots.Length; i++)
        {
            IInventoryItem item = null;

            try
            {
                item = (
                    from IInventoryItem it in EquipmentManager.Instance.Inventory
                    where it.SlotNumber == i
                    select it
                ).ToArray().First();
                
            } catch (Exception)
            {
                continue;
            }
                

            slots[i].GetComponent<RawImage>().texture = item.ImageWithSlot.texture;
            slots[i].GetComponent<Slot>().item = item;
            
        }

        if (lastViewed == LastViewed.MEELE_WEAPON)
            LoadRelevantItems(quickBarSlots, baseSprite, ItemType.MEELE_WEAPON, ItemType.TOOL);

        UI.SetActive(true);
    }

    private void LoadRelevantItems(GameObject[] slots, Sprite baseSprite, params ItemType[] types)
    {
        List<IInventoryItem> items = new List<IInventoryItem> { };

        for (
            int i = 0;
            i < slots.Length && 
            i < EquipmentManager.Instance.Inventory.ToArray().Length; 
            i++
        )
        {
            IInventoryItem item = EquipmentManager.Instance.Inventory[i];

            if (types.Contains(item.Type))
                items.Add(item);
        }


        foreach (GameObject slot in slots)
        {
            if (items.ToArray().Length == 0)
                break;

            IInventoryItem item = items.ToArray().First();

            slot.GetComponent<RawImage>().texture = item.ImageWithSlot.texture;
            slot.GetComponent<Slot>().item = item;

            items.RemoveAt(0);
        }

    }

    
}
