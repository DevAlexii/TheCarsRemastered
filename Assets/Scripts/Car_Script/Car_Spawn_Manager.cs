using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car_Spawn_Manager : Singleton<Car_Spawn_Manager>
{
    [SerializeField] List<Path_Dictionary> paths;
    [SerializeField] private float timer_to_spawn_car;
    [SerializeField] List<GameObject> car_prefabs;
    private float timer;
    private List<GameObject> spawned_car;
    private Dictionary<Direction, Dictionary<Point, List<Path>>> paths_dictionary;

    private void Start()
    {
        spawned_car = new List<GameObject>();
        paths_dictionary = new Dictionary<Direction, Dictionary<Point, List<Path>>>();
        for (int i = 0; i < paths.Count; i++)
        {
            paths_dictionary[paths[i].Key] = new Dictionary<Point, List<Path>>();
            for (int j = 0; j < paths[i].Value.Count; j++)
            {
                paths_dictionary[paths[i].Key][paths[i].Value[j].Key] = paths[i].Value[j].Value;
            }
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timer_to_spawn_car)
        {
            SpawnCar();
            timer = 0;
        }
    }
    void SpawnCar()
    {
        Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
        Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
        int random_path = Random.Range(0, 2);
        Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];

        int random_index = Random.Range(0, car_prefabs.Count);
        int arrow_index = -1;
        if (randomPoint == Point.Left && random_path == 0)
        {
            arrow_index = 0;
        }
        else if (randomPoint == Point.Right && random_path == 0)
        {
            arrow_index = 1;
        }
        GameObject car = Instantiate(car_prefabs[random_index], pathRef.Nodes[0].position, Quaternion.identity);
        car.GetComponent<Car_Core>().OnInitializedCar(pathRef, arrow_index);
        spawned_car.Add(car);
    }
    public void RemoveCar(GameObject car)
    {
        spawned_car.Remove(car);
    }
}
[Serializable]
struct Path_Dictionary
{
    [SerializeField] private Direction key;
    [SerializeField] private List<PathStart> value;
    public Direction Key => key;
    public List<PathStart> Value => value;
}
[Serializable]
struct PathStart
{
    [SerializeField] private Point key;
    [SerializeField] private List<Path> value;
    public Point Key => key;
    public List<Path> Value => value;
}