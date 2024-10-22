using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float minTimeOutOfCar = 0.3f; // Float to avoid reading exiting car input as entering car input

    private float timeSinceActivated;

    public static Action OnEnterCar;

    private void OnEnable()
    {
        timeSinceActivated = 0;
    }

    /// <summary>
    /// We check if the player is inside the car's trigger and pressed the E key to enter the car
    /// We deactivate the player's camera
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        CarManager car = other.GetComponentInParent<CarManager>();
        if (timeSinceActivated > minTimeOutOfCar && car != null && Input.GetKey("e"))
        {
            OnEnterCar?.Invoke();
            car.StartPlayerCar();
            cam.SetActive(false);
            gameObject.SetActive(false);
        }

        timeSinceActivated += Time.deltaTime;
    }
}
