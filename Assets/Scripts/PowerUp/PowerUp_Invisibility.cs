using UnityEngine;

public class PowerUp_Invisibility : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.ToggleInvisibility();
        base.OnClicked();
    }
}
