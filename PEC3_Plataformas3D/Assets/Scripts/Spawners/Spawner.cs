using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    protected void Update()
    {
        if(ShouldSpawn())
        {
            Spawn();
        }
    }

    protected abstract bool ShouldSpawn();

    protected abstract void Spawn();
}
