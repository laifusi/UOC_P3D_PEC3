using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float minTimeInCar = 0.3f;

    private CarManager carManager;
    private float timeSinceActivated;

    public static Action OnOutOfCar;

    private void Start()
    {
        carManager = GetComponentInParent<CarManager>();
    }

    private void OnEnable()
    {
        timeSinceActivated = 0;
    }

    private void Update()
    {
        if (timeSinceActivated > minTimeInCar && Input.GetKeyDown("e"))
        {
            carManager.StartStaticCar();
            OnOutOfCar?.Invoke();
        }

        timeSinceActivated += Time.deltaTime;
    }
}
