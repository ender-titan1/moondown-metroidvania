using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Item : IInventoryItem
{
    public string Name          { get; set; }
    public string Description   { get; set; }
    public ItemType Type        { get; set; }
    public int SlotNumber       { get; set; }
    public Sprite Image         { get; set; }
    public Sprite ImageWithSlot { get; set; }

    public Item(string name, string desc, string spriteName, ItemType type, Sprite baseSprite, int slotNumber)
    {
        this.Name = name;
        this.Description = desc;

        this.Type = type;

        string spritePath = @"Assets/Sprites/" + spriteName + ".png";
        this.Image = (Sprite)AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite));

        this.ImageWithSlot = new Sprite[] { baseSprite, Image }.MergeSprites();

        this.SlotNumber = slotNumber;
    }

}
