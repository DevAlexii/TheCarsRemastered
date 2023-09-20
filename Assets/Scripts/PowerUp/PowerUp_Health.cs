using UnityEngine;

public class PowerUp_Health : PowerUpBase
{
    public override void OnClicked()
    {
        GameManager.self.UpdateLife(1);
        base.OnClicked();
    }
}
