using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car_Manager : Singleton<Car_Manager>
{
    [SerializeField] List<Path_Dictionary> paths;
    [SerializeField] private float timer_to_spawn_car;
    public Dictionary<CarType, List<CarInfo>> CarInfos;
    [SerializeField] private Int32 max_car_in_scene;
    [SerializeField] private Int32 percentage_to_be_kamikaze;
    private float timer = float.MaxValue;
    float start_count = 5;
    [SerializeField]
    private AnimationCurve timer_curve;
    [SerializeField]
    private AnimationCurve car_wait_timer_curve;


    public List<GameObject> spawned_car;
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
    public GameObject nukeVFX;

    [Header("Coins")]
    [SerializeField] private GameObject coinPrefab;

    [Header("Combo")]
    [SerializeField] private GameObject combo25Prefab;
    [HideInInspector] public int comboCount = 0;
    [SerializeField]
    private int combo_num;
    [SerializeField] private bool spawn_combo;

    [Header("Camera")]
    public CameraShake cameraShake;

    public int ComboCount => comboCount;
    private GameObject lastComboCarSpawned;


    public GameObject baseCarPrefab;
    public GameObject tirPrefab;
    public GameObject specialPrefab;
    public GameObject bigVanPrefab;
    public GameObject vanPrefab;
    public GameObject kamikazePrefab;

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
        ObjectPoolManager.instance.CreateObjectPool(baseCarPrefab, 10);
        ObjectPoolManager.instance.CreateObjectPool(tirPrefab, 5);
        ObjectPoolManager.instance.CreateObjectPool(specialPrefab, 5);
        ObjectPoolManager.instance.CreateObjectPool(bigVanPrefab, 5);
        ObjectPoolManager.instance.CreateObjectPool(vanPrefab, 5);
        ObjectPoolManager.instance.CreateObjectPool(kamikazePrefab, 5);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timer_to_spawn_car + timer_curve.Evaluate(start_count))
        {
            SpawnCar();
            timer = 0;
            start_count -= 0.1f;
            if (start_count <= 0)
            {
                start_count = 0;
            }
        }

        On_Invisibility?.Invoke();
        On_Shrink?.Invoke();
    }

    #region Spawner
    [Header("Spawn Percentage")]
    public int percentage_tir;
    public int percentage_special;
    public int percentage_big_van;
    public int percentage_van;


    private Direction last_spawn_direction = Direction.Last;
    void SpawnCar()
    {
        if (spawned_car.Count > 0 && spawned_car.Count >= max_car_in_scene) return;

        Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
        if (last_spawn_direction != randomDirection)
        {
            last_spawn_direction = randomDirection;
        }
        else
        {
            if (randomDirection == 0)
            {
                randomDirection = Direction.Last - 1;
            }
            else
            {
                randomDirection -= 1;
            }
        }

        Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
        int random_path = Random.Range(0, 2);
        Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];
        bool isKamikaze = true;
        int arrow_index = -1;

        if (spawn_combo)
        {
            isKamikaze = false;
        }
        else
        {
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
        }


        CarInfo data = null;
        if (isKamikaze)
        {
            isKamikaze = CustomLibrary.RandomBoolInPercentage(percentage_to_be_kamikaze);
        }
        if (isKamikaze)
        {
            data = CarInfosRef.self.GetKamikazeInfo;
        }
        else
        {
            CarType random_key = CarType.BaseCar;
            int random_car_type = Random.Range(0, 100);

            if (random_car_type <= percentage_tir)
            {
                random_key = CarType.Tir;
            }
            else if (random_car_type <= percentage_special)
            {
                random_key = CarType.Special;
            }
            else if (random_car_type <= percentage_big_van)
            {
                random_key = CarType.BigVan;
            }
            else if (random_car_type <= percentage_van)
            {
                random_key = CarType.Van;
            }

            int random_value = Random.Range(0, CarInfos[random_key].Count);

            data = CarInfos[random_key][random_value];

        }


        GameObject car = ObjectPoolManager.instance.GetObjectFromPool(data.BasePrefab, pathRef.Nodes[0].position, Quaternion.identity);
        car.GetComponentInChildren<Car_Core>().OnInitializedCar(pathRef, arrow_index, data, isKamikaze, invisibility_on, car_wait_timer_curve.Evaluate(start_count));
        spawned_car.Add(car);
        if (isKamikaze)
        {
            AudioCallBack.self.PlayAudio(AudioType.ToothMeme, 1f);
        }
    }
    public void HandleComboSpawn()
    {
        //if (lastComboCarSpawned == null)
        //{
        int comboType = comboCount / combo_num;

        Direction randomDirection = (Direction)Random.Range(0, (int)Direction.Last);
        Point randomPoint = (Point)Random.Range(0, (int)Point.Last);
        int random_path = Random.Range(0, 2);
        Path pathRef = paths_dictionary[randomDirection][randomPoint][random_path];
        GameObject comboCar = Instantiate(combo25Prefab, pathRef.Nodes[0].position, Quaternion.identity, this.transform);
        comboCar.GetComponentInChildren<Car_Core>().OnInitializedCar(pathRef, -1, CarInfos[CarType.BaseCar][0], false, invisibility_on, car_wait_timer_curve.Evaluate(start_count), comboType + 1);
        spawned_car.Add(comboCar);
        lastComboCarSpawned = comboCar;
        comboCar.GetComponent<CarComboSetup>().ActivateCars(comboType);
        spawn_combo = false;
        //}
    }
    public void Increment_score_count(bool increment = true)
    {
        if (increment)
        {
            comboCount++;
            if (comboCount % combo_num == 0 && !spawn_combo)
            {
                spawn_combo = true;
            }
        }
        else
        {
            if (comboCount > combo_num)
            {
                spawn_combo = true;
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
            car_crashed.Add(car);
        }
        spawned_car.Remove(car);
        car_in_scene.Remove(car);
    }

    public void RemoveCarFromLists(GameObject car)
    {
        spawned_car.Remove(car);
        car_in_scene.Remove(car);
        car_crashed.Remove(car);
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
            car.GetComponentInChildren<Car_Core>().EnableInvisiblity();
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
            car.GetComponentInChildren<Car_Core>().EnableShrink();
            Vector3 effectPosition = car.transform.position - new Vector3(0f, .8f, 0f);
            GameObject effect = Instantiate(shrinkVFX, effectPosition, Quaternion.identity);
            effect.transform.parent = car.transform;
            Destroy(effect, 1.5f);
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
            Rigidbody rb = obj.GetComponentInChildren<Rigidbody>();
            rb.AddExplosionForce(explosionForce * Time.unscaledDeltaTime, transform.position, explosionRadius);
            rb.excludeLayers = GameManager.self.layer_to_exclude;
            Destroy(obj.GetComponent<CarFollowPath>());
            GameObject effect = Instantiate(nukeVFX, new Vector3(0, 1, 0), Quaternion.identity);
            cameraShake.StartShake();
            Destroy(effect, 3f);
            Destroy(obj, 2);
        }
        spawned_car.Clear();
    }
    #endregion
    #region Coins
    [HideInInspector] public int coinsAmount = 0;
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