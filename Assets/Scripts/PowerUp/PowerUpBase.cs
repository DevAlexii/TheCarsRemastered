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
        AudioCallback.self.PlayAudioSource(0.5f);
        Destroy(this.gameObject);
    }
}
