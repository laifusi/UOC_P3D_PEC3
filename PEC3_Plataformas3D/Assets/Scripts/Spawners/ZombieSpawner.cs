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

    private void Start()
    {
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
        int randomId = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePrefab, spawnPoints[randomId].position, Quaternion.identity);

        timeSinceLastSpawn = 0;
        timeBetweenSpawns -= decreaseInEachSpawn;
    }
}
