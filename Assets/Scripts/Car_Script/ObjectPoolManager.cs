using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    private Dictionary<GameObject, Queue<GameObject>> objectPools = new Dictionary<GameObject, Queue<GameObject>>();



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateObjectPool(GameObject prefab, int poolSize)
    {
        if (!objectPools.ContainsKey(prefab))
        {
            objectPools[prefab] = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, transform);

                Outline outlineComponent = obj.GetComponentInChildren<Outline>();
                if (outlineComponent != null)
                {
                    outlineComponent.enabled = false;
                }

                obj.SetActive(false);
                objectPools[prefab].Enqueue(obj);
            }
        }
    }

    public GameObject GetObjectFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (objectPools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;

                //Outline outlineComponent = obj.GetComponentInChildren<Outline>();
                //if (outlineComponent != null)
                //{
                //    outlineComponent.enabled = true;
                //}

                obj.SetActive(true);
                return obj;
            }
            else
            {
                Debug.LogWarning("Pool is empty for prefab: " + prefab.name);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Pool not found for prefab: " + prefab.name);
            return null;
        }
    }
    public void Enqueque(GameObject car)
    {
        if (objectPools.TryGetValue(car, out Queue<GameObject> pool))
        {
            if (pool.Count > 0)
            {
                pool.Enqueue(car);

                car.SetActive(false);
            }
            else
            {

            }
        }
        else
        {
        }
    }
}