using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class Slot : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public IInventoryItem item = null;

    public void OnClick(Sprite @base)
    {
        if (item == null)
            return;

        if (item.Type == ItemType.MEELE_WEAPON)
        {
            GameObject s = InventoryDisplay.Instance.equipedWeaponSlot;

            if (s == gameObject)
            {
                Weapon weapon = (Weapon)item;

                s.GetComponent<RawImage>().enabled = false;
                s.GetComponent<RawImage>().texture = null;
                s.GetComponent<Slot>().item = null;

                EquipmentManager.Instance.UnequipWeapon();

                weapon.SlotNumber = EquipmentManager.Instance.NextFreeSlot;
                InventoryDisplay.Instance.Load();

                return;
            }

            Texture2D texture = item.Image.texture;
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

            
            s.GetComponent<RawImage>().enabled = true;
            s.GetComponent<RawImage>().texture = texture;
            s.GetComponent<Slot>().item = selectedItem;
            EquipmentManager.Instance.Equip((Weapon)selectedItem);

        }       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            OnClick(InventoryDisplay.Instance.baseSlotTexture);
    }
}
