using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectData> _ObjectPool = new List<PooledObjectData>();

    private GameObject emptyTransform;

    private static GameObject cars; 
    private static GameObject vfx;


    public enum ObjectType
    {
        Car,
        VFX,
        None
    }
    public static ObjectType PoolingType;


    private void Awake()
    {
        emptyTransform = new GameObject("Pooled Object in Scene");

        cars = new GameObject("Cars pooled");
        cars.transform.SetParent(emptyTransform.transform);

        vfx = new GameObject("VFX pooled");
        vfx.transform.SetParent(emptyTransform.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnToration,ObjectType type = ObjectType.None)
    {
        PooledObjectData pool = null;
        foreach (PooledObjectData obj in _ObjectPool)
        {
            if (obj.LookUpString == objectToSpawn.name)
            {
                pool = obj;
                break;
            }
        }
        if (pool == null)
        {
            pool = new PooledObjectData() { LookUpString = objectToSpawn.name };
            _ObjectPool.Add(pool);
        }

        GameObject spawnableObject = pool.InactiveObject.FirstOrDefault();


        if (spawnableObject == null)
        {
            GameObject parentObj = SetParentObject(type);
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnToration);

            if (parentObj != null)
            {
                spawnableObject.transform.SetParent(parentObj.transform);
            }
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnToration;
            pool.InactiveObject.Remove(spawnableObject);
            spawnableObject.gameObject.SetActive(true);
        }
        return spawnableObject;
    }

    //Destroy
    public static void ReturnObjectToPool(GameObject obj)
    {
        string name = obj.name.Substring(0, obj.name.Length - 7);

        PooledObjectData pool = _ObjectPool.Find(p => p.LookUpString == name);

        if (pool == null)
        {

        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObject.Add(obj);
        }

    }

    private static GameObject SetParentObject(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Car:
                return cars;
            case ObjectType.VFX:
                return vfx;
            case ObjectType.None:
                return null;
                default:
                return null;
        }
    }

}

public class PooledObjectData
{
    public string LookUpString;
    public List<GameObject> InactiveObject = new List<GameObject>();
}