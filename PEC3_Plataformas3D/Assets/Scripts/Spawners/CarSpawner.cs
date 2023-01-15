using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : Spawner
{
    [SerializeField] private CarManager[] carPrefabs;
    [SerializeField] private CarLine[] carLines;
    [SerializeField] private float minTimeBetweenSpawns = 10;

    private float timeSinceLastSpawn;

    /// <summary>
    /// We check if it's been long enough to spawn a new car
    /// </summary>
    protected override bool ShouldSpawn()
    {
        timeSinceLastSpawn += Time.deltaTime;
        return timeSinceLastSpawn > minTimeBetweenSpawns;
    }

    /// <summary>
    /// We spawn a new random car in a random CarLine
    /// We set the circuit and start the AI car
    /// </summary>
    protected override void Spawn()
    {
        timeSinceLastSpawn = 0;
        int randomCarLine = Random.Range(0, carLines.Length);
        int randomCarPrefab = Random.Range(0, carPrefabs.Length);
        CarManager car = Instantiate(carPrefabs[randomCarPrefab], carLines[randomCarLine].getEntryPoint(), carLines[randomCarLine].getEntryRotation());
        car.SetWayPoints(carLines[randomCarLine].GetWayPoints());
        car.StartAICar();
    }
}
