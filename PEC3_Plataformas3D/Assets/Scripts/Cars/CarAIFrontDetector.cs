using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarAIFrontDetector : MonoBehaviour
{
    [SerializeField] CarAIControl carControl;

    private void OnTriggerEnter(Collider other)
    {
        CarBlockDetector car = other.GetComponent<CarBlockDetector>();
        PedestrianAIController pedestrian = other.GetComponent<PedestrianAIController>();
        Health player = other.GetComponent<Health>();

        if(car != null || pedestrian != null || player != null)
        {
            carControl.SetDrivingBool(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CarBlockDetector car = other.GetComponent<CarBlockDetector>();
        PedestrianAIController pedestrian = other.GetComponent<PedestrianAIController>();
        Health player = other.GetComponent<Health>();

        if (car != null || pedestrian != null || player != null)
        {
            carControl.SetDrivingBool(false);
        }

        if(car != null)
        {
            car.GetNotifiedOfRoadBlocking();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarBlockDetector car = other.GetComponent<CarBlockDetector>();
        PedestrianAIController pedestrian = other.GetComponent<PedestrianAIController>();
        Health player = other.GetComponent<Health>();

        if (car != null || pedestrian != null || player != null)
        {
            carControl.SetDrivingBool(true);
        }

        if (car != null)
        {
            car.GetNotifiedOfBlockingEnd();
        }
    }
}
