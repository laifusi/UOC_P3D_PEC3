using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : Spawner
{
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float initialTimeBetweenSpawns = 20f;
    [SerializeField] private float decreaseInEachSpawn = 0.05f;

    private float timeSinceLastSpawn;
    private float timeBetweenSpawns;
    private List<Transform> activeSpawnPoints = new List<Transform>();

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

        timeSinceLastSpawn += Time.deltaTime;
    }

    protected override bool ShouldSpawn()
    {
        return timeSinceLastSpawn >= timeBetweenSpawns;
    }

    protected override void Spawn()
    {
        int randomId = Random.Range(0, activeSpawnPoints.Count);
        Instantiate(zombiePrefab, activeSpawnPoints[randomId].position, Quaternion.identity);

        timeSinceLastSpawn = 0;
        timeBetweenSpawns -= decreaseInEachSpawn;
    }

    private void DoorDestroyed(Transform pointDestroyed)
    {
        activeSpawnPoints.Remove(pointDestroyed);
    }

    private void OnDestroy()
    {
        ZombieDoor.OnDoorDestroyed -= DoorDestroyed;
    }
}
