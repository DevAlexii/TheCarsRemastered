using UnityEngine;

public class PowerUp_Nuke : PowerUpBase
{
    [SerializeField] private GameObject nuke_prefab;
    public override void OnClicked()
    {
        GameObject.Instantiate(nuke_prefab, Vector3.up * 9f, Quaternion.identity);
        base.OnClicked();
    }
}
