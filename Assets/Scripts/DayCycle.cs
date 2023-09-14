using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private Transform day_origin;
    [SerializeField] private float angle;
    [SerializeField] private float day_time_speed;


    void Update()
    {
        angle += Time.deltaTime * day_time_speed;
        day_origin.eulerAngles = Vector3.right * angle;
        if (angle >= 360) angle = 0f;
    }
}
