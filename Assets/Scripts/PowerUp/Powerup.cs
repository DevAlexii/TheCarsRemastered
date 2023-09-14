using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, Interactable
{
   
    public virtual void OnClickPowerUp()
    {
        Destroy(gameObject);
    }
}
