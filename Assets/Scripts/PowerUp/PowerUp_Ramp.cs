using UnityEngine;

public class PowerUp_Ramp : PowerUpBase
{
    [SerializeField] private GameObject ramp_prefab;
    public override void OnClicked()
    {
        int random_angle = Random.Range(0, 4) * 90;
        Quaternion rot = Quaternion.Euler(Vector3.up * random_angle);
        GameObject ramp = Instantiate(ramp_prefab, Vector3.zero, rot);
        Destroy(ramp,5);
        base.OnClicked();
    }
}
