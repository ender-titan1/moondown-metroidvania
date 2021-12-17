using System;
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

    public void Load(GameObject[] slots, GameObject[] quickBarSlots, Sprite baseSprite, GameObject UI)
    {

        for (int i = 0; i < slots.Length; i++)
        {
            Sprite itemSprite = null;

            try
            {
                itemSprite = (
                    from IInventoryItem item in EquipmentManager.Instance.Inventory
                    where item.SlotNumber == i
                    select item.ImageWithSlot
                ).ToArray().First();
                
            } catch (Exception)
            {
                continue;
            }
                

            slots[i].GetComponent<RawImage>().texture = itemSprite.texture;
            
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

            items.RemoveAt(0);
        }

    }

    public static Sprite MergeTextures(Sprite[] sprites)
    {
        Resources.UnloadUnusedAssets();
        Texture2D newTexture = new Texture2D(320, 320);

        for (int y = 0; y < newTexture.height; y++)
        {
            for (int x = 0; x < newTexture.width; x++)
            {
                newTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
            }
        }

        for (int i = 0; i < sprites.Length; i++)
        {
            for (int y = 0; y < newTexture.height; y++)
            {
                for (int x = 0; x < newTexture.width; x++)
                {
                    Color color = sprites[i].texture.GetPixel(x, y).a == 0 ?
                        newTexture.GetPixel(x, y) :
                        sprites[i].texture.GetPixel(x, y);

                    newTexture.SetPixel(x, y, color);
                }
            }
        }

        newTexture.Apply();
        Sprite finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
        finalSprite.name = "Inevntory slot";
        return finalSprite;
    }
}
