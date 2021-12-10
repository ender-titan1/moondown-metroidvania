using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerModule : AbstractModule
{
    public BasePlayerModule()
    {
        setup();
    }

    public override void OnHeal(int amount)
    {
        PlayerManager.Instance.Health += amount;
    }

    public override void OnCharge(int amount)
    {
        PlayerManager.Instance.Charge += amount;
    }

    public override void OnDamageTaken(int amount)
    {
        // this is done because the 'amount' parameter is negative
        PlayerManager.Instance.Health += amount;
    }
}
