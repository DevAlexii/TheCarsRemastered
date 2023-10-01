using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car_Manager : Singleton<Car_Manager>
{
    [SerializeField] List<Path_Dictionary> paths;
    [SerializeField] private float timer_to_spawn_car;
    public GameObject car_prefab;
    public Dictionary<CarType, List<CarInfo>> CarInfos;
    [SerializeField] private Int32 max_car_in_scene;
    [SerializeField] private Int32 percentage_to_be_kamikaze;
    private float timer;

    private List<GameObject> spawned_car;
    public List<GameObject> car_crashed { get; private set; }
    private Dictionary<Direction, Dictionary<Point, List<Path>>> paths_dictionary;

    [Header("Invisibility")]
    [SerializeField] private float invisibility_time;
    private Action On_Invisibility;
    private float invisibility_timer;
    private bool invisibility_on;

    [Header("Shrink")]
    [SerializeField] private float shrink_time;
    private Action On_Shrink;
    private float shrink_timer;
    List<GameObject> car_in_scene = new List<GameObject>();
    private Vector3 original_scale;
    private Vector3 target_scale;
    public GameObject shrinkVFX;

    [Header("Coins")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Combo")]
    [SerializeField] private GameObject combo25Prefab;
    [SerializeField] public int comboCount = 0;
    public int ComboCount => comboCount;
    private GameObject lastComboCarSpawned;
    private void Start()
    {
        spawned_car = new List<GameObject>();
        car_crashed = new List<GameObject>();
        paths_dictionary = new Dictionary<Direction, Dictionary<Point, List<Path>>>();
        for (int i = 0; i < paths.Count; i++)
        {
            paths_dictionary[paths[i].Key] = new Dictionary<Point, List<Path>>();
            for (int j = 0; j < paths[i].Value.Count; j++)
            {
                paths_dictionary[paths[i].Key][paths[i].Value[j].Key] = paths[i].Value[j].Value;
            }
        }
        CarInfos = CarInfosRef.self.DefaultCarInfoData;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timer_to_spawn_car)
        {
            SpawnCar();
            timer = 0;
        }

        HandleComboSpawn();
        On_Invisibility?.Invoke();
        On_Shrink?.Invoke();
    }

    #region Spawner
    void SpawnCar()
    {
        if (spawned_car.Count > 0 && spawned_car.Count >= max_car_in_scene) return;

        Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
        Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
        int random_path = Random.Range(0, 2);
        Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];
        bool isKamikaze = true;

        int arrow_index = -1;
        if (randomPoint == Point.Left && random_path == 0)// 0 = per non andare dritto
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
        int random_index = Random.Range(0, CarInfos.Keys.Count);
        var data = CarInfos[(CarType)random_index][Random.Range(0, CarInfos[(CarType)random_index].Count)];

        GameObject car = Instantiate(car_prefab, pathRef.Nodes[0].position, Quaternion.identity, this.transform);
        car.GetComponent<Car_Core>().OnInitializedCar(pathRef, arrow_index, data, isKamikaze, invisibility_on);
        spawned_car.Add(car);
    }

    public void HandleComboSpawn()
    {
        if (comboCount % 25 == 0 && comboCount != 0)
        {
            if (lastComboCarSpawned == null)
            {
                int comboType = comboCount / 25;

                Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
                Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
                int random_path = Random.Range(0, 2);
                Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];
                GameObject comboCar = Instantiate(combo25Prefab, pathRef.Nodes[0].position, Quaternion.identity, this.transform);
                comboCar.GetComponentInChildren<Car_Core>().OnInitializedCar(pathRef, -1, CarInfos[CarType.BaseCar][0],false, invisibility_on);
                spawned_car.Add(comboCar);
                lastComboCarSpawned = comboCar;
                comboCar.GetComponent<CarComboSetup>().ActivateCars(comboType);
            }
        }
    }

    #endregion

    #region UpdateCarList
    public void RemoveCar(GameObject car)
    {
        spawned_car.Remove(car);
    }
    public void AddCrashedCar(GameObject car)
    {
        if (!car_crashed.Contains(car))
        {
            spawned_car.Remove(car);
            car_in_scene.Remove(car);
            car_crashed.Add(car);
        }
    }
    #endregion

    #region PowerUp
    #region Invisilibity
    void InvisibilityTimer()
    {
        invisibility_timer += Time.deltaTime;
        if (invisibility_timer >= invisibility_time)
        {
            invisibility_timer = 0;
            ToggleInvisibility();
            invisibility_on = false;
            On_Invisibility = null;
        }
    }
    public void ToggleInvisibility()
    {
        if (spawned_car.Count <= 0) return;
        foreach (var car in spawned_car)
        {
            if (car.transform.TryGetComponent(out Car_Core car_function))
            {
                car_function.EnableInvisiblity();
            }
        }
        invisibility_on = true;
        On_Invisibility = InvisibilityTimer;
    }
    #endregion
    #region Shrink
    public void ToggleShrink()
    {
        original_scale = Vector3.one;
        target_scale = original_scale * .5f;

        car_in_scene = spawned_car;
        foreach (var car in car_in_scene)
        {
            if (car.transform.TryGetComponent(out Car_Core car_function))
            {
                car_function.EnableShrink();
                Vector3 effectPosition = car.transform.position - new Vector3(0f, 1.5f, 0f);
                GameObject effect = Instantiate(shrinkVFX, effectPosition, Quaternion.identity);
                effect.transform.parent = car.transform;
                Destroy(effect, 1.5f);
            }
        }
        On_Shrink = ShrinkTimer;
    }

    private void InstantiateEffect(Vector3 position)
    {
        GameObject effect = Instantiate(shrinkVFX, position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    void ShrinkTimer()
    {
        shrink_timer += Time.deltaTime;
        if (shrink_timer >= shrink_time)
        {
            shrink_timer = 0;
            On_Shrink = null;
            return;
        }

        foreach (var car in car_in_scene)
        {
            car.transform.GetChild(0).localScale = Vector3.Slerp(car.transform.GetChild(0).localScale, target_scale, shrink_timer / shrink_time);
        }
    }


    #endregion
    #region Slowmo


    public void ToggleSlowDownGame(float slowdownDuration)
    {
        CustomLibrary.SetGlobalTimeDilation(0.3f);

        StartCoroutine(ResumeGameAfterDelay(slowdownDuration));
    }
    private IEnumerator ResumeGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        CustomLibrary.SetGlobalTimeDilation(1f);
        StopCoroutine(ResumeGameAfterDelay(delay));
    }

    #endregion
    #region Nuke
    public void toggleNuke(float explosionForce, float explosionRadius)
    {
        foreach (GameObject obj in spawned_car)
        {
            obj.layer = 6;
            obj.GetComponent<Rigidbody>().AddExplosionForce(explosionForce * Time.unscaledDeltaTime, transform.position, explosionRadius);
            Destroy(obj.GetComponent<CarFollowPath>());
            Destroy(obj, 2);
        }
        spawned_car.Clear();
    }
    #endregion
    #region Coins
    public int coinsAmount = 0;
    public void IncrementCoins()
    {
        coinsAmount++;
    }

    public void DropCoin(Vector3 position)
    {
        if (coinPrefab != null)
        {
            GameObject coins = Instantiate(coinPrefab, position, Quaternion.identity);
            coins.GetComponent<Rigidbody>().AddForce(Vector3.up * 5, ForceMode.Impulse);
            IncrementCoins();
        }
    }
    #endregion
    #endregion
}

#region Structs
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
#endregion