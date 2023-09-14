using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerUpNuke : PowerUp
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;

    private void OnCollisionEnter(Collision collision)
    {
        Car_Manager.self.toggleNuke(explosionForce, explosionRadius);
        
    }
}
