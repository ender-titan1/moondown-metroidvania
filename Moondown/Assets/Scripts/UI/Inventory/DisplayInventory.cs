using Moondown.Inventory;
using Moondown.Utility;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Moondown.UI.Inventory
{
    public class DisplayInventory : MonoBehaviour
    {
        private string   filter = "";
        private readonly List<GameObject> slots = new List<GameObject>();
        private List<ItemStack> currentPage = InventoryManager.Instance.Weapons;

        [Header("Data Panel")]
        [SerializeField] private GameObject      dataPanel;
        [SerializeField] private TextMeshProUGUI dataPanelTitle;
        [SerializeField] private TextMeshProUGUI dataPanelSubHeading;
        [SerializeField] private RawImage        dataPanelImage;
        [SerializeField] private Button          dataPanelEquipButton;

        [Header("Equipment")]
        [SerializeField] private GameObject      equiment;

        private void Awake()
        {
            DataPanel.Init(dataPanel, dataPanelTitle, dataPanelSubHeading, dataPanelImage, dataPanelEquipButton);
        }

        public void OnEnable()
        {
            foreach (Transform transform in transform)
            {
                if (transform.gameObject.Has<Slot>())
                    slots.Add(transform.gameObject);
            }

            RefreshEquipment();

            LoadPage();
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
                slot.GetComponent<Slot>().content = null;
            }
        }

        public void LoadPage()
        {

            var stacks = InventoryManager.Instance.GetInventory(currentPage, filter);

            for (int i = 0; i < stacks.ToArray().Length; i++)
            {
                ItemStack s = stacks[i];
                GameObject slot = slots[i];

                slot.GetComponentInChildren<RawImage>().texture = s.item.data.image;
                slot.GetComponentInChildren<RawImage>().enabled = true;
                slot.GetComponent<Slot>().content = s;


                if (s.amount > 1)
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = s.amount.ToString();
                else
                    slot.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        public void OnSearchChange()
        {
            filter = gameObject.GetComponentInChildren<TMP_InputField>().text;
            // reload the page
            UnloadPage();
            LoadPage();
        }

        public void OnSwitchPage(string name)
        {
            PropertyInfo prop = InventoryManager.Instance.GetType().GetProperty(name);

            var page = currentPage;
            
            currentPage = (List<ItemStack>)prop.GetValue(InventoryManager.Instance, null);

            if (currentPage != page)
            {
                UnloadPage();
                LoadPage();
            }

            try
            {
                DataPanel.Hide();
            }
            catch (NullReferenceException)
            {
                //ignore
            }
        }

        public void OnButtonEnter(Animator animator)
        {
            animator.enabled = true;
            gameObject.GetComponent<InventoryNavigation>().Clear(animator.GetComponentInParent<RectTransform>().gameObject);
            animator.SetBool("Mouse off", false);

        }

        public void OnButtonExit(Animator animator)
        {
            animator.SetBool("Mouse off", true);
            animator.GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        public void SelectPage(GameObject @object)
        {
            // close the side bar
            InventoryNavigation.Instance.SideBarActive = false;
        }

        public void RefreshEquipment()
        {
            //if (InventoryManager.Instance.equiped.meele == null)
            //{
            //   equiment.GetComponent<RawImage>().enabled = false;
            //    return;
            //}

            //equiment.GetComponent<RawImage>().enabled = true;
            //equiment.GetComponent<RawImage>().texture = InventoryManager.Instance.equiped.meele.data.image;
        }
    }
}
