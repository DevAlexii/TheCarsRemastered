using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    public GameObject[] powerUpPrefabs;
    public Transform[] spawnPoints;
    public Transform[] spawnPointsRamp; 
    public float spawnInterval = 20f;
    private float timeSinceLastSpawn;

    private void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            int randomPrefabIndex = Random.Range(0, powerUpPrefabs.Length);
            int randomSpawnPointIndex;
            Transform[] spawnPointList = spawnPoints; 

            if (powerUpPrefabs[randomPrefabIndex].layer == LayerMask.NameToLayer("Ramp"))
            {
                spawnPointList = spawnPointsRamp;
            }

            randomSpawnPointIndex = Random.Range(0, spawnPointList.Length);

            GameObject newPowerUp = Instantiate(powerUpPrefabs[randomPrefabIndex], spawnPointList[randomSpawnPointIndex].position, Quaternion.identity);

            timeSinceLastSpawn = 0f;
        }
    }
}