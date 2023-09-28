using UnityEngine;

public class PowerUp_Health : PowerUpBase
{
    public override void OnClicked()
    {
        GameManager.self.UpdateLife(1);
        GameObject effect = Instantiate(this.effect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
        base.OnClicked();
    }
}
