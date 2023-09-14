using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShrink : PowerUp
{
    public override void OnClickPowerUp()
    {
        Car_Manager.self.ToggleShrink();
    }
}

