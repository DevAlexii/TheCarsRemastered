using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour, Interactable
{
    [SerializeField] private PowerUpType powerUpType;
    public virtual void OnClickPowerUp()
    {
        switch (powerUpType)
        {
            case PowerUpType.Life:
                break;
            case PowerUpType.Invisible:
                
                break;
            case PowerUpType.Shrinking:
                break;
            case PowerUpType.SlowMo:
                break;
            case PowerUpType.Ramp:
                break;
            case PowerUpType.Nuke:
                break;
            case PowerUpType.Hook:
                break;
            case PowerUpType.Last:
                break;
            default:
                break;
        }

        Destroy(gameObject);
    }
}
public enum PowerUpType { Life, Invisible, Shrinking, SlowMo, Ramp, Nuke, Hook, Last }