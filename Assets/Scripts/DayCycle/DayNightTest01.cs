using System.Collections.Generic;
using UnityEngine;

public class DayNightTest01 : MonoBehaviour
{
    [Header("Day_Varaiables")]
    [Range(0f, 1f)] public float time;
    public float dayLenght;
    public float startday;
    private float timeRate;

    [Header("Sun")]
    [SerializeField] Light sun;
    [SerializeField] AnimationCurve sunIntensity;
    public Gradient sunGradient;

    [Header("AmbientLight")]
    [SerializeField] Light ambient;
    [SerializeField] AnimationCurve ambientIntensity;
    public Gradient ambientGradient;

    [Header("RenderingSettings")]
    [SerializeField] AnimationCurve intensity_multiplier;
    [SerializeField] AnimationCurve reflexion_multiplier;

    [Header("Lamps")]
    [SerializeField] GameObject lamps;
    [Range(0f, 1f)][SerializeField] float turnOff;
    [Range(0f, 1f)][SerializeField] float turnOn;
    [SerializeField] List<GameObject> lampsList;
    [SerializeField] Material luci2Material;
    private Material originalMaterial;

    [Header("EmissiveCity")]
    [SerializeField] List<GameObject> glassObj;
    private bool lightsAreOn = false;


    void Start()
    {
        timeRate = 1f / dayLenght;
        time = startday;

        if (lampsList.Count > 0)
        {
            foreach (GameObject lamp in lampsList)
            {
                Renderer lampRenderer = lamp.GetComponent<Renderer>();
                if (lampRenderer != null)
                {

                    if (lampRenderer.materials.Length >= 2)
                    {
                        originalMaterial = lampRenderer.materials[1];
                    }
                }
            }
        }
    }

    void Update()
    {
        time += timeRate * Time.deltaTime;
        if (time >= 1f)
        {
            time = 0f;
        }
        SetLighting();
        SetSunAmbientintensity();
        ToogleLamps();
        ToggleGlassObj();
    }
    void SetLighting()
    {
        RenderSettings.ambientIntensity = intensity_multiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflexion_multiplier.Evaluate(time);
    }
    void SetSunAmbientintensity()
    {
        sun.intensity = sunIntensity.Evaluate(time);
        sun.color = sunGradient.Evaluate(time);

        ambient.intensity = ambientIntensity.Evaluate(time);
        ambient.color = ambientGradient.Evaluate(time);
    }
    void ToogleLamps()
    {
        if (time >= turnOn || time < turnOff && !lamps.activeSelf)
        {
            lamps.SetActive(true);

            if (lampsList.Count > 0)
            {
                foreach (GameObject lamp in lampsList)
                {
                    Renderer lampRenderer = lamp.GetComponent<Renderer>();
                    if (lampRenderer != null)
                    {
                        if (lampRenderer.materials.Length >= 2)
                        {
                            Material[] materials = lampRenderer.materials;
                            materials[1] = luci2Material;
                            lampRenderer.materials = materials;
                        }
                    }
                }
            }
        }
        else if (time > turnOff && time < turnOn && lamps.activeSelf)
        {
            lamps.SetActive(false);

            if (lampsList.Count > 0)
            {
                foreach (GameObject lamp in lampsList)
                {
                    Renderer lampRenderer = lamp.GetComponent<Renderer>();
                    if (lampRenderer != null)
                    {
                        if (lampRenderer.materials.Length >= 2)
                        {
                            Material[] materials = lampRenderer.materials;
                            materials[1] = originalMaterial;
                            lampRenderer.materials = materials;
                        }
                    }
                }
            }
        }
    }

    void ToggleGlassObj()
    {
        if (time >= turnOn || time < turnOff)
        {
            if (!lightsAreOn)
            {
                lightsAreOn = true;
                if (glassObj.Count > 0)
                {
                    foreach (GameObject obj in glassObj)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }
        else
        {
            if (lightsAreOn)
            {
                lightsAreOn = false;
                if (glassObj.Count > 0)
                {
                    foreach (GameObject obj in glassObj)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }


    }
}