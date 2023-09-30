using System.Collections;
using UnityEngine;

public class PowerUpBase : MonoBehaviour, I_Interface
{
    [SerializeField] protected GameObject effect;

    private void Start()
    {
        Destroy(this.gameObject, 3f);
    }
    public virtual void OnClicked()
    {
        AudioCallBack.self.PlayAudio(AudioType.Coin,0.8f);
        Destroy(this.gameObject);
    }
}
