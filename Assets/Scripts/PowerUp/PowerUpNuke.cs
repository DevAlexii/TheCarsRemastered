using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PowerUpNuke : PowerUp
{
    [SerializeField] float explosionForce;
    [SerializeField] float explosionRadius;
    public GameObject nukePrefab;
    public float heightNuke = 9f;
    public override void OnClickPowerUp()
    {
        SpawnNuke();
        OnCollisionEnter();
    }
    private void OnCollisionEnter()
    {
        Car_Manager.self.toggleNuke(explosionForce, explosionRadius);
    }

    private void SpawnNuke()
    {
        Vector3 spawnPos = new Vector3(0f, heightNuke,0f);
        Instantiate(nukePrefab, spawnPos, Quaternion.identity);
    }
}
