using System.Collections.Generic;
using UnityEngine;

public class Car_Core : MonoBehaviour, Car_Interface
{
    [Header("Reference")]
    [SerializeField] private CarFollowPath carFollowPathRef;
    [SerializeField] private List<GameObject> directional_arrwos;

    [Header("CollisionForce")]
    [SerializeField] float impulse_force;
    [SerializeField] float impulse_radius;

    [Header("Internal Var")]
    private int directional_arrow_index_to_play;

    #region Initialized
    public void OnInitializedCar(Path newPath, int arrow_index, bool isKamikaze = false)
    {
        carFollowPathRef.InitilizedPath(newPath, this, isKamikaze);
        directional_arrow_index_to_play = arrow_index;
    }
    #endregion

    #region DirectionalArrow
    public void ShowDirectionalArrow()
    {
        if (directional_arrow_index_to_play >= 0)
        {
            GameObject directional_arrow = directional_arrwos[directional_arrow_index_to_play];
            directional_arrow.SetActive(true);
            directional_arrow.AddComponent<Directional_Arrow_Animation>();
        }
    }
    public void HideDirectionalArrow()
    {
        if (directional_arrwos.Count <= 0) return;
        foreach (var gameObject in directional_arrwos)
        {
            Destroy(gameObject);
        }
        directional_arrwos.Clear();
    }
    #endregion

    #region Collision
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            carFollowPathRef.OnCrash();
            GetComponent<Collider>().isTrigger = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddExplosionForce(impulse_force, other.transform.position, impulse_radius);
            HideDirectionalArrow();
        }
    }
    #endregion

    public void OnCarClicked()
    {
        carFollowPathRef.ToogleShouldMove();
    }
    public void EnableInvisiblity()
    {

    }
}