using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float minTimeInCar = 0.3f;
    [SerializeField] private Transform outOfCarPoint;

    private CarManager carManager;
    private float timeSinceActivated;

    public static Action<Transform> OnOutOfCar;

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
            OnOutOfCar?.Invoke(outOfCarPoint);
        }

        timeSinceActivated += Time.deltaTime;
    }
}