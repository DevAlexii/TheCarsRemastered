using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PedestrianSpawn : MonoBehaviour
{
    [SerializeField] GameObject pedestrianPrefab;
    public List<GameObject> pedestriansRef;

    [Header("Variables")]
    [SerializeField] float timeBetweenSpawn;
    float timer;

    [SerializeField] Int32 maxPedestrian;
    Int32 currentPedestrians;

    List<Transform> spawnPoint;

    void Start()
    {
        timer = timeBetweenSpawn;
        spawnPoint = new List<Transform>();
        spawnPoint = GetComponentsInChildren<Transform>().ToList();
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
            GameObject pede = Instantiate(pedestrianPrefab, spawnPoint[pedeRand].position, Quaternion.identity);
            pedestriansRef.Add(pede);

            currentPedestrians++;

            timer = timeBetweenSpawn;
        }
    }

}
