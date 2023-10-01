using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Coins : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.IncrementCoins();
        GameObject effect = Instantiate(this.effect, transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        Destroy(effect, 1f);
        base.OnClicked();
    }
}
