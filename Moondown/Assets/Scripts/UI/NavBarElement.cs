using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Moondown.UI
{
    public class NavBarElement : MonoBehaviour
    {
        public void Select(bool value)
        {
            if (value)
                GetComponent<TMPro.TextMeshProUGUI>().color = new Color(1, 0.11461f, 0);
            else
                GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;
        }

        public void Activate()
        {
            if (gameObject.GetComponent<EventTrigger>() != null)
            {
                gameObject.GetComponent<EventTrigger>().OnPointerDown(null);
            }
        }
    }
}