using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private Transform day_origin;
    [SerializeField] private Vector3 offset_angle;
    [SerializeField] private float angle;
    [SerializeField] private float day_time_speed;

    [SerializeField] private Light sunset;
    [SerializeField] private AnimationCurve sunset_curve;

    void Update()
    {
        angle += Time.deltaTime * day_time_speed;
        day_origin.eulerAngles = new Vector3(angle, offset_angle.y, offset_angle.z);
        if (angle >= 360) angle = 0f;
        sunset.intensity = sunset_curve.Evaluate(angle);
    }
}