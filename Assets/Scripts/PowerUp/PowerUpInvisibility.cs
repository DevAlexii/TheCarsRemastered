using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpInvisibility : PowerUp
{
    public override void OnClickPowerUp()
    {
        Car_Manager.self.ToggleInvisibility();
    }
}

