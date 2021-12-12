using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    public List<List<AbstractEquipable>> presets = new List<List<AbstractEquipable>> { };
    public List<AbstractEquipable> inventory = new List<AbstractEquipable> { };

    public MeeleWeapon meeleWeapon;
    public AbstractEquipable rangedWeapon;
    public AbstractEquipable armour;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Equip(AbstractEquipable equipable)
    {
        if (equipable is MeeleWeapon eq)
            meeleWeapon = eq;
    }    



}
