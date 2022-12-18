using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : Spawner
{
    [SerializeField] private GameObject healthPackPrefab;
    [SerializeField] private GameObject ammoPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [Tooltip("Number of items of each type that should be in the world at all times")]
    [SerializeField] private int numberOfItems = 4;

    private int currentNumberOfHealthPacks;
    private int currentNumberOfAmmoBoxes;

    private int currentNumberOfItems => currentNumberOfHealthPacks + currentNumberOfAmmoBoxes;

    private List<Transform> emptySpawnPoints = new List<Transform>();
    private List<Transform> usedSpawnPoints = new List<Transform>();

    private void Start()
    {
        Item.OnItemPickedUp += NewEmptyPoint;
        foreach(Transform point in spawnPoints)
        {
            emptySpawnPoints.Add(point);
        }

        for(int i = 0; i < numberOfItems*2; i++)
        {
            Spawn();
        }
    }

    private void NewEmptyPoint(bool healthPack, bool ammo, Transform emptiedSpawnPoint)
    {
        usedSpawnPoints.Remove(emptiedSpawnPoint);
        currentNumberOfHealthPacks -= healthPack ? 1 : 0;
        currentNumberOfAmmoBoxes -= ammo ? 1 : 0;
    }

    protected override bool ShouldSpawn()
    {
        return currentNumberOfItems < numberOfItems*2;
    }

    protected override void Spawn()
    {
        if(currentNumberOfHealthPacks < numberOfItems)
        {
            SpawnItem(healthPackPrefab);
            currentNumberOfHealthPacks++;
        }
        else if(currentNumberOfAmmoBoxes < numberOfItems)
        {
            SpawnItem(ammoPrefab);
            currentNumberOfAmmoBoxes++;
        }
    }

    private void SpawnItem(GameObject prefab)
    {
        int randomId = Random.Range(0, emptySpawnPoints.Count);
        Transform randomUnusedSpawnPoint = emptySpawnPoints[randomId];
        emptySpawnPoints.RemoveAt(randomId);
        usedSpawnPoints.Add(randomUnusedSpawnPoint);
        Instantiate(prefab, randomUnusedSpawnPoint.position, Quaternion.identity, randomUnusedSpawnPoint);
    }

    private void OnDestroy()
    {
        Item.OnItemPickedUp -= NewEmptyPoint;
    }
}
