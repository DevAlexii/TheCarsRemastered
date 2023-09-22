using System;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawn : Singleton<PedestrianSpawn>
{
    [Header("Variables")]
    [SerializeField] float timeBetweenSpawn;
    float timer;

    [SerializeField] Int32 maxPedestrian;
    Int32 currentPedestrians;

    [SerializeField] List<PathInfo> paths_Info;
    [SerializeField] public List<GameObject> pedestriansPrefab;
    [HideInInspector] public List<GameObject> pedestrians_spawned;

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
            int randomPrefab = UnityEngine.Random.Range(0, pedestriansPrefab.Count);
            int random_path = UnityEngine.Random.Range(0, paths_Info.Count);
            GameObject pede = Instantiate(pedestriansPrefab[randomPrefab], paths_Info[random_path].path.Nodes[0].position, Quaternion.identity, transform);
            pedestrians_spawned.Add(pede);
            pede.GetComponent<PedestrianMove>().Initilized(paths_Info[random_path]);
            currentPedestrians++;
            timer = timeBetweenSpawn;
        }
    }
}
[Serializable]
public struct PathInfo
{
    public Path path;
    public List<Path> cross_paths;
}
