using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRamp : PowerUp
{
    public GameObject rampPrefab;
    public Quaternion[] spawnRotations;

    public override void OnClickPowerUp()
    {
        SpawnRamp();
    }
    private void SpawnRamp()
    {
        int randomIndex = Random.Range(0, spawnRotations.Length);
        Quaternion spawnRotation = spawnRotations[randomIndex];

        Instantiate(rampPrefab, transform.position, spawnRotation);
    }

}