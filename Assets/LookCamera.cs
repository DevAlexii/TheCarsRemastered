using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{

    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(Camera.main.transform.position, -Vector3.up);
        this.transform.localEulerAngles = new Vector3 (160, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
    }
}
