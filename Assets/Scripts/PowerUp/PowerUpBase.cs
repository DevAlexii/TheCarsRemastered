using UnityEngine;

public class PowerUpBase : MonoBehaviour, I_Interface
{
    [SerializeField] private GameObject effect;

    private void Start()
    {
        Destroy(this.gameObject, 3f);

    }
    public virtual void OnClicked()
    {
        Destroy(this.gameObject);
    }
}
