using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PedestrianSpawn : MonoBehaviour
{
    [SerializeField] GameObject pedestrianPrefab;


    [Header("Variables")]
    [SerializeField] float timeBetweenSpawn;
    [SerializeField] Int32 maxPedestrian;
    float timer;
    List<Transform> spawnPoint;

    void Start()
    {
        timer = timeBetweenSpawn;
        spawnPoint = new List<Transform>();
        spawnPoint = GetComponentsInChildren<Transform>().ToList();
    }

    void Update()
    {

    }


    void SpawnPedestrian()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int pedeRand = UnityEngine.Random.Range(0,spawnPoint.Count);
            GameObject pede = Instantiate(pedestrianPrefab, spawnPoint[pedeRand]);


            timer = timeBetweenSpawn;
        }
    }
}
