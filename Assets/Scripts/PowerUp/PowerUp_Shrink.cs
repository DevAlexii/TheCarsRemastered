using UnityEngine;

public class PowerUp_Shrink : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.ToggleShrink();
        GameObject effect = Instantiate(this.effect,Vector3.zero,Quaternion.identity);
        Destroy(effect,1.5f);
        base.OnClicked();
    }
}
