using UnityEngine;

public class PowerUp_Shrink : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.ToggleShrink();
        GameObject effect = Instantiate(this.effect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
        base.OnClicked();
    }
}
