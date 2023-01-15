using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExitPoint : MonoBehaviour
{
    /// <summary>
    /// We check if an AI car entered the trigger
    /// If it did, we destroy it
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        CarManager car = other.GetComponentInParent<CarManager>();
        if (car != null && car.IsAICar())
        {
            car.UnspawnCar();
        }
    }
}
