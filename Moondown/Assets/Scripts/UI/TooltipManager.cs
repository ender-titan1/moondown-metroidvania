/*
    Copyright (C) 2021 Moondown Project

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TooltipManager : MonoBehaviour
{
    private const float TEXT_PADDING = 4f;

    public static TooltipManager Instance { get; set; }

    public string TooltipNameContent { get; set; } = "<Test>";

    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private TextMeshProUGUI text;

    private bool overridePosition;

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    private void Update()
    {
        if (overridePosition)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), mousePos, null, out Vector2 pos);

        pos.x += tooltipRect.sizeDelta.x / 2;
        pos.y += tooltipRect.sizeDelta.y / 2;

        tooltipRect.gameObject.transform.localPosition = pos;
    }

    public void ShowTooltip(string tooltipName, Vector2? pos = null)
    {
        if (pos != null)
        {
            tooltipRect.gameObject.transform.position = pos.Value;
            overridePosition = true;
        }
        else
            overridePosition = false;

        TooltipNameContent = tooltipName;
        text.text = tooltipName;

        tooltipRect.GetComponent<RectTransform>().gameObject.SetActive(true);

        Vector2 backgroundSize = new Vector2(text.preferredWidth + TEXT_PADDING * 2, text.preferredHeight + TEXT_PADDING * 2);
        tooltipRect.sizeDelta = backgroundSize;
        backgroundRect.sizeDelta = backgroundSize;
    }

    public void HideTooltip()
    {
        overridePosition = false;
        tooltipRect.GetComponent<RectTransform>().gameObject.SetActive(false);
    }
}
