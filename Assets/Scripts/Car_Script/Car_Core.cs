using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car_Core : MonoBehaviour, I_Interface
{
    [Header("Reference")]
    [SerializeField] private CarFollowPath carFollowPathRef;
    [SerializeField] private List<GameObject> directional_arrwos;
    [SerializeField] private List<GameObject> wheels;
    [SerializeField] private GameObject scocca;

    [Header("CollisionForce")]
    [SerializeField] float impulse_force;
    [SerializeField] float impulse_radius;

    [Header("Internal Var")]
    public bool ChangeColor = true;
    private int directional_arrow_index_to_play;
    private Material[] start_materials;
    private Material[] wheel_start_Materials;
    public bool shrink_on;
    private bool is_crashed;
    private bool selected;
    private bool isKamikaze;
    private bool isInsideTrigger = false;
    public GameObject collision_effect;

    #region Initialized
    public void OnInitializedCar(Path newPath, int arrow_index, CarInfo data, bool isKamikaze = false, bool has_to_be_invisible = false, float wait_time = 0, int score = 1)
    {
        carFollowPathRef.InitilizedPath(newPath, this, isKamikaze, data, wait_time, score);
        directional_arrow_index_to_play = arrow_index;
        wheel_start_Materials = wheels[0].GetComponent<MeshRenderer>().materials;
        if (has_to_be_invisible) EnableInvisiblity();
        this.isKamikaze = isKamikaze;
        RandomModel(data);
        RandomColor();
        start_materials = GetComponent<MeshRenderer>().materials;

    }
    private void RandomModel(CarInfo data)
    {
        int random_index = Random.Range(0, data.CarRef.Count);
        for (int i = 0; i < data.CarRef[random_index].transform.childCount; i++)
        {
            if (data.CarRef[random_index].transform.GetChild(i).name.StartsWith("scocca"))
            {
                scocca.GetComponent<MeshFilter>().sharedMesh = data.CarRef[random_index].transform.GetChild(i).GetComponent<MeshFilter>().sharedMesh;
                scocca.GetComponent<MeshRenderer>().sharedMaterials = data.CarRef[random_index].transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterials;
                scocca.GetComponent<Outline>().enabled = true;
                return;
            }
        }
    }
    void RandomColor()
    {
        if (ChangeColor)
        {
            Material[] materials = scocca.GetComponent<MeshRenderer>().materials;
            Shader_Color color = Color_Manager.self.GetRandomShaderColor;

            foreach (var material in materials)
            {
                if (material.name.StartsWith("shader"))
                {
                    material.SetColor("_top_color", color.top_color);
                    material.SetColor("_bottom_color", color.bottom_color);
                }
            }
            scocca.GetComponent<MeshRenderer>().materials = materials;
        }
    }
    #endregion

    #region DirectionalArrow
    public void ShowDirectionalArrow()
    {
        if (directional_arrwos.Count > 0)
        {
            if (directional_arrow_index_to_play >= 0)
            {
                GameObject directional_arrow = directional_arrwos[directional_arrow_index_to_play];
                directional_arrow.SetActive(true);
                directional_arrow.AddComponent<Directional_Arrow_Animation>();
            }
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
            Car_Manager.self.AddCrashedCar(transform.parent.gameObject);
            GameManager.self.E_OnCarCrash();
            if (collision_effect)
            {
                collision_effect.SetActive(true);
                Destroy(collision_effect, 2);
            }
            CheckAndDestroyIfCrashed();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ramp"))
        {
            On_Ramp_Collision();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("CrashTrigger"))
        {
            isInsideTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("CrashTrigger"))
        {
            isInsideTrigger = false;
            CheckAndDestroyIfCrashed();
        }
    }

    private void CheckAndDestroyIfCrashed()
    {
        if (is_crashed && !isInsideTrigger)
        {
            Car_Manager.self.RemoveCarFromLists(transform.parent.gameObject);
            if (transform.parent.gameObject != null)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }

    void On_Ramp_Collision()
    {
        foreach (var arrow in directional_arrwos)
        {
            Destroy(arrow);
        }
        directional_arrwos.Clear();
        Destroy(transform.parent.GetComponent<CarFollowPath>());
        transform.parent.gameObject.AddComponent<Car_Ramp_Movement>();
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
        else
        {
            if (!selected && Hook.self.enabled)
            {
                selected = true;
                Hook.self.seleceted_car++;
                startWaitSize = false;
                StartCoroutine(CarClickerAnimation());
            }
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

            transform.localScale = Vector3.Lerp(start_scale, target_scale, timer / time);

            yield return null;
        }
        float timer2 = 0;
        while (timer2 <= time)
        {
            timer2 += Time.unscaledDeltaTime;

            transform.localScale = Vector3.Lerp(target_scale, start_scale, timer2 / time);

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
            GetComponent<MeshRenderer>().materials = materials_to_change;
            GetComponent<Rigidbody>().excludeLayers = GetComponent<Rigidbody>().excludeLayers = GameManager.self.layer_to_exclude;
            GetComponent<Outline>().OutlineColor = GameManager.self.Get_Invisibility_Outline_Color;
            GetComponent<Outline>().ActiveOutline();
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
            GetComponent<MeshRenderer>().materials = start_materials;
            GetComponent<Rigidbody>().excludeLayers = GameManager.self.layer_to_exclude_default;
            GetComponent<Outline>().OutlineColor = GameManager.self.Get_Start_Outline_Color;
            GetComponent<Outline>().ActiveOutline();
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
    bool startWaitSize = true;

    public void ChangeColorGreen()
    {
        StartCoroutine(ToogleWaitSize());
    }
    IEnumerator ToogleWaitSize()
    {
        //yield return new WaitForSeconds(1f);
        Outline outlineScript = GetComponentInChildren<Outline>();
        float startScale = shrink_on ? 0.5f : 1;
        float maxScale = startScale * 1.1f;
        float minScale = startScale * 0.9f;
        float targetScale = maxScale;
        float scaleValue = startScale;

        while (startWaitSize)
        {
            scaleValue = Mathf.Lerp(scaleValue, targetScale, Time.unscaledDeltaTime * 10);
            if (scaleValue > maxScale - 0.01f && targetScale == maxScale)
            {
                scaleValue = maxScale;
                targetScale = minScale;
            }
            else if (scaleValue < minScale + 0.01f && targetScale == minScale)
            {
                scaleValue = minScale;
                targetScale = maxScale;
            }
            transform.localScale = new Vector3(startScale * scaleValue, startScale, startScale * scaleValue);

            if (outlineScript.OutlineWidth < 2.9f)
            {
                float widthTransition = Mathf.Lerp(outlineScript.OutlineWidth, 3f, Time.unscaledDeltaTime * 10);
                outlineScript.OutlineColor = Color.Lerp(outlineScript.OutlineColor, GameManager.self.GetCrashedColor, Time.unscaledDeltaTime * 10);
                outlineScript.OutlineWidth = widthTransition;
            }
            yield return null;
        }
        StopCoroutine(ToogleWaitSize());
        yield return null;
    }
}