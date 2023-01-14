using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingPointUI : MonoBehaviour
{
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();

        Car.OnOutOfCar += ActivateIndicator;
        Driver.OnEnterCar += DeactivateIndicator;
    }

    private void DeactivateIndicator()
    {
        image.enabled = false;
    }

    private void ActivateIndicator(Transform outPosition)
    {
        image.enabled = true;
    }

    private void Update()
    {
        transform.position = Input.mousePosition;
    }

    private void OnDestroy()
    {
        Car.OnOutOfCar -= ActivateIndicator;
        Driver.OnEnterCar -= DeactivateIndicator;
    }
}
