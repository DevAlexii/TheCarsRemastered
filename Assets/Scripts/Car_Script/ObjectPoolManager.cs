using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectPoolManager : MonoBehaviour
{
    public static List<PooledObjectData> _ObjectPool = new List<PooledObjectData>();

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnToration)
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
            spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnToration);
        }
        else
        {
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnToration;
            pool.InactiveObject.Remove(spawnableObject);
            spawnableObject.gameObject.SetActive(false);
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
            obj.SetActive(true);
            pool.InactiveObject.Add(obj);
        }

    }
}

public class PooledObjectData
{
    public string LookUpString;
    public List<GameObject> InactiveObject = new List<GameObject>();
}