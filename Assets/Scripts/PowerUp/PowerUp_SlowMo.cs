using UnityEngine;

public class PowerUp_SlowMo : PowerUpBase
{
    [SerializeField] private float duration;
    public override void OnClicked()
    {
        Car_Manager.self.ToggleSlowDownGame(duration);
        GameObject effect = Instantiate(this.effect, transform.position - new Vector3(0f, 0.5f, 0f), Quaternion.identity);
        Destroy(effect, 1f);
        base.OnClicked();
    }
}
