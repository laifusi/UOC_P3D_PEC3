using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarExitPoint : MonoBehaviour
{
    [SerializeField] private CarLine line;

    public static Action<CarLine> OnCarLineEmptied;

    private void OnTriggerEnter(Collider other)
    {
        CarManager car = other.GetComponentInParent<CarManager>();
        if (car != null && car.IsAICar())
        {
            OnCarLineEmptied?.Invoke(line);
            car.UnspawnCar();
        }
    }
}
