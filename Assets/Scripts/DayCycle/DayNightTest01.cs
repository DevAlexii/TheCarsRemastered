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

    [Header("Lamps")]
    [SerializeField] GameObject lamps;
    [Range(0f, 1f)][SerializeField] float turnOff;
    [Range(0f, 1f)][SerializeField] float turnOn;

    [Header("RenderingSettings")]
    [SerializeField] AnimationCurve intensity_multiplier;
    [SerializeField] AnimationCurve reflexion_multiplier;



    void Start()
    {
        timeRate = 1f / dayLenght;

        time = startday;
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
        }
        else if (time > turnOff && time < turnOn && lamps.activeSelf)
        {
            lamps.SetActive(false);
        }
    }
}