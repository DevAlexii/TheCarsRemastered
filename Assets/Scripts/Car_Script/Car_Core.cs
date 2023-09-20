using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Core : MonoBehaviour, I_Interface
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
    private bool shrink_on;
    private bool is_crashed;

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
            is_crashed = true;

            Car_Manager.self.AddCrashedCar(transform.parent.gameObject);
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

    #region CarClicked
    public void OnClicked()
    {
        if (!is_crashed)
        {
            if (carFollowPathRef.ClickedCar())
            {
                StartCoroutine(CarClickerAnimation());
            }
        }
    }
    private IEnumerator CarClickerAnimation()
    {
        float timer = 0;
        float time = .2f;
        Vector3 start_scale = shrink_on ? (Vector3.one * 0.01f) * 0.5f : Vector3.one * 0.01f;
        Vector3 target_scale = start_scale * 1.2f;

        while (timer <= time)
        {
            timer += Time.deltaTime;

            transform.parent.localScale = Vector3.Lerp(start_scale, target_scale, timer / time);
            yield return null;
        }
        float timer2 = 0;
        while (timer2 <= time)
        {
            timer2 += Time.deltaTime;
            transform.parent.localScale = Vector3.Lerp(target_scale, start_scale, timer2 / time);
            yield return null;
        }
        StopCoroutine(CarClickerAnimation());
    }
    #endregion

    #region InterfaceImplemented
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
    public void EnableShrink()
    {
        shrink_on = true;
    }
    #endregion
}