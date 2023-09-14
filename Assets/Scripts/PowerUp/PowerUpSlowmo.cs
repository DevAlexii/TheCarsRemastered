using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSlowmo : PowerUp
{
    [SerializeField] float slowdownDuration;

    public override void OnClickPowerUp()
    {
        Car_Manager.self.ToggleSlowDownGame(slowdownDuration);
    }
}
