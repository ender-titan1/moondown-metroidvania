using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

namespace Unity.Services.Analytics
{
    public class AnalyticsController : MonoBehaviour
    {
        MainControls controls;

        private void Awake()
        {
            controls = new MainControls();

            controls.Player.Jump.performed += _ =>
            {
                AnalyticsService.Instance.CustomData("playerJump", new Dictionary<string, object>());
            };
        }

        async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            }
            catch (ConsentCheckException)
            { }
        }

        private void OnEnable()
        {
            controls.Enable();
        }
    }
}