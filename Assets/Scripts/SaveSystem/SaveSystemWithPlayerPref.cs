using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemWithPlayerPref : Singleton<SaveSystemWithPlayerPref>
{
    [SerializeField] private List<cars_to_id> pool_cars_editor;
    private Dictionary<int, GameObject> pool_cars;
    [SerializeField] private List<cars_to_id> default_pool_car_editor;
    private Dictionary<int, GameObject> default_pool_cars;
    public List<GameObject> owner_pool_car { get; private set; }
    public List<int> saved_cars_id { get; private set; }


    private void Awake()
    {
        pool_cars = new Dictionary<int, GameObject>();
        for (int i = 0; i < pool_cars_editor.Count; i++)
        {
            pool_cars[pool_cars_editor[i].id_key] = pool_cars_editor[i].car_prefab_value;
        }
        default_pool_cars = new Dictionary<int, GameObject>();
        for (int i = 0; i < default_pool_car_editor.Count; i++)
        {
            default_pool_cars[default_pool_car_editor[i].id_key] = default_pool_car_editor[i].car_prefab_value;
        }
        Load();
        DontDestroyOnLoad(this.gameObject);
    }

    public void SaveCarId(int car_id)
    {
        int last_count = PlayerPrefs.GetInt("Car_Ids_Count");
        PlayerPrefs.SetInt("Car_Ids_Count", last_count + 1);
        PlayerPrefs.SetInt("Car_Ids_List_" + last_count, car_id);
        Load();
    }
    public void Load()
    {
        saved_cars_id = new List<int>();

        int count = PlayerPrefs.GetInt("Car_Ids_Count");

        for (int i = 0; i < count; i++)
        {
            saved_cars_id.Add(PlayerPrefs.GetInt("Car_Ids_List_" + i));
        }

        if (saved_cars_id.Count > 0)
        {
            owner_pool_car = new List<GameObject>();
            foreach (int id in saved_cars_id)
            {
                owner_pool_car.Add(pool_cars[id]);
            }
            if (Car_Manager.self != null)
            {
                Car_Manager.self.car_prefabs = owner_pool_car;
            }
        }
        else
        {
            owner_pool_car = new List<GameObject>();

            PlayerPrefs.SetInt("Car_Ids_Count", default_pool_cars.Keys.Count);
            foreach (int key in default_pool_cars.Keys)
            {
                PlayerPrefs.SetInt("Car_Ids_List_" + key, key);
                owner_pool_car.Add(default_pool_cars[key]);
            }
        }
    }
    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }

    [Serializable]
    private struct cars_to_id
    {
        public int id_key;
        public GameObject car_prefab_value;
    }
}