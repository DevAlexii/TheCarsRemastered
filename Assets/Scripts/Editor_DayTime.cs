using System;
using System.Net.Sockets;
using UnityEngine;

[ExecuteInEditMode]
public class Editor_DayTime : MonoBehaviour
{
    [Header("DayTime")]
    [SerializeField] Transform day;
    [SerializeField] Vector3 day_offset_rotation;
    private float angle_multiplier = 15f;
    [SerializeField][Range(0f, 23f)] private float time;

    [Header("Lights")]
    [SerializeField] private Light sun, moon, sunset;
    [SerializeField] private GameObject Lampioni;

    [Header("Var")]
    private float angle;
    private Int32 current_time;


    [Header("Curve")]
    [SerializeField] private AnimationCurve sun_curve;
    [SerializeField] private AnimationCurve moon_curve;
    [SerializeField] private AnimationCurve SunsetIntensity;
    [SerializeField] private AnimationCurve lampioni_intensity;

    private void Start()
    {
        angle = time * angle_multiplier;
    }
    void Update()
    {
        angle = (time * angle_multiplier) - 90;
        current_time = (Int32)((angle + 90) / angle_multiplier);
        RotateSun();
        ToogleSunMoon();
        ToogleLampioni();
    }
    private void RotateSun()
    {
        day.eulerAngles = new Vector3(angle, day_offset_rotation.y, day_offset_rotation.z);
    }
    private void ToogleSunMoon()
    {
        sun.intensity = sun_curve.Evaluate(current_time);
        moon.intensity = moon_curve.Evaluate(current_time);
        sunset.intensity = SunsetIntensity.Evaluate(current_time);
    }
    private void ToogleLampioni()
    {
        foreach (var light in Lampioni.GetComponentsInChildren<Light>())
        {
            light.intensity = lampioni_intensity.Evaluate(current_time);
        }
    }
}
