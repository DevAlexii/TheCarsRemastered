using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Material invisibleMaterial;
    private Material[] start_materials;

    #region Initialized
    public void OnInitializedCar(Path newPath, int arrow_index, bool isKamikaze = false, bool has_to_be_invisible = false)
    {
        carFollowPathRef.InitilizedPath(newPath, this, isKamikaze);
        directional_arrow_index_to_play = arrow_index;
        start_materials = GetComponent<MeshRenderer>().materials;
        if (has_to_be_invisible) EnableInvisiblity();
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
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            On_Ramp_Collision();
        }
    }
    void On_Ramp_Collision()
    {
        Destroy(transform.parent.GetComponent<CarFollowPath>());
        gameObject.AddComponent<Car_Ramp_Movement>();
        Car_Manager.self.RemoveCar(transform.parent.gameObject);
    }
    #endregion

    public void OnCarClicked()
    {
        carFollowPathRef.ToogleShouldMove();
    }
    public void EnableInvisiblity()
    {
        int current_layer = transform.gameObject.layer;
        transform.gameObject.layer = current_layer == 6 ? 3 : 6; //3 = car // 6 == Invisible

        if (transform.gameObject.layer == 6)
        {
            Material[] materials_to_change = GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < materials_to_change.Length; i++)
            {
                materials_to_change[i] = invisibleMaterial;
            }
            GetComponent<MeshRenderer>().materials = materials_to_change;
        }
        else
        {
            GetComponent<MeshRenderer>().materials = start_materials;
        }
    }
}