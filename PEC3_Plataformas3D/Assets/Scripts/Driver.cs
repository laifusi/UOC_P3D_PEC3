using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float minTimeOutOfCar = 0.3f;

    private float timeSinceActivated;

    public static Action OnEnterCar;

    private void OnEnable()
    {
        timeSinceActivated = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        CarManager car = other.GetComponent<CarManager>();
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
