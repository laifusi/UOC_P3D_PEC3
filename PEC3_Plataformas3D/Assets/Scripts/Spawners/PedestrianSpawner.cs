using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : Spawner
{
    [SerializeField] private Transform[] cityDestinations;
    [SerializeField] private Transform[] buildingDestinations;
    [SerializeField] private Transform[] safePoints;

    [SerializeField] private PedestrianAIController[] pedestrianPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float minTimeBetweenSpawns = 5;

    private float timeSinceLastSpawn;

    protected override bool ShouldSpawn()
    {
        timeSinceLastSpawn += Time.deltaTime;
        return timeSinceLastSpawn > minTimeBetweenSpawns;
    }

    protected override void Spawn()
    {
        int randomPrefab = Random.Range(0, pedestrianPrefabs.Length);
        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        PedestrianAIController pedestrian = Instantiate(pedestrianPrefabs[randomPrefab], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
        pedestrian.SetCityPoints(cityDestinations, buildingDestinations, safePoints);
        timeSinceLastSpawn = 0;
    }
}