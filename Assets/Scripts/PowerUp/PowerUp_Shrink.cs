using UnityEngine;

public class PowerUp_Shrink : PowerUpBase
{
    public override void OnClicked()
    {
        Car_Manager.self.ToggleShrink();
        GameObject effect = Instantiate(this.effect, transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        Destroy(effect, 1f);
        base.OnClicked();
    }
}
