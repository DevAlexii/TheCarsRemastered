using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Coins : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.IncrementCoins();
        base.OnClicked();
    }
}
