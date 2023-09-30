using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins_Kamikaze : MonoBehaviour
{
    private float force = 4;

    private void Start()
    {
        Destroy(this.gameObject, 3);
    }


    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * force, ForceMode.Impulse);
        force *= 0.5f;
    }
}
