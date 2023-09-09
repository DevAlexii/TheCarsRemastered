using UnityEngine;

public class Car_Core : MonoBehaviour,Car_Interface
{
    [SerializeField] private CarFollowPath carFollowPathRef;

    public void OnCarClicked()
    {
        carFollowPathRef.ToogleShouldMove();
    }

    public void EnableInvisiblity()
    {
       
    }
}
