using UnityEngine;

public class PowerUpBase : MonoBehaviour,I_Interface
{
    [SerializeField] private GameObject effect;

    public virtual void OnClicked()
    {
        Destroy(this.gameObject);
    }
}
