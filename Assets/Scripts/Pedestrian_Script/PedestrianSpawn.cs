using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class PedestrianSpawn : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] float timeBetweenSpawn;
    float timer;

    [SerializeField] Int32 maxPedestrian;
    Int32 currentPedestrians;

    [SerializeField] List<Transform> spawnPoint;
    [SerializeField] public List<GameObject> pedestriansRef;

    void Start()
    {
        timer = timeBetweenSpawn;
    }

    void Update()
    {
        SpawnPedestrian();
    }

    void SpawnPedestrian()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && currentPedestrians <= maxPedestrian)
        {
            int pedeRand = UnityEngine.Random.Range(0, spawnPoint.Count);
            int randomPrefab = UnityEngine.Random.Range(0, pedestriansRef.Count);
            GameObject pede = Instantiate(pedestriansRef[randomPrefab], spawnPoint[pedeRand].position, Quaternion.identity);
            pedestriansRef.Add(pede);

            currentPedestrians++;

            timer = timeBetweenSpawn;
        }
    }

}
