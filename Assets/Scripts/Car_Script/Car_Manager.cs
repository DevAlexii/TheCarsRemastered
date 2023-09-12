using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car_Manager : Singleton<Car_Manager>
{
    [SerializeField] List<Path_Dictionary> paths;
    [SerializeField] private float timer_to_spawn_car;
    [SerializeField] List<GameObject> car_prefabs;
    [SerializeField] private Int32 max_car_in_scene;
    [SerializeField] private Int32 percentage_to_be_kamikaze;
    private float timer;
    private List<GameObject> spawned_car;
    private Dictionary<Direction, Dictionary<Point, List<Path>>> paths_dictionary;

    [Header("Invisibility")]
    [SerializeField] private float invisibility_time;
    private Action On_Invisibility;
    private float invisibility_timer;
    private bool invisibility_on;

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
        On_Invisibility?.Invoke();
    }
    void SpawnCar()
    {
        if (spawned_car.Count > 0 && spawned_car.Count >= max_car_in_scene) return;

        Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
        Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
        int random_path = Random.Range(0, 2);
        Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];

        int random_index = Random.Range(0, car_prefabs.Count);
        int arrow_index = -1;
        bool isKamikaze = true;
        if (randomPoint == Point.Left && random_path == 0)
        {
            arrow_index = 0;
            isKamikaze = false;
        }
        else if (randomPoint == Point.Right && random_path == 0)
        {
            arrow_index = 1;
            isKamikaze = false;
        }
        if (isKamikaze)
        {
            if (!CustomLibrary.RandomBoolInPercentage(percentage_to_be_kamikaze))
            {
                isKamikaze = false;
            }
        }
        GameObject car = Instantiate(car_prefabs[random_index], pathRef.Nodes[0].position, Quaternion.identity);
        car.GetComponentInChildren<Car_Core>().OnInitializedCar(pathRef, arrow_index, isKamikaze, invisibility_on);
        spawned_car.Add(car);
    }
    public void RemoveCar(GameObject car)
    {
        spawned_car.Remove(car);
    }

    #region PowerUp
    #region Invisilibity
    void InvisibilityTimer()
    {
        invisibility_timer += Time.deltaTime;
        if (invisibility_timer >= invisibility_time)
        {
            invisibility_timer = 0;
            ToogleInvisibility();
            invisibility_on = false;
            On_Invisibility = null;
        }
    }
    public void ToogleInvisibility()
    {
        foreach (var car in spawned_car)
        {
            if (car.transform.GetChild(0).TryGetComponent(out Car_Interface car_Interface))
            {
                car_Interface.EnableInvisiblity();
            }
        }
        invisibility_on = true;
        On_Invisibility = InvisibilityTimer;
    }
    #endregion
    #endregion
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