using System;
using System.Net.Sockets;
using UnityEngine;

[ExecuteInEditMode]
public class Editor_DayTime : MonoBehaviour
{
    [SerializeField] Transform day;
    [SerializeField] Vector3 day_offset_rotation;
    private float angle_multiplier = 15f;
    [SerializeField][Range(0f, 23f)] private float time;

    [SerializeField] private Light sun, moon, sunset;
    [SerializeField][Range(0, 23)] private Int32 TimeWhenSunIsOff;
    [SerializeField][Range(0, 23)] private Int32 TimeWhenMoonIsOff;
    [SerializeField] private AnimationCurve SunsetIntensity;
    [SerializeField] private GameObject Lampioni;
    [SerializeField] private AnimationCurve lampioni_intensity;

    private float angle;
    private Int32 current_time;


    private void Start()
    {
        angle = time * angle_multiplier;
    }

    void Update()
    {
        //    angle += Time.deltaTime /** elapse_day_speed*/;
        //    if (angle >= 360) angle = 0;
        //    RotateDay();
        //    ToogleSunMoon();
        //    time = (Int32)(angle / angle_multiplier);
        //    sun.intensity = sunset_intensity_curve.Evaluate(angle);


        angle = time * angle_multiplier;
        current_time = (Int32)(angle / angle_multiplier);
        RotateDay();
        ToogleSunMoon();
        ToogleLampioni();
        sunset.intensity = SunsetIntensity.Evaluate(current_time);
    }

    private void RotateDay()
    {
        day.eulerAngles = new Vector3(angle, day_offset_rotation.y, day_offset_rotation.z);
    }
    private void ToogleSunMoon()
    {
        if (current_time >= TimeWhenSunIsOff || current_time < TimeWhenMoonIsOff)
            sun.enabled = false;
        else sun.enabled = true;

        if (current_time >= TimeWhenMoonIsOff || current_time < TimeWhenSunIsOff)
            moon.enabled = false;
        else moon.enabled = true;
    }
    private void ToogleLampioni()
    {
        foreach (var light in Lampioni.GetComponentsInChildren<Light>())
        {
            light.intensity = lampioni_intensity.Evaluate(current_time);
        }
    }
}
