using UnityEngine;

public class PowerUp_Shrink : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.ToggleShrink();
        base.OnClicked();
    }
}
