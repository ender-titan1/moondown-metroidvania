using Moondown.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Moondown.UI
{
    public class DisplayInventory : MonoBehaviour
    {
        string filter = "";
        List<GameObject> slots = new List<GameObject>();

        public void Awake()
        {
            foreach (Transform slot in transform)
            {
                slots.Add(slot.gameObject);
            }
        }

        public void OnEnable()
        {
            foreach (Transform slot in transform)
            {
                slots.Add(slot.gameObject);
            }

            LoadPage(InventoryManager.Instance.Resources);
        }

        public void OnDisable()
        {
            UnloadPage();
        }

        public void UnloadPage()
        {
            foreach(GameObject slot in slots)
            {
                slot.GetComponentInChildren<RawImage>().enabled = false;
                slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        public void LoadPage(List<Item> items)
        {
            var stacks = InventoryManager.Instance.GetInventory(items, filter);

            for (int i = 0; i < stacks.ToArray().Length; i++)
            {
                ItemStack s = stacks[i];
                GameObject slot = slots[i];

                slot.GetComponentInChildren<RawImage>().texture = s.item.data.image;
                slot.GetComponentInChildren<RawImage>().enabled = true;


                if (s.amount > 1)
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = s.amount.ToString();
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
