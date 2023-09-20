using UnityEngine;

public class PowerUpManager : Singleton<PowerUpManager>
{
    [Header("PowerUp Prefab")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject powerUP_Health;

    [Header("Spawn Timer")]
    [SerializeField] private int spawn_timer;
    private float timer;

    [Header("SpawnPoint")]
    [SerializeField] private Transform[] spawn_points;

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
        int spawn_random_index = Random.Range(0, spawn_points.Length);

        GameObject.Instantiate(powerUps[powerUp_random_index], spawn_points[spawn_random_index].position, Quaternion.identity, this.transform);
    }
    public void SpawnHealth()
    {
        int spawn_random_index = Random.Range(0, spawn_points.Length);
        GameObject.Instantiate(powerUP_Health, spawn_points[spawn_random_index].position, Quaternion.identity, this.transform);
    }
}
