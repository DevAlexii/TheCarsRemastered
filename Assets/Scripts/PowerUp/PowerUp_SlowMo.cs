using UnityEngine;

public class PowerUp_SlowMo : PowerUpBase
{
    [SerializeField] private float duration;
    public override void OnClicked()
    {
        Car_Manager.self.ToggleSlowDownGame(duration);
        base.OnClicked();
    }
}
