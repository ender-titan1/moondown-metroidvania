using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class Slot : MonoBehaviour
{
    [HideInInspector]
    public IInventoryItem item = null;

    public void OnClick(Sprite @base)
    {
        if (item == null)
            return;

        if (item.Type == ItemType.MEELE_WEAPON)
        {
            Texture2D texture = item.ImageWithSlot.texture;
            IInventoryItem selectedItem = item;

            Slot[] slots = (from GameObject slot in InventoryDisplay.Instance.allSlots 
                            where slot.GetComponent<Slot>().item == item
                            select slot.GetComponent<Slot>()
                           ).ToArray();


            foreach (Slot slot in slots)
            {
                slot.gameObject.GetComponent<RawImage>().texture = @base.texture;
                slot.item = null;
            }

            InventoryDisplay.Instance.equipedWeaponSlot.GetComponent<RawImage>().texture = texture;
            InventoryDisplay.Instance.equipedWeaponSlot.GetComponent<Slot>().item = selectedItem;

        }       
    }
}
