using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Core : MonoBehaviour, I_Interface
{
    [Header("Reference")]
    [SerializeField] private CarFollowPath carFollowPathRef;
    [SerializeField] private List<GameObject> directional_arrwos;
    [SerializeField] private List<GameObject> wheels;

    [Header("CollisionForce")]
    [SerializeField] float impulse_force;
    [SerializeField] float impulse_radius;

    [Header("Internal Var")]
    private int directional_arrow_index_to_play;
    private Material[] start_materials;
    private Material[] wheel_start_Materials;
    private bool shrink_on;
    private bool is_crashed;
    private bool selected;
    private bool isKamikaze;

    #region Initialized
    public void OnInitializedCar(Path newPath, int arrow_index, bool isKamikaze = false, bool has_to_be_invisible = false)
    {
        carFollowPathRef.InitilizedPath(newPath, this, isKamikaze);
        directional_arrow_index_to_play = arrow_index;
        start_materials = GetComponentInChildren<MeshRenderer>().materials;
        wheel_start_Materials = wheels[0].GetComponent<MeshRenderer>().materials;
        if (has_to_be_invisible) EnableInvisiblity();
        this.isKamikaze = isKamikaze;
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
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            carFollowPathRef.OnCrash();
            AudioCallBack.self.PlayAudio(AudioType.Crash, 1f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddExplosionForce(impulse_force, other.transform.position, impulse_radius);
            HideDirectionalArrow();
            is_crashed = true;
            Car_Manager.self.AddCrashedCar(transform.gameObject);
            GameManager.self.E_OnCarCrash();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            On_Ramp_Collision();
        }
    }
    void On_Ramp_Collision()
    {
        Destroy(transform.GetComponent<CarFollowPath>());
        gameObject.AddComponent<Car_Ramp_Movement>();
        Car_Manager.self.RemoveCar(transform.gameObject);
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
        else
        {
            if (!selected && Hook.self.enabled)
            {
                selected = true;
                Hook.self.seleceted_car++;
            }
            StartCoroutine(CarClickerAnimation());
        }

        if (isKamikaze)
        {
            Car_Manager.self.DropCoin(transform.position);
        }
    }
    private IEnumerator CarClickerAnimation()
    {
        float timer = 0;
        float time = .1f;
        Vector3 start_scale = shrink_on ? Vector3.one * 0.5f : Vector3.one;
        Vector3 target_scale = start_scale * 1.4f;

        while (timer <= time)
        {
            timer += Time.unscaledDeltaTime;

            transform.GetChild(0).localScale = Vector3.Lerp(start_scale, target_scale, timer / time);
            yield return null;
        }
        float timer2 = 0;
        while (timer2 <= time)
        {
            timer2 += Time.unscaledDeltaTime;
            transform.GetChild(0).localScale = Vector3.Lerp(target_scale, start_scale, timer2 / time);
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
            //MaterialiScocca
            Material[] materials_to_change = GetComponentInChildren<MeshRenderer>().materials;
            for (int i = 0; i < materials_to_change.Length; i++)
            {
                materials_to_change[i] = GameManager.self.GetInvisibilityMaterial;
            }
            GetComponentInChildren<MeshRenderer>().materials = materials_to_change;
            GetComponentInChildren<Outline>().OutlineColor = GameManager.self.Get_Invisibility_Outline_Color;
            GetComponent<Rigidbody>().excludeLayers = GetComponent<Rigidbody>().excludeLayers = GameManager.self.layer_to_exclude;
            //MaterialiRuote
            Material[] wheel_materials_to_change = wheels[0].GetComponent<MeshRenderer>().materials;
            for (int i = 0; i < wheel_materials_to_change.Length; i++)
            {
                wheel_materials_to_change[i] = GameManager.self.GetInvisibilityMaterial;
            }
            foreach (var wheel in wheels)
            {
                wheel.GetComponent<MeshRenderer>().materials = wheel_materials_to_change;
            }
        }
        else
        {
            //MaterialiScocca
            GetComponentInChildren<MeshRenderer>().materials = start_materials;
            GetComponentInChildren<Outline>().OutlineColor = GameManager.self.Get_Start_Outline_Color;
            GetComponent<Rigidbody>().excludeLayers = GameManager.self.layer_to_exclude_default;
            //MaterialiRuote
            foreach (var wheel in wheels)
            {
                wheel.GetComponent<MeshRenderer>().materials = wheel_start_Materials;
            }
        }
    }
    public void EnableShrink()
    {
        shrink_on = true;
    }
    #endregion
}