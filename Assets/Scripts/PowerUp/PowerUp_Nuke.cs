using UnityEngine;

public class PowerUp_Nuke : PowerUpBase
{
    [SerializeField] private GameObject nuke_prefab;
    public override void OnClicked()
    {
        AudioCallBack.self.PlayAudio(AudioType.Bomb, 1f);
        Instantiate(nuke_prefab, Vector3.up * 9f, Quaternion.identity);
        base.OnClicked();
    }
}
