using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : Spawner
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float initialTimeBetweenSpawns = 20f;
    [SerializeField] private float decreaseInEachSpawn = 0.05f;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float bossSpawnTime = 40f;

    private float timeSinceLastSpawn;
    private float timeBetweenSpawns;
    private float timeSinceLastBossSpawn;
    private int spawnedBosses;
    private List<Transform> activeSpawnPoints = new List<Transform>();

    public static System.Action OnNoActivePoints;

    private void Start()
    {
        ZombieDoor.OnDoorDestroyed += DoorDestroyed;

        foreach(Transform point in spawnPoints)
        {
            activeSpawnPoints.Add(point);
        }

        Spawn();
        timeBetweenSpawns = initialTimeBetweenSpawns;
    }

    protected void Update()
    {
        base.Update();

        if (ShouldSpawnBoss())
        {
            SpawnBoss();
        }

        timeSinceLastSpawn += Time.deltaTime;
        timeSinceLastBossSpawn += Time.deltaTime;
    }

    /// <summary>
    /// If there's still open spawn points and the time since last spawn is bigger than the established time between spawns, we should spawn
    /// </summary>
    /// <returns></returns>
    protected override bool ShouldSpawn()
    {
        return activeSpawnPoints.Count > 0 && timeSinceLastSpawn >= timeBetweenSpawns;
    }

    /// <summary>
    /// We spawn a zombie in a random spawn point
    /// </summary>
    protected override void Spawn()
    {
        int randomId = Random.Range(0, activeSpawnPoints.Count);
        Instantiate(zombiePrefab, activeSpawnPoints[randomId].position, Quaternion.identity);

        timeSinceLastSpawn = 0;
        timeBetweenSpawns -= decreaseInEachSpawn;
    }

    /// <summary>
    /// If not all bosses were spawned and the time since the last boss spawn is bigger than the established time between boss spawns, we should spawn a boss
    /// </summary>
    /// <returns></returns>
    private bool ShouldSpawnBoss()
    {
        return spawnedBosses < spawnPoints.Length && timeSinceLastBossSpawn >= bossSpawnTime;
    }

    /// <summary>
    /// Instantiate a boss in a random spawn point
    /// </summary>
    private void SpawnBoss()
    {
        spawnedBosses++;
        timeSinceLastBossSpawn = 0;
        int randomId = Random.Range(0, activeSpawnPoints.Count);
        Instantiate(bossPrefab, activeSpawnPoints[randomId].position, Quaternion.identity);
    }

    /// <summary>
    /// We update the active spawn points when a door is destroyed
    /// </summary>
    /// <param name="pointDestroyed">Transfomr of the destroyed door</param>
    private void DoorDestroyed(Transform pointDestroyed)
    {
        activeSpawnPoints.Remove(pointDestroyed);
        if(activeSpawnPoints.Count <= 0)
        {
            OnNoActivePoints?.Invoke();
        }
    }

    private void OnDestroy()
    {
        ZombieDoor.OnDoorDestroyed -= DoorDestroyed;
    }
}
