using UnityEngine;
using System.Collections.Generic;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [Header("PowerUp Prefab")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject powerUP_Health;

    [Header("Spawn Timer")]
    [SerializeField] private int spawn_timer;
    private float timer;

    [Header("SpawnPoint")]
    [SerializeField] private List<Transform> spawn_points;
    private Transform saved_sp;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawn_timer)
        {
            timer = 0;
            SpawnPowerUp();
        }
    }
    private void SpawnPowerUp()
    {
        int powerUp_random_index = Random.Range(0, powerUps.Length);
        int spawn_random_index = Random.Range(0, spawn_points.Count);

        GameObject.Instantiate(powerUps[powerUp_random_index], spawn_points[spawn_random_index].position, Quaternion.identity, this.transform);
        FixSpawnPoints(spawn_random_index);
    }
    private void FixSpawnPoints(int spawn_random_index)
    {
        if (saved_sp != null)
        {
            spawn_points.Add(saved_sp);
        }
        saved_sp = spawn_points[spawn_random_index];
        spawn_points.Remove(saved_sp);
    }
    public void SpawnHealth()
    {
        int spawn_random_index = Random.Range(0, spawn_points.Count);
        GameObject.Instantiate(powerUP_Health, spawn_points[spawn_random_index].position, Quaternion.identity, this.transform);
        FixSpawnPoints(spawn_random_index);
    }
}
