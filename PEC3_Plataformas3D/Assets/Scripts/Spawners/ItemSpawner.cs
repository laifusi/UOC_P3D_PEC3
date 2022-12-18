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
        ZombieAIController.OnItemDropped += NewItemDropped;

        foreach(Transform point in spawnPoints)
        {
            emptySpawnPoints.Add(point);
        }

        for(int i = 0; i < numberOfItems*2; i++)
        {
            Spawn();
        }
    }

    /// <summary>
    /// Method to update the counters when an item is picked up
    /// </summary>
    /// <param name="healthPack">Boolean that indicates if the item was a health pack</param>
    /// <param name="ammo">Boolean that indicates if the item was an ammo box</param>
    /// <param name="emptiedSpawnPoint">Transform of the spawn point that got emptied</param>
    private void NewEmptyPoint(bool healthPack, bool ammo, Transform emptiedSpawnPoint)
    {
        usedSpawnPoints.Remove(emptiedSpawnPoint);
        currentNumberOfHealthPacks -= healthPack ? 1 : 0;
        currentNumberOfAmmoBoxes -= ammo ? 1 : 0;
    }

    /// <summary>
    /// Method to update the counters when an item is spawned outside of the ItemSpawner (by a zombie death drop)
    /// </summary>
    /// <param name="isHealthPack">Boolean that indicates if the item was a health pack</param>
    /// <param name="isAmmo">Boolean that indicates if the item was an ammo box</param>
    /// <param name="isKey">Boolean that indicates if the item was a key</param>
    private void NewItemDropped(bool isHealthPack, bool isAmmo, bool isKey)
    {
        if(isHealthPack)
        {
            currentNumberOfHealthPacks++;
        }
        else if(isAmmo)
        {
            currentNumberOfAmmoBoxes++;
        }
    }

    /// <summary>
    /// If the current number of items is lower than the minimum, we should spawn
    /// </summary>
    protected override bool ShouldSpawn()
    {
        return emptySpawnPoints.Count > 0 && currentNumberOfItems < numberOfItems*2;
    }

    /// <summary>
    /// We check which type of item was picked up and spawn it
    /// </summary>
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

    /// <summary>
    /// We instantiate the chosen prefab in a random point that is empty
    /// </summary>
    /// <param name="prefab">Item that should be spawned</param>
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
        ZombieAIController.OnItemDropped -= NewItemDropped;
    }
}
