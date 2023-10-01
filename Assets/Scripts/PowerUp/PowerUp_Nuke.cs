using UnityEngine;

public class PowerUp_Nuke : PowerUpBase
{
    [SerializeField] private GameObject nuke_prefab;
  
    public override void OnClicked()
    {
        AudioCallBack.self.PlayAudio(AudioType.Bomb, 1f);
        Instantiate(nuke_prefab, Vector3.up * 10f, Quaternion.Euler(180, 0, 0));
        nuke_prefab.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
        base.OnClicked();
    }
}
